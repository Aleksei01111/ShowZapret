using Microsoft.EntityFrameworkCore;

namespace DB.External.Service;

public class ReportService
{
    private DB.External.ContextExternal.SolveZapretDbContext _context = new();

    public void SendReport(External.EntitiesExternal.Report report)
    {
        _context.Reports.Add(report);
        _context.SaveChanges();
    }
}