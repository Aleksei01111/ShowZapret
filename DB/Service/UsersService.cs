using DB.Context;
using DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace DB.Service;

public class UsersService
{
    private ShowZapretDbContext _context = new();
    
    public void RegisterNewUser(User user)
    {
        if(_context.Users.Any(u => u.Login == user.Login))
            throw new ArgumentException("Пользователь с таким логином уже ест!");
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public void DeleteUser(User user)
    {
        var foundUser = _context.Users.Where(u => u.Login == user.Login)?.FirstOrDefault();
        if (foundUser == null)
            throw new ArgumentException("Пользователь не найден");
        _context.Users.Remove(foundUser);
        _context.SaveChanges();
    }
    
    public void SaveUser(User user, User newUserData)
    {
        if (_context.Users.Any(u => u.Login == user.Login && u.Id != user.Id))
            throw new ArgumentException("Пользователь с таким логином уже есть");
        
        var foundUser = _context.Users.Where(u => u.Login == user.Login && u.Id == user.Id)?.FirstOrDefault();

        if (foundUser == null)
            throw new ArgumentException($"Пользователь {user.Login} не найден");
        
        foundUser.Login = newUserData.Login;
        foundUser.Password = newUserData.Password;
        foundUser.Address =  newUserData.Address;
        foundUser.Role = newUserData.Role;
        
        _context.Users.Update(foundUser);
        _context.SaveChanges();
    }

    public User? GetUserByLoginAndPassword(string login, string password, bool isGuest = false)
    {
        if (isGuest)
        {
            var found = _context.Users.FirstOrDefault(u => u.Role == User.UserRole.Guest);
            if (found == null)
            {
                var newUserGuest = new User
                {
                    Login = "Guest",
                    Password = "",
                    Address = "",
                    Role = User.UserRole.Guest
                };

                RegisterNewUser(newUserGuest);
                
                return newUserGuest;
            }

            return found;
        }
            
        return _context.Users.FirstOrDefault(u => u.Login == login && u.Password == password);
    }
    
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