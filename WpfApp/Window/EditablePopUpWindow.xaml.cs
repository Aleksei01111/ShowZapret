using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace WpfApp.Window;

public partial class EditablePopUpWindow : INotifyPropertyChanged
{
    private OnTextEditEndDelegate _onTextEditEnd;
    private string _text;

    public delegate void OnTextEditEndDelegate(string resultText);

    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            OnPropertyChanged();
        }
    }

    public EditablePopUpWindow(string text, OnTextEditEndDelegate textEditEndDelegate, 
        int windowWidth = 400, int windowHeight = 400)
    {
        _onTextEditEnd = textEditEndDelegate;
        Text = text;
        
        InitializeComponent();

        DataContext = this;
        
        Width = windowWidth;
        Height = windowHeight;
    }

    private void EndEdit_OnClick(object sender, RoutedEventArgs e)
    {
        _onTextEditEnd(Text);
        DialogResult = true;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}