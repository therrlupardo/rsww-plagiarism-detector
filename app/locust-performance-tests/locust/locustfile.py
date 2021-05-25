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

START_TEST = [False]

DEBUG = False

# with client.get("/does_not_exist/", catch_response=True) as response:
#     if response.status_code == 404:
#         response.success()


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
            self._login()

        if len(USERS_CREDENTIALS_LIST) == 0:
            START_TEST[0] = True
            logging.info(f'################## TEST STARTED ##################')

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

            if DEBUG:
              logging.info(f'Upload test file ({filename})')

    @task(1000)
    def request_test_files_list(self):
        if START_TEST[0]:
            headers = copy.deepcopy(self.headers)
            headers['Content-Type'] = 'application/json'

            self.client.request('GET', '/api/analysis/all', headers=headers)

            if DEBUG:
              logging.info('Request - get test files list')

    @task(100)
    def upload_model_file(self):
        if START_TEST[0] and len(FILES_LIST) > 0:
            file, filename = _load_file()

            headers = copy.deepcopy(self.headers)

            self.client.request('POST', '/api/sources/create', files=file, headers=headers)

            if DEBUG:
              logging.info(f'Upload model file ({filename})')

    @task(1000)
    def request_model_files_list(self):
        if START_TEST[0]:
            headers = copy.deepcopy(self.headers)
            headers['Content-Type'] = 'application/json'

            self.client.request('GET', '/api/sources/all', headers=headers)

            if DEBUG:
              logging.info('Request - get model files list')

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

            if DEBUG:
              logging.info(f'Request - analyze file with file_id ({file_id})')

    @task(2000)
    def request_test_file_analysis_status(self):
        if START_TEST[0] and len(TASKS_IDS[self.login]) > 0:
            task_id = TASKS_IDS[self.login].pop()

            headers = copy.deepcopy(self.headers)
            headers['Content-Type'] = 'application/json'

            response = self.client.request('GET', f'/api/analysis/{task_id}', headers=headers)

            if DEBUG:
              logging.info(f'Request - get test file analysis status with task_id ({task_id})')


class User(HttpUser):
    tasks = [LoggedInUserSteps]
    wait_time = between(2,5)
    sock = None

    def on_start(self):
        self.client.verify = False  # allow self-signed certificates
