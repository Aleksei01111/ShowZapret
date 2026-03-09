namespace TextProcess;

public class Word
{
    public string Value { get; set; }

    public Word(): this("")
    {
        
    }
    
    public Word(string value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return Value;
    }
}
