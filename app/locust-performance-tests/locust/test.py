import io
import os
import shutil
import sys
import tarfile
import tokenize

import pandas as pd  # pip install pandas
import pyarrow as pa  # pip install pyarrow
import pyarrow.parquet as pq

if len(sys.argv) != 2:
    exit()

df = pd.DataFrame(columns=['UserId', 'FileId', 'Repository', 'FileName', 'FileContent'])
directory = sys.argv[1] + "/"


def remove_comments_and_docstrings(source):
    io_obj = io.StringIO(source)
    out = ""
    prev_toktype = tokenize.INDENT
    last_lineno = -1
    last_col = 0
    for tok in tokenize.generate_tokens(io_obj.readline):
        token_type = tok[0]
        token_string = tok[1]
        start_line, start_col = tok[2]
        end_line, end_col = tok[3]
        ltext = tok[4]
        if start_line > last_lineno:
            last_col = 0
        if start_col > last_col:
            out += (" " * (start_col - last_col))
        if token_type == tokenize.COMMENT:
            pass
        elif token_type == tokenize.STRING:
            if prev_toktype != tokenize.INDENT:
                if prev_toktype != tokenize.NEWLINE:
                    if start_col > 0:
                        out += token_string
        else:
            out += token_string
        prev_toktype = token_type
        last_col = end_col
        last_lineno = end_line
    out = '\n'.join(l for l in out.splitlines() if l.strip())
    return out


def process_dir(repo, dir):
    global df
    for file in os.listdir(dir):
        if os.path.isdir(dir + "/" + file):
            process_dir(repo, dir + "/" + file)
        if len(file.split(".")) < 2 or file.split(".")[-1] != "py":
            continue
        with open(dir + "/" + file, 'r') as fileF:
            data = fileF.read()
            file_content = remove_comments_and_docstrings(data)

            df = df.append({'UserId': "", 'FileId': "",
                            'Repository': str(repo), 'FileName': str(file),
                            'FileContent': str(file_content)},
                           ignore_index=True)


for file_name in os.listdir(directory):
    file = tarfile.open(directory + file_name)
    # extracting file
    file.extractall(directory + "tmp")
    process_dir(file_name, directory + "tmp")
    shutil.rmtree(directory + "tmp")

table = pa.Table.from_pandas(df)
pq.write_table(table, 'dataset.parquet')
