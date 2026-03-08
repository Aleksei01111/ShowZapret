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

public class RulesSentencesChecker
{
    private WordsProcess _wordsProcess = new WordsProcess();
    private SentencesProcess _sentencesProcess = new SentencesProcess();
    
    //Выполняет ли условие правила
    public bool RuleConditionsMatch(int indexOfWordStartFromSentence, Sentence sentence, 
        Rule rule, HashSet<Word> wordsExclude, out Word? nextWord, out Word? previousWord)
    {
        nextWord = null;
        previousWord = null;
        
        var passedConditionsCount = 0;
        if (rule.RightWordNext != null)
        {
            nextWord = _sentencesProcess.FindWordInSentence(sentence, rule.RightWordNext.Value, indexOfWordStartFromSentence,
                rule.ThresholdForRightWordNext, true, _wordsProcess, wordsExclude);
            if(nextWord != null)
                passedConditionsCount++;
        }

        if (rule.RightWordPrevious != null)
        {
            previousWord = _sentencesProcess.FindWordInSentence(sentence, rule.RightWordPrevious.Value, indexOfWordStartFromSentence,
                rule.ThresholdForRightWordPrevious, false, _wordsProcess, wordsExclude);
            
            if(previousWord != null)
                passedConditionsCount++;
        }

        return passedConditionsCount != 0;
    }
}