from locust import HttpLocust, TaskSet, task
import logging

from locust.settings import USERS_CREDENTIALS, TEST_FILES, MODEL_FILES
from locust.utils.utils import read_data_from_csv_file

USERS_CREDENTIALS_LIST = read_data_from_csv_file(USERS_CREDENTIALS)
TEST_FILES_LIST = read_data_from_csv_file(TEST_FILES)
MODEL_FILES_LIST = read_data_from_csv_file(MODEL_FILES)


class LoggedInUserSteps(TaskSet):
    email = "NOT_FOUND"
    password = "NOT_FOUND"

    def on_start(self):
        if len(USERS_CREDENTIALS_LIST) > 0:
            self.email, self.password = USERS_CREDENTIALS_LIST.pop()
            self._login()

    def _login(self):
        self.client.post('/login', {
            'email': self.email,
            'password': self.password,
        })
        logging.info('Login with %s email and %s password', self.email, self.password)

    @task(50)
    def upload_test_file(self):
        self.client.post('/upload-test')
        logging.info('Upload test file (name)')

    @task(1000)
    def request_test_files_list(self):
        self.client.post('/list-test')
        logging.info('Request test files list')

    @task(100)
    def upload_model_file(self):
        self.client.post('/upload-model')
        logging.info('Upload model file (name)')

    @task(1000)
    def request_model_files_list(self):
        self.client.post('/list-model')
        logging.info('Request model files list')

    @task(2000)
    def request_test_file_analysis(self):
        self.client.post('/analyze')
        logging.info('Request test file (name) analysis')

    @task(2000)
    def request_test_file_analysis_status(self):
        self.client.post('/analyze-status')
        logging.info('Request test file (name) analysis status')


class User(HttpLocust):
    task_set = LoggedInUserSteps
    sock = None

    def __init__(self):
        super(User, self).__init__()
