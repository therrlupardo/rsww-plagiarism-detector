using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.IO;
using System;

namespace AnalysisService
{
    public class AnalysisService : AnalysisStart.AnalysisStartBase
    {
        private readonly ILogger<AnalysisService> _logger;
        public AnalysisService(ILogger<AnalysisService> logger)
        {
            _logger = logger;
        }

        public override Task<AnalysisResult> StartAnalysis(AnalysisRequest request, ServerCallContext context)
        {
            // TODO: implement spark logic below

            return Task.FromResult(new AnalysisResult
            {
                PlagiarismPercentage = 0f
            });
        }

        public override Task<PersistanceResult> PersistFile(PersistanceRequest request, ServerCallContext context)
        {
            // TODO: move file persistance from local storage to HDFS
            string directory = "files";
            Directory.CreateDirectory(directory);
            byte[] fileContent = request.FileContent.ToByteArray();
            bool succeeded = true;
            try
            {
                File.WriteAllBytes(directory + "/" + request.FileId, fileContent);
            }
            catch (Exception)
            {
                succeeded = false;
            }

            return Task.FromResult(new PersistanceResult
            {
                Succeeded = succeeded
            });

        }
    }
}
