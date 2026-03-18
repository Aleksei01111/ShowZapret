using DB.Entities;
using DB.Service;

namespace TestDB;

public class Tests
{
    private UsersService _usersService = new UsersService();
    private RulesService _rulesService = new RulesService();
    private NotesService _notesService = new NotesService();
    
    private bool _needToStart = false;
    
    [SetUp]
    public void Setup()
    {
        if(!_needToStart)
            throw new Exception("need to start is false");
        
        _rulesService.ClearAllRules();
        _notesService.ClearAllNotes();
        _usersService.ClearUsers();
    }
    
    [Test, Order(1)]
    public void RegisterNewUserAndGet()
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
        
        Assert.That(users.Last().Login, Is.EqualTo(user.Login));
    }

    [Test, Order(2)]
    public void SaveAndGetNote()
    {
        var user = GetUserOrCreateAndAddToDB("testNote", "testNote");
        
        var note = new Note()
        {
            Text = "test",
            User = user,
        };
        
        _notesService.SaveNote(note);
        
        Assert.That(_notesService.GetNotes().Last().Text, Is.EqualTo(note.Text));
    }

    [Test, Order(3)]
    public void AddNewRuleAndGet()
    {
        var user = GetUserOrCreateAndAddToDB("testRule", "testRule");

        var rule = new Rule()
        {
            FreedomPunishInMonth = 100,
            MoneyPunishmentInRubles = 120,
            NameOfRule = "testNameOfRule",
            RightWordNext = "testRightWordNext",
            RightWordPrevious = null,
            ThresholdForRightWordNext = 0.4f,
            ThresholdForRightWordPrevious = null,
            TriggerWord = "testTriggerWord",
            TriggerWordMatchThreshold = 0.5f,
            UserCreator = user,
        };
        
        _rulesService.AddNewRule(rule);
        var rules = _rulesService.GetRules();
        
        Assert.That(rules.Last().FreedomPunishInMonth, Is.EqualTo(rule.FreedomPunishInMonth));
        Assert.That(rules.Last().MoneyPunishmentInRubles, Is.EqualTo(rule.MoneyPunishmentInRubles));
        Assert.That(rules.Last().NameOfRule, Is.EqualTo(rule.NameOfRule));
        Assert.That(rules.Last().RightWordNext, Is.EqualTo(rule.RightWordNext));
        Assert.That(rules.Last().RightWordPrevious, Is.EqualTo(rule.RightWordPrevious));
        Assert.That(rules.Last().ThresholdForRightWordNext, Is.EqualTo(rule.ThresholdForRightWordNext));
        Assert.That(rules.Last().ThresholdForRightWordPrevious, Is.EqualTo(rule.ThresholdForRightWordPrevious));
        Assert.That(rules.Last().TriggerWord, Is.EqualTo(rule.TriggerWord));
        Assert.That(rules.Last().TriggerWordMatchThreshold, Is.EqualTo(rule.TriggerWordMatchThreshold));
    }

    private User GetUserOrCreateAndAddToDB(string loginIfNeedToCreateNew, string passwordIfNeedToCreateNew)
    {
        var users = _usersService.GetUsers();

        if (users.Count != 0)
        {
            return users[0];
        }
        
        var user = new User()
        {
            Login = loginIfNeedToCreateNew,
            Password = passwordIfNeedToCreateNew,
            Address = "address",
            Role = "role",
        };
        
        _usersService.RegisterNewUser(user);
        
        return user;
    }
}