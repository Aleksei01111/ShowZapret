using TextProcess;

namespace Zapret;

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

    public Dictionary<Word, List<Rule>> GetViolateRulesWordsInSentence(
        Sentence sentence, List<Rule> rules)
    {
        var result = new Dictionary<Word, List<Rule>>();

        var excludeWords = new HashSet<Word>();
        
        for (var i = 0; i < sentence.Count; i++)
        {
            var violatingRules = IsViolatesAnyRules(sentence[i], rules);
            if (violatingRules != null)
            {
                excludeWords.Add(sentence[i]);
                
                var nextWord = new Word();
                var previousWord = new Word();
                
                foreach (var rule in violatingRules)
                {
                    if (!RuleConditionsMatch(i, sentence, rule, excludeWords, 
                            out nextWord, out previousWord))
                    {
                        if (result.TryGetValue(sentence[i], out var violatingRulesForCurrentWord))
                        {
                            // if (sentence[i] == null)
                            //     sentence[i] = new List<Rule>();
                            result[sentence[i]].Add(rule);
                        }
                        else
                        {
                            result.Add(sentence[i], [rule]);
                        }
                    }
                    
                    if(nextWord != null)
                        excludeWords.Add(nextWord);
                    if(previousWord != null)
                        excludeWords.Add(previousWord);
                }
            }
        }
        
        return result;
    }
    
    public List<Rule>? IsViolatesAnyRules(Word word, List<Rule> rules)
    {
        var res = new List<Rule>();
        foreach (var rule in rules)
        {
            if(IsViolatesForRule(word, rule))
                res.Add(rule);
        }
        if (res.Count == 0)
            return null;
        return res;
    }

    private bool IsViolatesForRule(Word word, Rule rule)
    {
        if (_wordsProcess.CompareByTrigram(word, rule.TriggerWord, rule.TriggerWordMatchThreshold))
            return true;
        return false;
    }
}