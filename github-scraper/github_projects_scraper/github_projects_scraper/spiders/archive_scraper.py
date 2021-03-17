import scrapy
import os
import pandas
from shutil import copy2

from github_projects_scraper.settings import FILES_STORE


class ArchivesSpider(scrapy.Spider):
    name = "archives"
    MAX_ARCHIVES = 55  # https://docs.github.com/en/rest/overview/resources-in-the-rest-api#rate-limiting

    def start_requests(self):
        archive_urls_filepath = getattr(self, 'archives-csv', None)
        resume_scraping = getattr(self, 'resume', False)

        if not resume_scraping:
           self._copy_archives_csv(archive_urls_filepath)
        archive_urls_filepath = self._get_archives_csv_copy_filepath(archive_urls_filepath)

        archives_df = pandas.read_csv(archive_urls_filepath)
        archives_df = self._remove_scraped_urls_from_dataframe(archives_df)

        archive_count = 0
        for index, archive_row in archives_df.iterrows():
            yield scrapy.Request(archive_row['scrape-url'], self.parse)

            archives_df.at[index, 'archive-scraped'] = True
            archives_df.to_csv(archive_urls_filepath, index=False)

            archive_count += 1
            if archive_count is self.MAX_ARCHIVES:
                break

    def parse(self, response, **kwargs):
        if response.status == 200:
            url = response.request.url
            archive_name = self._extract_archive_filename_from_url(url)
            archive_path = self._get_archive_path(archive_name)
            with open(archive_path, 'wb') as archive:
                archive.write(response.body)

    @staticmethod
    def _copy_archives_csv(filepath):
        copy2(filepath, FILES_STORE)

    @staticmethod
    def _get_archives_csv_copy_filepath(filepath):
        filename = os.path.basename(filepath)
        return os.path.join(FILES_STORE, filename)

    @staticmethod
    def _remove_scraped_urls_from_dataframe(dataframe):
        return dataframe.drop(dataframe[dataframe['archive-scraped'] == True].index)

    @staticmethod
    def _extract_archive_filename_from_url(url):
        # url: https://codeload.github.com/{owner}/{repository}/legacy.tar.gz/master
        archive_filename = url.split('/')[3:5]
        archive_filename = '-'.join(archive_filename) + '.tar.gz'
        return archive_filename

    @staticmethod
    def _get_archive_path(archive_name):
        return os.path.join(FILES_STORE, archive_name)
