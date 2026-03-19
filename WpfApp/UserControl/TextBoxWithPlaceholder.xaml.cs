using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp.UserControl;

public partial class TextBoxWithPlaceholder : System.Windows.Controls.UserControl, INotifyPropertyChanged
{
    private string _placeholder;
    private string _text;
    
    public string Placeholder
    {
        get => _placeholder;
        set
        {
            _placeholder = value;
            OnPropertyChanged();
        }
    }

    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            OnPropertyChanged();
        }
    }
    
    public TextBoxWithPlaceholder()
    {
        InitializeComponent();

        DataContext = this;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}