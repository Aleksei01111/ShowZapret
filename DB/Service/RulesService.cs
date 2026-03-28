using DB.Context;
using DB.Entities;
using Microsoft.EntityFrameworkCore;

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

    public void SaveOrUpdateRule(Rule rule)
    {
        rule.UserCreator = _context.Users.First(u => u.Id == rule.UserCreator.Id);
        
        var oldVersion = _context.Rules.ToList().Find(r => r.Id == rule.Id);
        if (oldVersion == null)
        {
            _context.Rules.Add(rule);
            _context.SaveChanges();
        }
        else
        {
            _context.Rules.Update(rule);
            _context.SaveChanges();
        }
    }
    
    public List<Rule> GetRules() => _context.Rules.Include(r => r.UserCreator).ToList();
    
    public void ClearAllRules()
    {
        if (!_context.Rules.Any())
            return;
        
        _context.Rules.RemoveRange(_context.Rules.ToList());
        _context.SaveChanges();
    }

    public void DeleteRule(Rule rule, bool throwExceptionIfNotExists = false)
    {
        if (throwExceptionIfNotExists && _context.Rules.ToList().All(r => r.NameOfRule != rule.NameOfRule))
            throw new Exception($"Значение {rule.NameOfRule} в бд не было найдено");
        
        _context.Rules.Remove(rule);
        _context.SaveChanges();
    }
}