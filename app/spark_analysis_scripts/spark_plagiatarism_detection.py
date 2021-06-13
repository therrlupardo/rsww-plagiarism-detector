import atexit
import difflib
import pyspark
import os
import sys

from pyspark.sql import SparkSession
from pyspark.sql.functions import udf
from pyspark.sql.types import StructField, StructType, StringType
from collections import namedtuple


source_id = sys.argv[1]
analysis_repository_path = "hdfs://10.40.71.55:9000/group3/data.parquet"


def precise_analysis(file):
    return difflib.SequenceMatcher(None, source, file).ratio()


def quick_analysis(file):
    return difflib.SequenceMatcher(None, source, file).quick_ratio()


def get_diff(file):
    return str(difflib.SequenceMatcher(None, source, file).get_matching_blocks())


def read_source():
    # extracting file content
    source_df = spark.read.parquet("hdfs://10.40.71.55:9000/group3/sources.parquet")
    source_df = source_df.filter(source_df["FileId"] == source_id)

    return source_df.collect()[0]["FileContent"]


def get_spark_session():
    spark_context = pyspark.SparkContext.getOrCreate(
        pyspark.SparkConf() \
            .setMaster("spark://10.40.71.55:7077") \
            .setAppName("rsww3_analysis") \
            .set("spark.executor.memory", "4096m") \
            .set("spark.driver.port", os.environ.get("SPARK_DRIVER_PORT")) \
            .set("spark.ui.port", os.environ.get("SPARK_UI_PORT")) \
            .set("spark.blockManager.port", os.environ.get("SPARK_BLOCKMANAGER_PORT")) \
            .set("spark.driver.host", "10.40.71.55") \
            .set("spark.driver.bindAddress", "0.0.0.0")
    )
    spark = SparkSession.builder.config(conf=spark_context.getConf()).getOrCreate()
    
    return spark


def perform_analysis():
    df = spark.read.schema(schema).parquet(analysis_repository_path)
    df = df.withColumn("diff_quick", quick_analysis_udf(df["FileContent"]))
    df = df.filter(df["diff_quick"] > 0.1)
    df = df.cache()
    df = df.withColumn("diff_exact", precise_analysis_udf(df["FileContent"]))
    df = df.filter(df["diff_exact"] > 0.1)
    df = df.withColumn("reversed", 1.0 / df["diff_exact"])
    df = df.cache()
    count = df.count()

    result = df.groupBy().sum("reversed").collect()[0]

    return count / float(result["sum(reversed)"])


# defining udfs
precise_analysis_udf = udf(precise_analysis)
quick_analysis_udf = udf(quick_analysis)
get_diff_udf = udf(get_diff)

schema = StructType([
    StructField("UserId", StringType(), True),
    StructField("FileId", StringType(), True),
    StructField("Repository", StringType(), True),
    StructField("FileName", StringType(), True),
    StructField("FileContent", StringType(), True)
])

try:
    spark = get_spark_session()
except:
    print("-1")
    sys.exit()

try:
    source = read_source()
    result = perform_analysis()
    print(result)
except:
    print("-1")
finally:
    spark.stop()
    spark.sparkContext.stop()
