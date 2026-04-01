using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace WpfApp.Window;

public partial class PopUpWindow : INotifyPropertyChanged
{
    private string _text;

    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            OnPropertyChanged();
        }
    }
    
    public PopUpWindow(string text)
    {
        Text = text;
        
        InitializeComponent();
        DataContext = this;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void PopUpGrid_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        DialogResult = true;
    }
}