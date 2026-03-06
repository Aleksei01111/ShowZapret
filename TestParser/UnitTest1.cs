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
    public void FindWordInCurrentSentence()
    {
        var sentence = "Привет, как у тебя дела?";
        var parser = new TextParser(sentence, ['?', '.'], [' ', ',']);
        var sentences = parser.Parse();

        var wordsFinder = new WordsFinder();

        var actual = wordsFinder.IsNextInCurrentSentence(sentences[0], 0, new Word("делА"));
        var actual2 = wordsFinder.IsNextInCurrentSentence(sentences[0], 0, new Word("ааа"));
        var actual3 = wordsFinder.IsPreviousInCurrentSentence(sentences[0], 2, new Word("привет"));
        Assert.Multiple(() =>
        {
            Assert.That(actual, Is.True);
            Assert.That(actual2, Is.False);
            Assert.That(actual3, Is.True);
        });
    }

    [Test]
    public void FindWordInText()
    {
        var sentence = "Привет, как у тебя дела? У меня нормально, а у тебя?";
        var parser = new TextParser(sentence, ['?', '.'], [' ', ',']);
        var sentences = parser.Parse();

        var wordsFinder = new WordsFinder();

        var actualNext = wordsFinder.IsNextInText(sentences, 0, 1, new Word("нормально"));
        var actualNext2 = wordsFinder.IsNextInText(sentences, 1, 1, new Word("тебя"));
        var actualNext3 = wordsFinder.IsNextInText(sentences, 1,1, new Word("аа"));
        
        var actualPrevious = wordsFinder.IsPreviousInText(sentences, 1,1, new Word("нормально"));
        var actualPrevious2 = wordsFinder.IsPreviousInText(sentences, 1,3, new Word("нормально"));
        var actualPrevious3 = wordsFinder.IsPreviousInText(sentences, 1,1, new Word("Привет"));
        
        
        Assert.Multiple(() =>
        {
            Assert.That(actualNext, Is.True);
            Assert.That(actualNext2, Is.True);
            Assert.That(actualNext3, Is.False);
            
            Assert.That(actualPrevious, Is.False);
            Assert.That(actualPrevious2, Is.True);
            Assert.That(actualPrevious3, Is.True);
        });
    }

    [Test]
    public void NextRightWordsInSimpleText()
    {
        var rawText = "Привет Z. z. V. O. Как дела?";
        var parser = new TextParser(rawText, ['.', '?'], [' ']);
        var text = parser.Parse();
        
        var rightSequence = new List<Word> {new("z"), new("z"), new("v"), new("o")};
        
        var sequenceFinder = new SequenceFinder();

        var actual = sequenceFinder.SequenceWordsIsNextFromThreshold(text, 0,0, rightSequence, 80);
        
        Assert.That(actual, Is.True);
        Assert.That(rightSequence.Count, Is.Not.EqualTo(0));
    }
    
    [Test]
    public void NextRightWordsInComplexText()
    {
        var rawText = "Привет, меня зовут z илья. Я давно не делал v репа. Я не думал что вы меня знаете, o но я мазезелов. Ха-ха-ха. Не, я крут Привет, меня зовут z илья. Я давно не делал v репа. Я не думал что вы меня знаете, o но я мазезелов. Ха-ха-ха. Не, я крут Привет, меня зовут z илья. Я давно не делал v репа. Я не думал что вы меня знаете, o но я мазезелов. Ха-ха-ха. Не, я крут";
        
        var parser = new TextParser(rawText, ['.', '?'], [' ']);
        var text = parser.Parse();
        
        var rightSequence = new List<Word> {new("z"), new("z"), new("v"), new("o")};
        
        var sequenceFinder = new SequenceFinder();
        var actual = sequenceFinder.SequenceWordsIsNextFromThreshold(text, 0,0, rightSequence, 100);
        
        Assert.That(actual, Is.True);
    }
}