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

public class WordsProcess
{
    public bool CompareByTrigram(Word word1, Word word2, double threshold)
    {
        var valueWord1 = word1.Value;
        var valueWord2 = word2.Value;
        
        valueWord1 = PrepareString(valueWord1);
        valueWord2 = PrepareString(valueWord2);
        
        var word1Trigram = GetTrigrams(valueWord1);
        var word2Trigram = GetTrigrams(valueWord2);

        if (word1Trigram.Count == 0 || word2Trigram.Count == 0) return false;

        var intersection = word1Trigram.Count(x => word2Trigram.Contains(x));
        var union = word1Trigram.Count + word2Trigram.Count - intersection;

        return ((double)intersection / union) >= threshold - 1e-9;
    }

    private string PrepareString(string s) => s.ToLowerInvariant();

    private HashSet<string> GetTrigrams(string s)
    {
        var result = new HashSet<string>();

        for (var i = 0; i < s.Length - 3; i++)
        {
            result.Add(s.Substring(i, 3));
        }
        
        return result;
    }
}