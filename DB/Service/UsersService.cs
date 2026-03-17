using DB.Context;
using DB.Entities;

namespace DB.Service;

internal static class ShowZapretDbContextInstance
{
    public static ShowZapretDbContext Instance { get; } = new ShowZapretDbContext();
}

public class UsersService
{
    private ShowZapretDbContext _context = ShowZapretDbContextInstance.Instance;
    
    public void RegisterNewUser(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public User? GetUserByUserData(User userData) => 
        _context.Users.FirstOrDefault(u => u.Login == userData.Login && u.Password == userData.Password);
    
    public List<User> GetUsers() => _context.Users.ToList();

    public void ClearUsers()
    {
        _context.Users.RemoveRange(_context.Users.ToList());
        _context.SaveChanges();
    }
}