using System.Globalization;
using System.Text;
using ClosedXML.Excel;
using CsvHelper;
using Hackathon.Application.DTOs.Export;
using Hackathon.Application.Interfaces;

namespace Hackathon.Application.Services;

public class ExportService : IExportService
{
    private readonly IEventRepository _eventRepository;
    private readonly IRankingService _rankingService;

    public ExportService(IEventRepository eventRepository, IRankingService rankingService)
    {
        _eventRepository = eventRepository;
        _rankingService = rankingService;
    }

    // Gom xếp hạng của MỌI vòng trong sự kiện thành danh sách dòng phẳng
    private async Task<(string EventTitle, List<RankingExportRow> Rows)> BuildRowsAsync(Guid eventId)
    {
        var ev = await _eventRepository.GetByIdWithDetailsAsync(eventId)
            ?? throw new KeyNotFoundException($"Không tìm thấy sự kiện {eventId}.");

        var rows = new List<RankingExportRow>();

        // ev.Rounds đã được Include sẵn; sắp theo thứ tự vòng nếu có RoundOrder
        var rounds = ev.Rounds.OrderBy(r => r.RoundOrder).ToList();

        foreach (var round in rounds)
        {
            var ranking = await _rankingService.GetRoundResultsAsync(round.Id);
            foreach (var r in ranking.Results.OrderBy(x => x.Rank))
            {
                rows.Add(new RankingExportRow
                {
                    RoundName = ranking.RoundName,
                    Rank = r.Rank,
                    TeamName = r.TeamName,
                    CategoryName = r.CategoryName,
                    TotalScore = r.TotalScore,
                    IsAdvanced = r.IsAdvanced,
                    Note = r.Note
                });
            }
        }

        return (ev.Title, rows);
    }

    public async Task<(byte[] Content, string FileName)> ExportEventRankingCsvAsync(Guid eventId)
    {
        var (title, rows) = await BuildRowsAsync(eventId);

        using var ms = new MemoryStream();
        // BOM UTF-8 để Excel mở CSV không lỗi tiếng Việt
        using (var writer = new StreamWriter(ms, new UTF8Encoding(true)))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(rows);
        } // dispose writer -> flush hết vào ms

        var fileName = $"BangXepHang_{Sanitize(title)}.csv";
        return (ms.ToArray(), fileName);
    }

    public async Task<(byte[] Content, string FileName)> ExportEventRankingExcelAsync(Guid eventId)
    {
        var (title, rows) = await BuildRowsAsync(eventId);

        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Bang xep hang");

        // Header
        ws.Cell(1, 1).Value = "Vòng";
        ws.Cell(1, 2).Value = "Hạng";
        ws.Cell(1, 3).Value = "Đội";
        ws.Cell(1, 4).Value = "Hạng mục";
        ws.Cell(1, 5).Value = "Tổng điểm";
        ws.Cell(1, 6).Value = "Vào vòng sau";
        ws.Cell(1, 7).Value = "Ghi chú";
        ws.Row(1).Style.Font.Bold = true;

        // Data
        int row = 2;
        foreach (var r in rows)
        {
            ws.Cell(row, 1).Value = r.RoundName;
            ws.Cell(row, 2).Value = r.Rank;
            ws.Cell(row, 3).Value = r.TeamName;
            ws.Cell(row, 4).Value = r.CategoryName;
            ws.Cell(row, 5).Value = r.TotalScore;
            ws.Cell(row, 6).Value = r.IsAdvanced ? "Có" : "Không";
            ws.Cell(row, 7).Value = r.Note ?? "";
            row++;
        }

        ws.Columns().AdjustToContents();

        using var ms = new MemoryStream();
        wb.SaveAs(ms);

        var fileName = $"BangXepHang_{Sanitize(title)}.xlsx";
        return (ms.ToArray(), fileName);
    }

    // Bỏ ký tự không hợp lệ trong tên file
    private static string Sanitize(string name)
    {
        foreach (var c in Path.GetInvalidFileNameChars())
            name = name.Replace(c, '_');
        return name.Replace(' ', '_');
    }
}