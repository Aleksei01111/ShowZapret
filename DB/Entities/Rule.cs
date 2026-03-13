using System;
using System.Collections.Generic;

namespace DB.Entities;

public partial class Rule
{
    public int Id { get; set; }

    public string NameOfRule { get; set; } = null!;

    public string TriggerWord { get; set; } = null!;

    public float TriggerWordMatchThreshold { get; set; }

    public int UserCreatorId { get; set; }

    public float FreedomPunishInMonth { get; set; }

    public float MoneyPunishmentInRubles { get; set; }

    public string? RightWordNext { get; set; }

    public float? ThresholdForRightWordNext { get; set; }

    public string? RightWordPrevious { get; set; }

    public float? ThresholdForRightWordPrevious { get; set; }

    public virtual User UserCreator { get; set; } = null!;
}
