namespace TextProcess;

public class SentencesProcess
{
    public Word? FindWordInSentence(Sentence sentence, string targetWordValue, 
        int startIndex, double threshold, bool isNext, WordsProcess wordsProcess, HashSet<Word>? wordsExclude = null)
    {
        var step = 1;
        var iterationsCount = sentence.Count - 1 - startIndex;

        var targetWord = new Word(targetWordValue);
        
        if (!isNext)
        {
            step = -1;
            iterationsCount = startIndex;
        }

        for (var i = 1; i <= iterationsCount; i++)
        {
            var offset = i * step;
            var currentWord = sentence[startIndex + offset];
            
            if(wordsProcess.CompareByTrigram(currentWord, targetWord, threshold) &&
               (wordsExclude == null || wordsExclude.Count == 0 || !wordsExclude.Contains(currentWord)))
                return currentWord;
        }

        return null;
    }
}