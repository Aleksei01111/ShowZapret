using System.Windows;
using System.Windows.Controls;

namespace GeneralElementsZapretUI.UserControls;

public partial class DatePickerWithPlaceholder : UserControl
{
    public static readonly DependencyProperty PlaceholderProperty = 
        DependencyProperty.Register(
            nameof(Placeholder),
            typeof(string),
            typeof(DatePickerWithPlaceholder));

    public static readonly DependencyProperty DateProperty =
        DependencyProperty.Register(
            nameof(SelectedDate),
            typeof(DateTime),
            typeof(DatePickerWithPlaceholder));
    
    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public DateTime SelectedDate
    {
        get => (DateTime)GetValue(DateProperty);
        set => SetValue(DateProperty, value);
    }
    
    public DatePickerWithPlaceholder()
    {
        InitializeComponent();
    }
}