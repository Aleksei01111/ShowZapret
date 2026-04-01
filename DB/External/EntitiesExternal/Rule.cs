namespace DB.External.EntitiesExternal;

public partial class Rule
{
    public int Id { get; set; }

    public int ReportStatusMask { get; set; }

    public float HonorAdditiveValue { get; set; }

    public int UserRoleMaskId { get; set; }

    public virtual UserRole UserRoleMask { get; set; } = null!;
}
