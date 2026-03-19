using DB.Context;
using DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace DB.Service;

public class UsersService
{
    private ShowZapretDbContext _context = new();
    
    public void RegisterNewUser(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public User? GetUserByUserData(User userData) => 
        _context.Users.FirstOrDefault(u => u.Login == userData.Login && u.Password == userData.Password);
    
    public List<User> GetUsers() => _context.Users
        .Include(u => u.Notes)
        .Include(u => u.Rules)
        .ToList();

    public void ClearUsers()
    {
        foreach (var user in _context.Users.Include(u => u.Rules).Include(u => u.Notes))
        {
            DeleteUser(user, true);
        }
    }
    
    internal void DeleteUser(User user, bool cascade = false)
    {
        var userRules = user.Rules.ToList();
        var userNotes = user.Notes.ToList();
        
        if((userRules.Count > 0 && !cascade) || (userNotes.Count > 0 && !cascade))
            throw new ArgumentException("user include rules or notes by delete dont cascade!");
        
        if (userRules.Count > 0 && cascade)
        {
            foreach (var rule in userRules)
                _context.Rules.Remove(rule);
        }
    
        if (userNotes.Count > 0)
        {
            foreach (var note in userNotes)
                _context.Notes.Remove(note);
        }
        
        _context.Users.Remove(user);
        _context.SaveChanges();
    }
}