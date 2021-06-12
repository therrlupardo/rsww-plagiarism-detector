import difflib
import pyspark
import os
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


spark_context = pyspark.SparkContext.getOrCreate(
    pyspark.SparkConf() \
        .setMaster("spark://10.40.71.55:7077") \
        .setAppName("rsww3_save_source_file") \
        .set("spark.driver.port", os.environ.get("SPARK_DRIVER_PORT")) \
        .set("spark.ui.port", os.environ.get("SPARK_UI_PORT")) \
        .set("spark.blockManager.port", os.environ.get("SPARK_BLOCKMANAGER_PORT")) \
        .set("spark.driver.host", "10.40.71.55") \
        .set("spark.driver.bindAddress", "0.0.0.0")
)
spark = SparkSession.builder.config(conf=spark_context.getConf()).getOrCreate()

source_id = sys.argv[1]
analysis_repository_path = "hdfs://10.40.71.55:9000/group3/data.parquet"

# defining udfs
precise_analysis_udf = udf(precise_analysis)
quick_analysis_udf = udf(quick_analysis)
get_diff_udf = udf(get_diff)

# extracting file content
source_df = spark.read.parquet("hdfs://10.40.71.55:9000/group3/sources.parquet")
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
    file_repo = f"{row['Repository']}/{row['FileName']}"
    for match in matching_blocks:
        if match.size > 10:
            print(
                f"Sequence in {file_repo} from byte {match.a} = sequence from {match.b} in source file, length = {match.size}")
            if verbose:
                print(f"\x1b[31m\"{source[match.a:match.a + match.size]}\"\x1b[0m")

spark.stop()
spark_context.stop()
