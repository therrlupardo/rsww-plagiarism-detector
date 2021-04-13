import fnmatch
import os
import shutil

SOURCE_DIRECTORY = '/home/django-django-775b796'
DESTINATION_DIRECTORY = '/home/data'

CSV_FILE = 'filenames.csv'


def __find_files_with_extension(file_extension='*.py'):
    matches = []

    path = os.path.join(SOURCE_DIRECTORY)
    for root, _, filenames in os.walk(path):
        matched_filenames = fnmatch.filter(filenames, file_extension)
        for filename in matched_filenames:
            filepath = os.path.join(root, filename)
            matches.append(filepath)

    return matches


def __copy_files_to_destination_directory(files):
    os.makedirs(DESTINATION_DIRECTORY, exist_ok=True)
    path = os.path.join(DESTINATION_DIRECTORY)

    for file in files:
        shutil.copy2(file, path)


def __write_filenames_to_csv():
    path = os.path.join(DESTINATION_DIRECTORY)
    names = []
    for _, _, filenames in os.walk(path):
        names.extend(filenames)

    path = os.path.join(DESTINATION_DIRECTORY, CSV_FILE)
    with open(path, 'w') as file:
        for name in names:
            file.write(f'{name}\n')


def main():
    python_files = __find_files_with_extension()

    python_files = list(set(python_files))
    python_files = python_files[:300]

    __copy_files_to_destination_directory(python_files)
    __write_filenames_to_csv()


if __name__ == '__main__':
    main()
