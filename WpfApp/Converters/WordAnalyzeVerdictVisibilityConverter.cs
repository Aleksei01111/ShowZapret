using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Zapret;

namespace WpfApp.Converters;

public class WordAnalyzeVerdictVisibilityConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var valueAsVerdict = (WordAnalysisResult.VerdictType)value;
        if(valueAsVerdict == WordAnalysisResult.VerdictType.Clean)
            return Visibility.Hidden;
        return Visibility.Visible;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}