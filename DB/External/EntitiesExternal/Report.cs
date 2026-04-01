namespace DB.External.EntitiesExternal;

public partial class Report
{
    public int Id { get; set; }

    public int UserSenderId { get; set; }

    public string Text { get; set; } = null!;

    public DateTime Date { get; set; }

    public virtual ICollection<Kusplog> Kusplogs { get; set; } = new List<Kusplog>();

    public virtual User UserSender { get; set; } = null!;
}
