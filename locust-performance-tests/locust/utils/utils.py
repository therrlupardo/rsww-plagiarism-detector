import csv


def read_data_from_csv_file(filepath):
    with open(filepath, 'rb') as f:
        reader = csv.reader(f)
        data = list(reader)
    return data
