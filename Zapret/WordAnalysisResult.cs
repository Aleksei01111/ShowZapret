using DB.Entities;
using TextProcess;

namespace Zapret;

/// <summary>
/// Вердикт по слову
/// </summary>
public class WordAnalysisResult(Word target, Sentence sentence, List<Rule> rules, HashSet<Word> wordsExclude)
{
    private Sentence _sentence = sentence;
    private List<Rule> _rules = rules;
    private HashSet<Word> _wordsExclude = wordsExclude;
    
    public enum VerdictType
    {
        Clean,
        Violation,
        Exempted,
        ViolationWithNegativeValue,
    }

    public Word Target { get; } = target;
    public VerdictType Verdict { get; private set; }
    public Rule? ViolationRule { get; private set; }
    public Word? NextRightWord { get; private set; }
    public Word? PreviousRightWord { get; private set; }

    public void Analyze()
    {
        var rulesChecker = new RulesSentencesChecker();
        var violatingRules = rulesChecker.IsViolatesAnyRules(Target, _rules);
        
        if (violatingRules == null)
        {
            Verdict = VerdictType.Clean;
            return;
        }

        ViolationRule = violatingRules[0];

        var checkResult = rulesChecker.RuleConditionsMatch(_sentence.IndexOf(Target), _sentence, ViolationRule,
            _wordsExclude, out var nextRightWord, out var previousRightWord);
        
        PreviousRightWord = previousRightWord;
        NextRightWord = nextRightWord;

        if (checkResult)
        {
            if(ViolationRule.FreedomPunishInMonth < 0 || ViolationRule.MoneyPunishmentInRubles < 0)
                Verdict = VerdictType.ViolationWithNegativeValue;
            else
                Verdict = VerdictType.Exempted;
        }
        else
        {
            if ((ViolationRule.RightWordNext == null && ViolationRule.RightWordPrevious == null) &&
                (ViolationRule.FreedomPunishInMonth < 0 || ViolationRule.MoneyPunishmentInRubles < 0))
                Verdict = VerdictType.ViolationWithNegativeValue;
            else if(!(ViolationRule.FreedomPunishInMonth < 0 || ViolationRule.MoneyPunishmentInRubles < 0))
                Verdict = VerdictType.Violation;
            else
                Verdict = VerdictType.Clean;
        }
    }
}

public class SentencesAnalyzer(List<Rule> rules)
{
    private List<Rule> _rules = rules;
    
    public Dictionary<Sentence, List<WordAnalysisResult>> AnalyzeText(List<Sentence> text)
    {
        var res = new Dictionary<Sentence, List<WordAnalysisResult>>();

        foreach (var sentence in text)
        {
            res.Add(sentence, AnalyzeSentence(sentence));
        }
        
        return res;
    }

    public List<WordAnalysisResult> AnalyzeSentence(Sentence sentence)
    {
        var res = new List<WordAnalysisResult>();
        var exclude = new HashSet<Word>();
        
        foreach (Word word in sentence)
        {
            var wordAnalysisResult = new WordAnalysisResult(word, sentence, _rules, exclude);
            wordAnalysisResult.Analyze();
            
            if (wordAnalysisResult.Verdict == WordAnalysisResult.VerdictType.Exempted)
            {
                if (wordAnalysisResult.NextRightWord != null)
                    exclude.Add(wordAnalysisResult.NextRightWord);
                if(wordAnalysisResult.PreviousRightWord != null)
                    exclude.Add(wordAnalysisResult.PreviousRightWord);
            }
            
            exclude.Add(word);
            res.Add(wordAnalysisResult);
        }

        return res;
    }
}