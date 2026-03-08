using TextProcess;
using Zapret;

namespace TestParser;

public class Tests
{
    private static readonly char[] DefaultSentenceSeparators = { '.', '!', '?' };
    private static readonly char[] DefaultWordSeparators = { ' ', '\t', '\n', '\r' };
    
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Parse_EmptyString_ReturnsEmptyList()
    {
        var parser = new TextParser("", DefaultSentenceSeparators, DefaultWordSeparators);
        
        var result = parser.Parse();

        Assert.That(result.Count, Is.EqualTo(0));
    }
    
    [Test]
    public void Parse_OnlyWhitespace_ReturnsEmptyList()
    {
        var parser = new TextParser("   \t\n\r   ", DefaultSentenceSeparators, DefaultWordSeparators);
        
        var result = parser.Parse();
        
        Assert.That(result.Count, Is.EqualTo(0));
    }

    [Test]
    public void ParseTextWithoutSentenceSeparators()
    {
        var parser = new TextParser("Привет, как у тебя дела?", [], [' ']);

        var textParsed = parser.Parse();
        
        Assert.That(textParsed.Count, Is.EqualTo(1));
    }
    
    [Test]
    public void Parse_OneSimpleSentence_CorrectlySplitWords()
    {
        var text = "Привет как дела";
        var parser = new TextParser(text, DefaultSentenceSeparators, DefaultWordSeparators);

        var sentences = parser.Parse();

        Assert.That(sentences, Has.Count.EqualTo(1));
        
        Assert.That(sentences[0], Has.Count.EqualTo(3));
        
        Assert.That(sentences[0][0].Value, Is.EqualTo("Привет"));
        Assert.That(sentences[0][1].Value, Is.EqualTo("как"));
        Assert.That(sentences[0][2].Value, Is.EqualTo("дела"));
    }
    
    [Test]
    public void Parse_MultipleSentences_CorrectCountAndContent()
    {
        var text = "Первый. Второй! Третий? Четвёртый...";
        var parser = new TextParser(text, DefaultSentenceSeparators, DefaultWordSeparators);

        var sentences = parser.Parse();

        Assert.That(sentences, Has.Count.EqualTo(4));

        Assert.That(sentences[0][0].Value, Is.EqualTo("Первый"));
        Assert.That(sentences[1][0].Value, Is.EqualTo("Второй"));
        Assert.That(sentences[2][0].Value, Is.EqualTo("Третий"));
        Assert.That(sentences[3][0].Value, Is.EqualTo("Четвёртый"));
    }
    
    [Test]
    public void Parse_SentenceWithPunctuationInside_StaysInWordOrHandledCorrectly()
    {
        var text = "Привет, как дела? Всё \"хорошо\", сказал он...";
        var parser = new TextParser(text, ['.', '!', '?'], [' ', '\t']);

        var sentences = parser.Parse();

        Assert.That(sentences, Has.Count.EqualTo(2));

        var first = sentences[0];
        Assert.That(first.Select(w => w.Value), Is.EqualTo(new[]
        {
            "Привет", "как", "дела"
        }));

        var second = sentences[1];
        Assert.That(second.Select(w => w.Value), Is.EqualTo(new[]
        {
            "Всё", "хорошо", "сказал", "он"
        }));
    }
    
    [Test]
    public void Parse_TextWithMultipleSeparatorsInRow_IgnoresExtraSeparators()
    {
        var text = "Один..Два!!!   Три?   ";
        var parser = new TextParser(text, DefaultSentenceSeparators, DefaultWordSeparators);

        var sentences = parser.Parse();

        Assert.That(sentences, Has.Count.EqualTo(3));
        Assert.That(sentences[0][0].Value, Is.EqualTo("Один"));
        Assert.That(sentences[1][0].Value, Is.EqualTo("Два"));
        Assert.That(sentences[2][0].Value, Is.EqualTo("Три"));
    }
    
