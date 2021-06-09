import findspark
import sys

from pyspark.sql import SparkSession


findspark.init()

user_id = sys.argv[1]
file_id = sys.argv[2]
repository_name = sys.argv[3]
file_name = sys.argv[4]
file_content = sys.argv[5]

spark = SparkSession.builder.master("spark://localhost:7077").appName("rsww3_save_source_file").getOrCreate()

analysis_repository_path = '/group3/data.parquet'

analysis_repository_df = spark.read.parquet(analysis_repository_path)
columns = ["UserId", "FileId", "Repository", "FileName", "FileContent"]
row = (user_id, file_id, repository_name, file_name, file_content)
source_file_df = spark.createDataFrame([row], columns)
file_repository_df = analysis_repository_df.union(source_file_df)
file_repository_df.write.parquet(analysis_repository_path)
