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

        foreach(var c in _rawText)
        {
            if(_wordsSeporators.Contains(c) && currentWord.Length > 0)
            {
                currentSentenceWords.Add(new Word(currentWord));
                currentWord = "";
            }
            else if(_sentencesSeporators.Contains(c))
            {
                if(currentWord.Length > 0)
                {
                    currentSentenceWords.Add(new Word(currentWord));
                    currentWord = "";
                }
                
                if (currentSentenceWords.Count > 0)
                {
                    result.Add(new Sentence(currentSentenceWords, true));
                    currentSentenceWords.Clear();
                }
            }
            else if(char.IsLetter(c) || (char.IsDigit(c) && currentWord.Length > 0))
            {
                currentWord += c;
            }
        }

        if(currentWord.Length > 0)
        {
            currentSentenceWords.Add(new Word(currentWord));
            result.Add(new Sentence(currentSentenceWords, true));
        }

        return result;
    }
}