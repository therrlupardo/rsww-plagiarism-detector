import os
import sys
import pyspark

from pyspark.sql import SparkSession

user_id = sys.argv[1]
file_id = sys.argv[2]
repository_name = sys.argv[3]
file_name = sys.argv[4]
file_content = sys.argv[5]

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

source_repository_path = "hdfs://10.40.71.55:9000/group3/sources.parquet"
tmp_path = "hdfs://10.40.71.55:9000/group3/tmp.parquet"

source_repository_df = spark.read.parquet(source_repository_path)
source_repository_df.write.format("parquet").mode("overwrite").save(tmp_path)
source_repository_df = spark.read.parquet(tmp_path)

columns = ["UserId", "FileId", "Repository", "FileName", "FileContent"]
row = (user_id, file_id, repository_name, file_name, file_content)
source_file_df = spark.createDataFrame([row], columns)
file_repository_df = source_repository_df.union(source_file_df)
file_repository_df.write.format("parquet").mode("overwrite").save(source_repository_path)

spark.stop()
spark_context.stop()
