using DB.Entities;
using DB.Service;

namespace TestDB;

public class Tests
{
    private UsersService _usersService = new UsersService();
    private bool _needToStart = false;
    
    [SetUp]
    public void Setup()
    {
        if(!_needToStart)
            throw new Exception("need to start is false");
        
        _usersService.ClearUsers();
    }
    
    [Test]
    public void RegisterNewUser()
    {
        var user = new User()
        {
            Login = "test",
            Password = "test",
            Address = "test",
            Role = "test",
        };
        
        _usersService.RegisterNewUser(user);
        
        var users = _usersService.GetUsers();
        
        Assert.That(users.Count, Is.EqualTo(1));
        Assert.That(users[0].Login, Is.EqualTo(user.Login));
    }
}