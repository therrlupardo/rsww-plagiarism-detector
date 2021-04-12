import csv


def read_data_from_csv_file(filepath):
    with open(filepath, 'r') as file:
        reader = csv.reader(file)
        data = list(reader)
    return data
