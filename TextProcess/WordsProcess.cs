namespace TextProcess;

public class WordsProcess
{
    public bool CompareByTrigram(Word word1, Word word2, double threshold)
    {
        var valueWord1 = word1.Value;
        var valueWord2 = word2.Value;
        
        valueWord1 = PrepareString(valueWord1);
        valueWord2 = PrepareString(valueWord2);

        if (valueWord1.Length <= 3 || valueWord2.Length <= 3)
            return valueWord1 == valueWord2;
        
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