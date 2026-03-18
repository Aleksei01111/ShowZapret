using System;
using System.Collections.Generic;

namespace DB.Entities;

public partial class Note
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Text { get; set; } = null!;
    public DateTime Date { get; set; }

    public virtual User User { get; set; } = null!;
}