    [Test]
    public void Parse_UsesCustomWordSeparators_Correctly()
    {
        var text = "слово1,слово2;слово3 слово4";
        char[] customWordSep = [',', ';', ' '];
        
        var parser = new TextParser(text, DefaultSentenceSeparators, customWordSep);

        var sentences = parser.Parse();

        Assert.That(sentences, Has.Count.EqualTo(1));
        
        var words = sentences[0].Select(w => w.Value).ToList();
        Assert.That(words, Is.EqualTo(new[] { "слово1", "слово2", "слово3", "слово4" }));
    }
    
    [Test]
    public void Parse_NoSentenceSeparators_AllInOneSentence()
    {
        var text = "Это просто текст без точек и знаков";
        var parser = new TextParser(text, [], DefaultWordSeparators);

        var sentences = parser.Parse();

        Assert.That(sentences, Has.Count.EqualTo(1));
        Assert.That(sentences[0], Has.Count.EqualTo(7));
    }

    [Test]
    public void TrigramTest()
    {
        var wordProcess = new WordsProcess();
        
        var actual1 = wordProcess.CompareByTrigram(new Word("война"), new Word("война"), 0.8);
        var actual2 = wordProcess.CompareByTrigram(new Word("война"), new Word("войнушка"), 0.4);
        var actual3 = wordProcess.CompareByTrigram(new Word("война"), new Word("войны"), 0.7);
        var actual4 = wordProcess.CompareByTrigram(new Word("война"), new Word("вой"), 0.6);
        
        Assert.Multiple(() =>
        {
            Assert.That(actual1, Is.True);
            Assert.That(actual2, Is.True);
            Assert.That(actual3, Is.True);
            Assert.That(actual4, Is.False);
        });
    }

    [Test]
    public void FindNextWordInSentence()
    {
        var wordProcess = new WordsProcess();
        var sentencesProcess = new SentencesProcess();

        var text = "Привет как у тебя дела войнушка?";
        var parser = new TextParser(text, ['?'], [' ']);

        var sentence = parser.Parse()[0];
        
        var actual = sentencesProcess.FindWordInSentence(sentence, "Война", 1, 0.4, true, wordProcess);
        
        Assert.That(actual, Is.Not.Null);
        Assert.That(actual.Value, Is.EqualTo("войнушка"));
    }
    
    [Test]
    public void FindPreviousWordInSentence()
    {
        var wordProcess = new WordsProcess();
        var sentencesProcess = new SentencesProcess();

        var text = "Привет войнушка как у тебя дела?";
        var parser = new TextParser(text, ['?'], [' ']);

        var sentence = parser.Parse()[0];
        
        var actual = sentencesProcess.FindWordInSentence(sentence, "Война", 4, 0.4, false, wordProcess);
        
        Assert.That(actual, Is.Not.Null);
        Assert.That(actual.Value, Is.EqualTo("войнушка"));
    }

    [Test]
    public void CompareSmallWords()
    {
        var wordProcess = new WordsProcess();

        var word1 = new Word("XYZ");
        var word2 = new Word("XYY");
        
        var word12 = new Word("XYZ");
        var word22 = new Word("XYZ");
        
        Assert.Multiple(() =>
        {
            Assert.That(wordProcess.CompareByTrigram(word1, word2, 0.5), Is.EqualTo(false));
            Assert.That(wordProcess.CompareByTrigram(word12, word22, 0.5), Is.EqualTo(true));
        });
    }
    
    [Test]
    public void RuleMatchWordInSentenceWithoutMatches()
    {
        var rawText = "Первое слово обычно правильное но война опасное слово его не надо писать";
        
        var sentence = new TextParser(rawText, ['.'], [' ']).Parse()[0];
    
        var rule = new Rule(new Word("война"), 0.6, 0, 
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
    
        var rule = new Rule(new Word("война"), 0.6, 0, 
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
    
        var rule = new Rule(new Word("война"), 0.6, 0, 
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
        var warRule = new Rule(new Word("война"), 0.4,
            0, 10000);
        var aueRule = new Rule(new Word("ауе"), 1,
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
}