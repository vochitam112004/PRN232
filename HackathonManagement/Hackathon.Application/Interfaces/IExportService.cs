namespace Hackathon.Application.Interfaces;

public interface IExportService
{
    // Trả về nội dung file + tên file gợi ý
    Task<(byte[] Content, string FileName)> ExportEventRankingCsvAsync(Guid eventId);
    Task<(byte[] Content, string FileName)> ExportEventRankingExcelAsync(Guid eventId);
}