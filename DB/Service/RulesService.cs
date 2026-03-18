using DB.Context;
using DB.Entities;

namespace DB.Service;

public class RulesService
{
    private ShowZapretDbContext _context = new();

    public void AddNewRule(Rule rule)
    {
        rule.UserCreator = _context.Users.First(u => u.Id == rule.UserCreator.Id);
        _context.Rules.Add(rule);
        _context.SaveChanges();
    }
    
    public List<Rule> GetRules() => _context.Rules.ToList();
    
    public void ClearAllRules()
    {
        if (!_context.Rules.Any())
            return;
        
        _context.Rules.RemoveRange(_context.Rules.ToList());
        _context.SaveChanges();
    }
}