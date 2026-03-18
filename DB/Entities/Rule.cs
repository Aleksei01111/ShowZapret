using System;
using System.Collections.Generic;

namespace DB.Entities;

public partial class Rule
{
    public int Id { get; set; }

    public string NameOfRule { get; set; } = null!;

    public string TriggerWord { get; set; } = null!;

    public double TriggerWordMatchThreshold { get; set; }

    public int UserCreatorId { get; set; }

    public double FreedomPunishInMonth { get; set; }

    public double MoneyPunishmentInRubles { get; set; }

    public string? RightWordNext { get; set; }

    public double? ThresholdForRightWordNext { get; set; }

    public string? RightWordPrevious { get; set; }

    public double? ThresholdForRightWordPrevious { get; set; }

    public virtual User UserCreator { get; set; } = null!;
    
    public Rule() : this("", "", 0, 0, 0) {}
    
    public Rule(string name, string triggerWord, double triggerWordMatchThreshold, double freedomPunishmentInMonth,
        double moneyPunishmentInRubles, double thresholdForRightWordNext = 0, double thresholdForRightWordPrevious = 0,
        string? rightWordNext = null, string? rightWordPrevious = null)
    {
        NameOfRule = name;
        TriggerWord = triggerWord;
        TriggerWordMatchThreshold = triggerWordMatchThreshold;
        FreedomPunishInMonth = freedomPunishmentInMonth;
        MoneyPunishmentInRubles = moneyPunishmentInRubles;
        RightWordNext = rightWordNext;
        RightWordPrevious = rightWordPrevious;
        ThresholdForRightWordNext = thresholdForRightWordNext;
        ThresholdForRightWordPrevious = thresholdForRightWordPrevious;
    }
}
