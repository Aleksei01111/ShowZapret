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

public class SequenceFinder
{
    public bool SequenceWordsIsNextFromThreshold
    (List<Sentence> text, int startSentenceIndex, int startWordIndex, List<Word> comparableWords,
        int thresholdWordsInPercent, bool useRegister = false)
    {
        var comparableWordsEdited = new List<Word>(comparableWords);
        var rightWords = 0;
        var wordsRightInPercent = 0.0;

        for (var i = startSentenceIndex; i < text.Count; i++)
        {
            for (var j = startWordIndex; j < text[i].Count; j++)
            {
                var indexOfFound = WordsFinder.ContainsInList(comparableWordsEdited, text[i][j], useRegister);
                if (indexOfFound != -1)
                {
                    comparableWordsEdited.RemoveAt(indexOfFound);
                    rightWords++;
                }

                wordsRightInPercent = (Convert.ToDouble(rightWords) / comparableWords.Count) * 100;

                if (wordsRightInPercent >= thresholdWordsInPercent)
                    return true;
            }
        }

        if (wordsRightInPercent >= thresholdWordsInPercent)
            return true;
        return false;
    }
}

public class WordsFinder
{
    public bool IsNextInText(List<Sentence> text, int startIndexOfSentence, int startIndexOfWord, Word comparableWord, bool useRegister = false)
    {
        for (var i = startIndexOfSentence; i < text.Count; i++)
        {
            for (var j = startIndexOfWord + 1; j < text[i].Count; j++)
            {
                if (CompareTwoWords(comparableWord, text[i][j], useRegister))
                    return true;
            }
        }

        return false;
    }
    
    public bool IsPreviousInText(List<Sentence> text, int startIndexOfSentence, int startIndexOfWord, Word comparableWord, bool useRegister = false)
    {
        for (var i = startIndexOfSentence; i >= 0; i--)
        {
            for (var j = startIndexOfWord - 1; j >= 0; j--)
            {
                if (CompareTwoWords(comparableWord, text[i][j], useRegister))
                    return true;
            }
        }

        return false;
    }
    
    public bool IsNextInCurrentSentence(Sentence sentence, int startWordIndex, Word comparableWord, bool useRegister = false)
    {
        for (var i = startWordIndex + 1; i < sentence.Count; i++)
        {
            if (CompareTwoWords(comparableWord, sentence[i], useRegister))
                return true;
        }

        return false;
    }
    
    public bool IsPreviousInCurrentSentence(Sentence sentence, int startWordIndex, Word comparableWord, bool useRegister = false)
    {
        for (var i = startWordIndex - 1; i >= 0; i--)
        {
            if (CompareTwoWords(comparableWord, sentence[i], useRegister))
                return true;
        }

        return false;
    }

    internal static bool CompareTwoWords(Word word1, Word word2, bool useRegister)
    {
        if ((useRegister && word1.Value == word2.Value) ||
            string.Equals(word1.Value, word2.Value, StringComparison.CurrentCultureIgnoreCase))
            return true;
        return false;
    }

    internal static int ContainsInList(List<Word> words, Word comparableWord, bool useRegister)
    {
        for (var i = 0; i < words.Count; i++)
        {
            if ((useRegister && words[i].Value == comparableWord.Value)
                || string.Equals(words[i].Value, comparableWord.Value, StringComparison.OrdinalIgnoreCase))
                return i;
        }
        return -1;
    }
}