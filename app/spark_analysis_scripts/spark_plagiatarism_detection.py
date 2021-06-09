import findspark
import difflib
import pyspark
import sys

from pyspark.sql import SparkSession
from pyspark.sql.functions import udf
from collections import namedtuple


def precise_analysis(file):
    return difflib.SequenceMatcher(None, source, file).ratio()


def quick_analysis(file):
    return difflib.SequenceMatcher(None, source, file).quick_ratio()


def get_diff(file):
    return str(difflib.SequenceMatcher(None, source, file).get_matching_blocks())


findspark.init()

spark = SparkSession.builder.master("spark://localhost:7077").appName("rsww3_analysis").getOrCreate()

source_id = sys.argv[1]
analysis_repository_path = '/group3/data.parquet'

# defining udfs
precise_analysis_udf = udf(precise_analysis)
quick_analysis_udf = udf(quick_analysis)
get_diff_udf = udf(get_diff)

# extracting file content
source_df = spark.read.parquet('/group3/sources.parquet')
source = source_df.filter(source_df["FileId"] == source_id).collect()[0]["FileContent"]

# performing analysis
df = spark.read.parquet(analysis_repository_path)
df = df.withColumn("diff_quick", quick_analysis_udf(df["FileContent"]))
df = df.filter(df["diff_quick"] > 0.1)
df = df.cache()
df = df.withColumn("diff_exact", precise_analysis_udf(df["FileContent"]))
df = df.filter(df["diff_exact"] > 0.1)
df = df.withColumn("matching_blocks", get_diff_udf(df["FileContent"]))
df = df.cache()
Match = namedtuple("Match", "a b size")

print("Detected plagiarism:")

verbose = True

for row in df.collect():
    matching_blocks = eval(row["matching_blocks"])
    file_repo = "{}/{}".format(row["Repository"], row["FileName"])
    for match in matching_blocks:
        if match.size > 10:
            print("Sequence in {} from byte {} = sequence from {} in source file, length = {}".format(file_repo, match.a, match.b, match.size))
            if verbose:
                print("\x1b[31m\"{}\"\x1b[0m".format(source[match.a:match.a + match.size]))

spark.stop()
