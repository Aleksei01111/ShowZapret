namespace DB.External.EntitiesExternal;

public partial class Kusplog
{
    public int Id { get; set; }

    public int ReportId { get; set; }

    public DateTime DateOfAdd { get; set; }

    public int UserAddedId { get; set; }

    public int IsAccepted { get; set; }

    public virtual Report Report { get; set; } = null!;

    public virtual User UserAdded { get; set; } = null!;
}
