from locust import HttpUser, TaskSet, task, between
import logging
import json
import os
import copy

from settings import USERS_CREDENTIALS, FILES_DATA, FILES_DIRECTORY
from utils.utils import read_data_from_csv_file
import urllib3

urllib3.disable_warnings(
    urllib3.exceptions.InsecureRequestWarning)  # suppress warnings about usage of self-signed certificates

USERS_CREDENTIALS_LIST = read_data_from_csv_file(USERS_CREDENTIALS)
FILES_LIST = read_data_from_csv_file(FILES_DATA)

USERS_TOKENS = dict((user[0], None) for user in USERS_CREDENTIALS_LIST)
FILES_IDS = dict((user[0], []) for user in USERS_CREDENTIALS_LIST)
TASKS_IDS = dict((user[0], []) for user in USERS_CREDENTIALS_LIST)

USERS_TOKENS['NOT_FOUND'] = None
FILES_IDS['NOT_FOUND'] = []
TASKS_IDS['NOT_FOUND'] = []

COUNTER_UPLOAD_TEST_FILES = []
COUNTER_REQUEST_TEST_FILES_LIST = []
COUNTER_UPLOAD_MODEL_FILES = []
COUNTER_REQUEST_MODEL_FILES = []
COUNTER_REQUEST_TEST_FILE_ANALYSIS = []
COUNTER_REQUEST_TEST_FILE_ANALYSIS_STATUS = []

START_TEST = [False]


def _get_image_part(path, file_content_type='text/x-python'):
    filename = os.path.basename(path)
    file_content = open(path, 'rb')
    return filename, file_content, file_content_type


def _load_file():
    filename = FILES_LIST.pop()[0]
    filepath = os.path.join(FILES_DIRECTORY, filename)
    file = {
        'file': _get_image_part(filepath),
    }

    return file, filename


class LoggedInUserSteps(TaskSet):
    login = 'NOT_FOUND'
    password = 'NOT_FOUND'

    headers = {
        'Accept': 'application/json'
    }

    def on_start(self):
        if len(USERS_CREDENTIALS_LIST) > 0:

            self.login, self.password = USERS_CREDENTIALS_LIST.pop()

            #if not self._users_created():
            #   self._register()

            self._login()

        if len(USERS_CREDENTIALS_LIST) == 0:
            START_TEST[0] = True
            logging.info(f'################## TEST STARTED ##################')

    def _users_created(self):
        response = self.client.request('GET', '/api/identity/all', headers=self.headers)
        users_list = json.loads(response.text)
        return len(users_list) != 1
        return len(users_list) == 1

    def _register(self):
        headers = copy.deepcopy(self.headers)
        headers['Content-Type'] = 'application/json'
        body = {
            'login': self.login,
            'password': self.password,
        }
        self.client.request('POST', '/api/identity/create', data=json.dumps(body), headers=headers)
        logging.info(f'Register with ({self.login}) login and ({self.password}) password')

    def _login(self):
        headers = copy.deepcopy(self.headers)
        headers['Content-Type'] = 'application/json'
        body = {
            'login': self.login,
            'password': self.password,
        }
        response = self.client.request('POST', '/api/identity/login', data=json.dumps(body), headers=headers)

        response = json.loads(response.text)
        token = response['accessToken']
        self.headers['Authorization'] = f'Bearer {token}'

        logging.info(f'Login with ({self.login}) login and ({self.password}) password')

    @task(50)
    def upload_test_file(self):
        if START_TEST[0] and len(FILES_LIST) > 0:
            file, filename = _load_file()

            headers = copy.deepcopy(self.headers)

            response = self.client.request('POST', '/api/analysis/send', files=file, headers=headers)

            response = json.loads(response.text)
            file_id = response['fileId']
            FILES_IDS[self.login].append(file_id)

            # logging.info(f'Upload test file ({filename})')
            COUNTER_UPLOAD_TEST_FILES.append(True)

    @task(1000)
    def request_test_files_list(self):
        if START_TEST[0]:
            headers = copy.deepcopy(self.headers)
            headers['Content-Type'] = 'application/json'

            self.client.request('GET', '/api/analysis/all', headers=headers)
            # logging.info('Request - get test files list')
            COUNTER_REQUEST_TEST_FILES_LIST.append(True)

    @task(100)
    def upload_model_file(self):
        if START_TEST[0] and len(FILES_LIST) > 0:
            file, filename = _load_file()

            headers = copy.deepcopy(self.headers)

            self.client.request('POST', '/api/sources/create', files=file, headers=headers)

            # logging.info(f'Upload model file ({filename})')
            COUNTER_UPLOAD_MODEL_FILES.append(True)

    @task(1000)
    def request_model_files_list(self):
        if START_TEST[0]:
            headers = copy.deepcopy(self.headers)
            headers['Content-Type'] = 'application/json'

            self.client.request('GET', '/api/sources/all', headers=headers)
            # logging.info('Request - get model files list')
            COUNTER_REQUEST_MODEL_FILES.append(True)

    @task(2000)
    def request_test_file_analysis(self):
        if START_TEST[0] and len(FILES_IDS[self.login]) > 0:
            file_id = FILES_IDS[self.login].pop()

            headers = copy.deepcopy(self.headers)
            headers['Content-Type'] = 'application/json'

            response = self.client.request('POST', f'/api/analysis/{file_id}/start', headers=headers)

            response = json.loads(response.text)
            task_id = response['taskId']
            TASKS_IDS[self.login].append(task_id)

            # logging.info(f'Request - analyze file with file_id ({file_id})')
            COUNTER_REQUEST_TEST_FILE_ANALYSIS.append(True)

    @task(2000)
    def request_test_file_analysis_status(self):
        if START_TEST[0] and len(TASKS_IDS[self.login]) > 0:
            task_id = TASKS_IDS[self.login].pop()

            headers = copy.deepcopy(self.headers)
            headers['Content-Type'] = 'application/json'

            response = self.client.request('GET', f'/api/analysis/{task_id}', headers=headers)
            logging.info(f'{json.loads(response.text)}')
            # logging.info(f'Request - get test file analysis status with task_id ({task_id})')

    # @task
    # def stop(self):
    #     uploaded_test_files = len(COUNTER_UPLOAD_TEST_FILES) > 50
    #     requested_test_files_list = len(COUNTER_REQUEST_TEST_FILES_LIST) > 1000
    #     uploaded_model_files = len(COUNTER_UPLOAD_MODEL_FILES) > 100
    #     requested_model_files = len(COUNTER_REQUEST_MODEL_FILES) > 1000
    #     requested_test_file_analysis = len(COUNTER_REQUEST_TEST_FILE_ANALYSIS) > 1000
    #     requested_test_file_analysis_status = len(COUNTER_REQUEST_TEST_FILE_ANALYSIS_STATUS) > 2000

    #     if uploaded_test_files and requested_test_files_list and uploaded_model_files and requested_model_files and requested_test_file_analysis and requested_test_file_analysis_status:
    #         logging.info(f'################## TEST FINISHED ##################')
    #         self.user.environment.reached_end = True
    #         self.user.environment.runner.quit()


class User(HttpUser):
    tasks = [LoggedInUserSteps]
    wait_time = between(2,5)
    sock = None

    def on_start(self):
        self.client.verify = False  # allow self-signed certificates
