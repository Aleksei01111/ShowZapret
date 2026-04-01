using DB.Entities;
using DB.External.ContextExternal;
using Microsoft.EntityFrameworkCore;

namespace DB.External.Service;

public class UserServiceLayer
{
    private SolveZapretDbContext _context = new();

    public DB.External.EntitiesExternal.User GetUserMizulina(DB.Entities.User user)
    {
        if (user.Role != User.UserRole.Mizulina)
            throw new ArgumentException("Метод GetUserMizulina может работать только с ролью User.UserRole.Mizulina");

        var role = GetUserRole(user);
        var mizulinaUser = GetUser(user, role);

        return mizulinaUser;
    }

    private External.EntitiesExternal.User GetUser(DB.Entities.User user, External.EntitiesExternal.UserRole role)
    {
        External.EntitiesExternal.User res = null;
        
        var mizulinaUsers = _context.Users.Where(u => 
            u.Role.Name == role.Name && 
            u.Login == user.Login && 
            user.Password == u.Password)
            .ToList();
        if (mizulinaUsers.Count == 0)
        {
            res = new DB.External.EntitiesExternal.User()
            {
                Honor = 0,
                Login = user.Login,
                Password = user.Password,
                Role = role
            };
            _context.Users.Add(res);
            _context.SaveChanges();
        }
        else
        {
            res = mizulinaUsers.First();
        }

        return res;
    }
    
    private External.EntitiesExternal.UserRole GetUserRole(DB.Entities.User user)
    {
        EntitiesExternal.UserRole res = null!;
        
        var mizulinaRoles = _context.UserRoles
            .Include(ur => ur.Rules)
            .Include(ur => ur.Users)
            .Where(ur => ur.Name == user.Role.ToString()).ToList();
    
        if (mizulinaRoles.Count == 0)
        {
            res = new DB.External.EntitiesExternal.UserRole()
            {
                Name = user.Role.ToString(),
            };
            _context.UserRoles.Add(res);
            _context.SaveChanges();
        }
        else
        {
            res = mizulinaRoles.First();
        }
        return res;
    }
}