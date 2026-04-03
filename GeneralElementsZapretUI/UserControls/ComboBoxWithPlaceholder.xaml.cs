using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace GeneralElementsZapretUI.UserControls;

public partial class ComboBoxWithPlaceholder : UserControl, INotifyPropertyChanged
{
    private object _selectedItem;
    
    public static readonly DependencyProperty PlaceholderProperty =
        DependencyProperty.Register(
            nameof(Placeholder),
            typeof(string),
            typeof(ComboBoxWithPlaceholder));

    public static readonly DependencyProperty ItemsProperty =
        DependencyProperty.Register(
            nameof(Items),
            typeof(IEnumerable),
            typeof(ComboBoxWithPlaceholder));

    public static readonly DependencyProperty SelectedItemProperty =
        DependencyProperty.Register(
            nameof(SelectedItem),
            typeof(object),
            typeof(ComboBoxWithPlaceholder));
    
    public ItemCollection Items
    {
        get => (ItemCollection)GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }
    
    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public object SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }
    
    public ComboBoxWithPlaceholder()
    {
        InitializeComponent();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}