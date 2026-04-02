using Microsoft.EntityFrameworkCore;

namespace DB.External.Service;

public class ReportService
{
    private DB.External.ContextExternal.SolveZapretDbContext _context = new();

    public void SendReport(External.EntitiesExternal.Report report)
    {
        var foundUser = _context.Users
            .Include(u => u.Role)
            .Include(u => u.Kusplogs)
            .Include(u => u.Reports)
            .Where(u => u.Login == report.UserSender.Login && u.Password == report.UserSender.Password)
            .ToList().First();

        report.UserSender = foundUser;
        
        _context.Reports.Add(report);
        _context.SaveChanges();
    }
}