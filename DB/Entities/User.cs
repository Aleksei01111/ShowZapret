using System;
using System.Collections.Generic;

namespace DB.Entities;

public partial class User
{
    public enum UserRole
    {
        Guest,
        Client,
        Mizulina,
        RKNEmployee
    }
    
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Address { get; set; } = null!;

    public UserRole Role { get; set; }

    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();

    public virtual ICollection<Rule> Rules { get; set; } = new List<Rule>();
}
