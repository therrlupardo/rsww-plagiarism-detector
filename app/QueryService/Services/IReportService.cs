namespace QueryService.Services
{
    public interface IReportService<in T>
    {
        byte[] GenerateReport(T obj, params object[] parameters);
    }
}