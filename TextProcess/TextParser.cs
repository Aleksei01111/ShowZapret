namespace TextProcess;

public class TextParser
{
    private string _rawText;
    private char[] _sentencesSeporators;
    private char[] _wordsSeporators;
    
    public TextParser(string rawText, char[] sentencesSeporators, char[] wordsSeporators)
    {
        _rawText = rawText;
        _sentencesSeporators = sentencesSeporators;
        _wordsSeporators = wordsSeporators;
    }

    public List<Sentence> Parse()
    {
        var result = new List<Sentence>();
        var currentSentenceWords = new List<Word>();
        var currentWord = "";

        for (var i = 0; i < _rawText.Length; i++)
        {
            var c = _rawText[i];

            if (_sentencesSeporators.Contains(c))
            {
                if (currentWord.Length > 0)
                {
                    currentSentenceWords.Add(new Word(currentWord));
                    currentWord = "";
                }
                if (currentSentenceWords.Count > 0)
                {
                    result.Add(new Sentence(new List<Word>(currentSentenceWords), true));
                    currentSentenceWords.Clear();
                }
            }
            else if (_wordsSeporators.Contains(c))
            {
                if (currentWord.Length > 0)
                {
                    currentSentenceWords.Add(new Word(currentWord));
                    currentWord = "";
                }
            }
            else if (char.IsLetter(c) || char.IsDigit(c))
                currentWord += c;

            if (i == _rawText.Length - 1)
            {
                if (currentWord.Length > 0)
                    currentSentenceWords.Add(new Word(currentWord));
                
                if (currentSentenceWords.Count > 0)
                    result.Add(new Sentence(new List<Word>(currentSentenceWords), true));
            }
        }

        return result;
    }
}