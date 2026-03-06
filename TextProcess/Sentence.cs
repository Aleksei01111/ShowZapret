using System.Collections;

namespace TextProcess;

public class Sentence : IEnumerable<Word>
{
    private List<Word> _words;

    public Sentence(List<Word> words)
    {
        _words = words;
    }

    public Sentence(List<Word> words, bool copy)
    {
        if(copy)
        {
            _words = new List<Word>();
            foreach (var word in words)
            {
                _words.Add(word);
            }
        }
        else
        {
            _words = words;
        }
    }

    IEnumerator<Word> IEnumerable<Word>.GetEnumerator() => _words.GetEnumerator();

    public IEnumerator GetEnumerator() => _words.GetEnumerator();
}