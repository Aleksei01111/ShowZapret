namespace DB.External.EntitiesExternal;

public partial class UserRole
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Rule> Rules { get; set; } = new List<Rule>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
