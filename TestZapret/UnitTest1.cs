using System.Runtime.InteropServices.ComTypes;
using TextProcess;
using Zapret;
using Zapret.Entity;

namespace TestZapret;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void RuleMatchWordInSentenceWithoutMatches()
    {
        var rawText = "Первое слово обычно правильное но война опасное слово его не надо писать";
        
        var sentence = new TextParser(rawText, ['.'], [' ']).Parse()[0];
    
        var rule = new Rule("Правило дескредитации сво", new Word("война"), 0.6, 0, 
            10000, 0.6, 0, new Word("СВО"));

        var nextWordFound = new Word("");
        var previousWordFound = new Word("");
        
        var rulesSentencesChecker = new RulesSentencesChecker();
        var result = rulesSentencesChecker.RuleConditionsMatch(2, sentence, rule, null, out nextWordFound, out previousWordFound);
        
        Assert.That(result, Is.False);
        Assert.That(nextWordFound, Is.Null);
        Assert.That(previousWordFound, Is.Null);
    }

    [Test]
    public void RuleMatchWordInSentenceWithTwoMatches()
    {
        var rawText = "Первое слово обычно россия правильное но война опасное слово сво его не надо писать";
        
        var sentence = new TextParser(rawText, ['.'], [' ']).Parse()[0];
    
        var rule = new Rule("Правило дескридитации СВО", new Word("война"), 0.6, 0, 
            10000, 0.6,
            0.7, new Word("СВО"), new Word("россия"));

        var nextWordFound = new Word("");
        var previousWordFound = new Word("");
        
        var rulesSentencesChecker = new RulesSentencesChecker();
        var result = rulesSentencesChecker.RuleConditionsMatch(4, sentence, rule, null, out nextWordFound, out previousWordFound);
        
        Assert.That(result);
        Assert.That(nextWordFound, Is.Not.Null);
        Assert.That(previousWordFound, Is.Not.Null);
    }
    
    [Test]
    public void RuleMatchWordInSentenceWithExclude()
    {
        var rawText = "Первое слово клавиатурка клавиатуры обычно правильное но война опасное слово сво его не надо писать";
        
        var sentence = new TextParser(rawText, ['.'], [' ']).Parse()[0];
    
        var rule = new Rule("Правило дескридитации СВО", new Word("война"), 0.6, 0, 
            10000, 0.6, 0.7, new Word("СВО"), new Word("клавиатура"));
    
        var nextWordFound = new Word("");
        var previousWordFound = new Word("");

        var excludes = new HashSet<Word>
        {
            sentence[3]
        };
        
        var rulesSentencesChecker = new RulesSentencesChecker();
        var result = rulesSentencesChecker.RuleConditionsMatch(4, sentence, rule,excludes ,
            out nextWordFound, out previousWordFound);
        
        Assert.That(result);
        Assert.That(nextWordFound, Is.Not.Null);
        Assert.That(nextWordFound.Value, Is.EqualTo("сво"));
        Assert.That(previousWordFound, Is.Not.Null);
        Assert.That(previousWordFound.Value, Is.EqualTo("клавиатурка"));
    }

    [Test]
    public void ViolatingRules()
    {
        var warRule = new Rule("Правило дескридитации СВО", new Word("война"), 0.4,
            0, 10000);
        var aueRule = new Rule("Правило экстремизма", new Word("ауе"), 1,
            2, 10000);

        var rules = new List<Rule>
        {
            warRule,
            aueRule
        };

        var rulesSentencesChecker = new RulesSentencesChecker();
        var violatingRules = rulesSentencesChecker.IsViolatesAnyRules(
            new Word("войнушка"), rules);
        
        Assert.That(violatingRules, Is.Not.Null);
        Assert.That(violatingRules.Count, Is.EqualTo(1));
        Assert.That(violatingRules[0].TriggerWord.Value, Is.EqualTo("война"));
    }

    [Test]
    public void ViolatingRuleInSentenceWhitRightWord()
    {
        var rawText = "привет привет война сво опасное слово";

        var parser = new TextParser(rawText, ['.'], [' ']);

        var text = parser.Parse();
        
        var rule = new Rule("Правило дескридитации СВО", new Word("война"), 0.6, 0,
            10000, 1, 1, new Word("сво"));
        
        var rulesChecker = new RulesSentencesChecker();
        var res = rulesChecker.GetViolateRulesWordsInSentence(text[0], [rule]);
        
        Assert.That(res.Count, Is.EqualTo(0));
    }

    [Test]
    public void ViolatingRuleInSentenceWithoutRightWords()
    {
        var rawText = "Привет война это плохое слово но сво это не войны";
        var parser = new TextParser(rawText, ['.'], [' ']);
        var text = parser.Parse();
        
        var rule = new Rule("Правило дескридитации СВО", new Word("война"), 0.6, 0,
            10000, 1, 1, new Word("сво"));
        
        var rulesChecker = new RulesSentencesChecker();
        var res = rulesChecker.GetViolateRulesWordsInSentence(text[0], [rule]);
        
        Assert.That(res.Count, Is.EqualTo(1));
        Assert.That(res[text[0][9]].Count, Is.EqualTo(1));
    }
    
    [Test]
    public void ViolatingRulesInSentenceWithoutRightWords()
    {
        var rawText = "Привет война это плохое слово но сво это не войны но главное не ауе";
        var parser = new TextParser(rawText, ['.'], [' ']);
        var text = parser.Parse();
        
        var rule = new Rule("Правило дескридитации СВО", new Word("война"), 0.6, 0,
            10000, 1, 1, new Word("сво"));
        var rule2 = new Rule("Правило экстремизма", new Word("ауе"), 0.6, 0,
            10000);
        
        var rulesChecker = new RulesSentencesChecker();
        var res = rulesChecker.GetViolateRulesWordsInSentence(text[0], [rule, rule2]);
        
        Assert.That(res.Count, Is.EqualTo(2));
        Assert.That(res[text[0][9]].Count, Is.EqualTo(1));
        Assert.That(res[text[0][13]].Count, Is.EqualTo(1));
    }
}