using TextProcess;

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
    public void Parse_OneSimpleSentence_CorrectlySplitWords()
    {
        var text = "Привет как дела";
        var parser = new TextParser(text, DefaultSentenceSeparators, DefaultWordSeparators);

        var sentences = parser.Parse();

        Assert.That(sentences, Has.Count.EqualTo(1));
        
        var words = sentences[0].ToList();
        Assert.That(words, Has.Count.EqualTo(3));
        
        Assert.That(words[0].Value, Is.EqualTo("Привет"));
        Assert.That(words[1].Value, Is.EqualTo("как"));
        Assert.That(words[2].Value, Is.EqualTo("дела"));
    }
    
    [Test]
    public void Parse_MultipleSentences_CorrectCountAndContent()
    {
        var text = "Первый. Второй! Третий? Четвёртый...";
        var parser = new TextParser(text, DefaultSentenceSeparators, DefaultWordSeparators);

        var sentences = parser.Parse();

        Assert.That(sentences, Has.Count.EqualTo(4));

        Assert.That(sentences[0].ToList()[0].Value, Is.EqualTo("Первый"));
        Assert.That(sentences[1].ToList()[0].Value, Is.EqualTo("Второй"));
        Assert.That(sentences[2].ToList()[0].Value, Is.EqualTo("Третий"));
        Assert.That(sentences[3].ToList()[0].Value, Is.EqualTo("Четвёртый"));
    }
    
    [Test]
    public void Parse_SentenceWithPunctuationInside_StaysInWordOrHandledCorrectly()
    {
        var text = "Привет, как дела? Всё \"хорошо\", сказал он...";
        var parser = new TextParser(text, new[] { '.', '!', '?' }, new[] { ' ', '\t' });

        var sentences = parser.Parse();

        Assert.That(sentences, Has.Count.EqualTo(2));

        var first = sentences[0].ToList();
        Assert.That(first.Select(w => w.Value), Is.EqualTo(new[]
        {
            "Привет", "как", "дела"
        }));

        var second = sentences[1].ToList();
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
        Assert.That(sentences[0].ToList()[0].Value, Is.EqualTo("Один"));
        Assert.That(sentences[1].ToList()[0].Value, Is.EqualTo("Два"));
        Assert.That(sentences[2].ToList()[0].Value, Is.EqualTo("Три"));
    }
    
    [Test]
    public void Parse_UsesCustomWordSeparators_Correctly()
    {
        var text = "слово1,слово2;слово3 слово4";
        char[] customWordSep = { ',', ';', ' ' };
        
        var parser = new TextParser(text, DefaultSentenceSeparators, customWordSep);

        var sentences = parser.Parse();

        Assert.That(sentences, Has.Count.EqualTo(1));
        
        var words = sentences[0].ToList().Select(w => w.Value).ToList();
        Assert.That(words, Is.EqualTo(new[] { "слово1", "слово2", "слово3", "слово4" }));
    }
    
    [Test]
    public void Parse_NoSentenceSeparators_AllInOneSentence()
    {
        var text = "Это просто текст без точек и знаков";
        var parser = new TextParser(text, new char[0], DefaultWordSeparators);

        var sentences = parser.Parse();

        Assert.That(sentences, Has.Count.EqualTo(1));
        Assert.That(sentences[0].Count(), Is.EqualTo(7));
    }
}