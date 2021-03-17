import json
from time import sleep

import scrapy


class ProjectsSpider(scrapy.Spider):
    name = "projects"
    MAX_PAGES = 10
    ARCHIVE_FORMAT = 'tarball'

    def start_requests(self):
        language = getattr(self, 'language', 'python')

        # https://docs.github.com/en/rest/reference/search#search-repositories
        urls = [f'https://api.github.com/search/repositories?q=language:{language}&sort=stars&per_page=100',
                f'https://api.github.com/search/repositories?q=language:{language}&sort=forks&per_page=100',
                f'https://api.github.com/search/repositories?q=language:{language}&sort=updated&per_page=100',
                f'https://api.github.com/search/repositories?q=language:{language}&sort=help-wanted-issues&per_page=100',
                f'https://api.github.com/search/repositories?q=language:{language}&sort=stars&order=asc&per_page=100',
                f'https://api.github.com/search/repositories?q=language:{language}&sort=forks&order=asc&per_page=100',
                f'https://api.github.com/search/repositories?q=language:{language}&sort=updated&order=asc&per_page=100',
                f'https://api.github.com/search/repositories?q=language:{language}&sort=help-wanted-issues&order=asc&per_page=100',
                f'https://api.github.com/search/repositories?q=language:{language}&per_page=100']

        for url in urls:
            page_number = 1
            while page_number < self.MAX_PAGES:
                request_url = f'{url}&page={page_number}'
                yield scrapy.Request(request_url, self.parse)
                page_number += 1
            print('Waiting for rate limit to refresh - 10 queries per minute for GitHub Search API')
            # https://docs.github.com/en/rest/reference/search#rate-limit
            sleep(60)

    def parse(self, response, **kwargs):
        # https://docs.github.com/en/rest/reference/repos#download-a-repository-archive-tar
        projects = json.loads(response.text)['items']

        for project in projects:
            # "archive_url": "https://api.github.com/repos/{owner}/{repository}/{archive_format}",
            archive_url = project['archive_url']
            archive_url = self._append_archive_format(archive_url)

            yield {'repository-name': project['full_name'], 'scrape-url': archive_url, 'archive-scraped': False}

    def _append_archive_format(self, url):
        url = url.split('/')[:-2]
        url = '/'.join(url)
        url = f'{url}/{self.ARCHIVE_FORMAT}'
        return url
