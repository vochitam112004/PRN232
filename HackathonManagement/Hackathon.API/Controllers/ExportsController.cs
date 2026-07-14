using Hackathon.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.API.Controllers;

[ApiController]
[Route("api/exports")]
[Authorize(Roles = "organizer")]
public class ExportsController : ControllerBase
{
    private readonly IExportService _exportService;
    public ExportsController(IExportService exportService) => _exportService = exportService;

    [HttpGet("events/{eventId:guid}/ranking/csv")]
    public async Task<IActionResult> ExportCsv(Guid eventId)
    {
        var (content, fileName) = await _exportService.ExportEventRankingCsvAsync(eventId);
        return File(content, "text/csv", fileName);
    }

    [HttpGet("events/{eventId:guid}/ranking/excel")]
    public async Task<IActionResult> ExportExcel(Guid eventId)
    {
        var (content, fileName) = await _exportService.ExportEventRankingExcelAsync(eventId);
        return File(
            content,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            fileName);
    }
}