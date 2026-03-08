using TextProcess;

namespace Zapret;

public class Rule
{
    public Word TriggerWord { get; set; }
    public double TriggerWordMatchThreshold { get; set; }
    public double FreedomPunishmentInMonth { get; set; }
    public double MoneyPunishmentInRubles { get; set; }
    
    //Условие чтобы слово не триггерилось
    public Word? RightWordNext { get; set; }
    public double ThresholdForRightWordNext { get; set; }
    //Условие чтобы слово не триггерилось
    public Word? RightWordPrevious { get; set; }
    public double ThresholdForRightWordPrevious { get; set; }

    public Rule(Word triggerWord, double triggerWordMatchThreshold, double freedomPunishmentInMonth,
        double moneyPunishmentInRubles, double thresholdForRightWordNext = 0, double thresholdForRightWordPrevious = 0,
        Word? rightWordNext = null, Word? rightWordPrevious = null)
    {
        TriggerWord = triggerWord;
        TriggerWordMatchThreshold = triggerWordMatchThreshold;
        FreedomPunishmentInMonth = freedomPunishmentInMonth;
        MoneyPunishmentInRubles = moneyPunishmentInRubles;
        RightWordNext = rightWordNext;
        RightWordPrevious = rightWordPrevious;
        ThresholdForRightWordNext = thresholdForRightWordNext;
        ThresholdForRightWordPrevious = thresholdForRightWordPrevious;
    }
}