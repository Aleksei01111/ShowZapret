using System.Collections;

namespace TextProcess;

public class Sentence : IList<Word>
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
    public void Add(Word item) =>  _words.Add(item);
    public void Clear() => _words.Clear();
    public bool Contains(Word item) => _words.Contains(item);
    public void CopyTo(Word[] array, int arrayIndex) => _words.CopyTo(array, arrayIndex);
    public bool Remove(Word item) => _words.Remove(item);
    public int Count => _words.Count;
    public bool IsReadOnly => false;
    public int IndexOf(Word item) => _words.IndexOf(item);
    public void Insert(int index, Word item) => _words.Insert(index, item);
    public void RemoveAt(int index) => _words.RemoveAt(index);
    public Word this[int index]
    {
        get => _words[index];
        set => _words[index] = value;
    }
}