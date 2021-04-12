from locust import HttpUser, TaskSet, task
import logging
import json

from settings import USERS_CREDENTIALS, TEST_FILES, MODEL_FILES
from utils.utils import read_data_from_csv_file
import urllib3

urllib3.disable_warnings(
    urllib3.exceptions.InsecureRequestWarning)  # suppress warnings about usage of self-signed certificates

USERS_CREDENTIALS_LIST = read_data_from_csv_file(USERS_CREDENTIALS)
TEST_FILES_LIST = read_data_from_csv_file(TEST_FILES)
MODEL_FILES_LIST = read_data_from_csv_file(MODEL_FILES)

USERS_TOKENS = dict((user[0], None) for user in USERS_CREDENTIALS_LIST)
FILES_IDS = dict((user[0], []) for user in USERS_CREDENTIALS_LIST)
TASKS_IDS = dict((user[0], []) for user in USERS_CREDENTIALS_LIST)


class LoggedInUserSteps(TaskSet):
    login = "NOT_FOUND"
    password = "NOT_FOUND"

    headers = {
        'Content-type': 'application/json',
        'Accept': 'application/json'
    }

    def on_start(self):
        if len(USERS_CREDENTIALS_LIST) > 0:
            self.login, self.password = USERS_CREDENTIALS_LIST.pop()

            if not self._users_created():
                self._register()

            self._login()

    def _users_created(self):
        response = self.client.request('GET', '/api/identity/all', headers=self.headers)
        users_list = json.loads(response.text)
        return len(users_list) != 1

    def _register(self):
        body = {
            'login': self.login,
            'password': self.password,
        }
        self.client.request('POST', '/api/identity/create', data=json.dumps(body), headers=self.headers)
        logging.info(f'Register with ({self.login}) login and ({self.password}) password')

    def _login(self):
        body = {
            'login': self.login,
            'password': self.password,
        }
        response = self.client.request('POST', '/api/identity/login', data=json.dumps(body), headers=self.headers)

        response = json.loads(response.text)
        token = response['accessToken']
        self.headers['Authorization'] = f'Bearer {token}'

        logging.info(f'Login with ({self.login}) login and ({self.password}) password')

    # @task(50)
    # def upload_test_file(self):
    #     file_id = self.client.post('/api/analysis/send')
    #     file_id = file_id['fileId']
    #     FILES_IDS[self.login].append(file_id)
    #     logging.info('Upload test file (name)')

    @task(1000)
    def request_test_files_list(self):
        self.client.request('GET', '/api/analysis/all', headers=self.headers)
        logging.info('Request - get test files list')

    # @task(100)
    # def upload_model_file(self):
    #     self.client.post('/api/sources/create')
    #     logging.info('Upload model file (name)')
    #
    @task(1000)
    def request_model_files_list(self):
        self.client.request('GET', '/api/sources/all', headers=self.headers)
        logging.info('Request - get model files list')
    #
    # @task(2000)
    # def request_test_file_analysis(self):
    #     file_id = FILES_IDS[self.login].pop()
    #     task_id = self.client.post(f'/api/analysis/{file_id}/start')
    #     task_id = task_id['taskId']
    #     TASKS_IDS[self.login].append(task_id)
    #     logging.info('Request test file (name) analysis')
    #
    # @task(2000)
    # def request_test_file_analysis_status(self):
    #     task_id = TASKS_IDS[self.login].pop()
    #     self.client.post(f'/api/analysis/{task_id}')
    #     logging.info('Request test file (name) analysis status')


class User(HttpUser):
    tasks = [LoggedInUserSteps]
    sock = None

    def on_start(self):
        self.client.verify = False  # allow self-signed certificates
