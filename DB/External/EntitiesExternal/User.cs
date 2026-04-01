namespace DB.External.EntitiesExternal;

public partial class User
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }

    public float? Honor { get; set; }

    public virtual ICollection<Kusplog> Kusplogs { get; set; } = new List<Kusplog>();

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

    public virtual UserRole Role { get; set; } = null!;
}
