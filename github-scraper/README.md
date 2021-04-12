## GitHub projects scraper
### Preparing evnironment and running scraper

1. Prepare Python virtual environemnt with required packages:\
    `./scripts/00setup.sh`

2. Activate venv:\
    `source ./scripts/01activate.sh`

3. Start scraping GitHub repositiories data:\
    `cd github_projects_scraper; scrapy crawl projects`

    By default .csv files with scraped data are written to `/mnt/usb/project-names` directory. You can change the path in `github_projects_scraper/settings.py` file in `FEED` dictionary entry.

3. Start scraping repositories archives:\
    `cd github_projects_scraper; scrapy crawl archives -a archives-csv=/path/to/repositories/info/csv/file`

    Resume scraping archives:\
    `cd github_projects_scraper; scrapy crawl archives -a archives-csv=path/to/repositories/info/csv -a resume=True`

    By default archives are written to `/mnt/usb` directory. You can change the path in `github_projects_scraper/settings.py` file in `FILES_STORE` variable.

4. Deactivate venv:\
    `source ./scripts/02deactivate.sh`

5. Remove Python virtual environment completely:\
    `./scripts/03destroy.sh`


### Collecting data from multiple runs

To collect GitHub repositories data scraped during multiple runs into one file, copy `./scripts/cleanup-repository-data.sh` to the directory with result .csv files and run:

`./cleanup-repository-data.sh`

The script removes duplicate entries from .csv files.

### Processing collected data

To filter python files from scrapped repositories, process them and store in sequence file run:

`py to_parquet.py path_to_archives`

where `path_to_archives` is path to a folder containing repositiories in *.tar.gz archives. The script will output
sequence file in parquet format (conversion snappy) called 'dataset.parquet'.