import findspark
import sys

from pyspark.sql import SparkSession


findspark.init()

user_id = sys.argv[1]
file_id = sys.argv[2]
repository_name = sys.argv[3]
file_name = sys.argv[4]
file_content = sys.argv[5]

spark = SparkSession.builder.master("spark://localhost:7077").appName("rsww3_save_analysis_file").getOrCreate()

source_repository_path = '/group3/sources.parquet'
tmp_path = "/group3/tmp.parquet"

source_repository_df = spark.read.parquet(source_repository_path)
source_repository_df.write.format("parquet").mode("overwrite").save(tmp_path)
source_repository_df = spark.read.parquet(tmp_path)

columns = ["UserId", "FileId", "Repository", "FileName", "FileContent"]
row = (user_id, file_id, repository_name, file_name, file_content)
source_file_df = spark.createDataFrame([row], columns)
file_repository_df = source_repository_df.union(source_file_df)
file_repository_df.write.format("parquet").mode("overwrite").save(source_repository_path)
