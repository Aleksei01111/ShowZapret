using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Zapret;

namespace WpfApp.Converters;

public class WordAnalysisVerdictToBrushConverter : IValueConverter
{
    private SolidColorBrush _cleanWordAnalysisColor = new SolidColorBrush(Colors.LightGreen);
    private SolidColorBrush _exemptedWordAnalysisColor = new SolidColorBrush(Colors.LightYellow);
    private SolidColorBrush _violationWordAnalysisColor = new SolidColorBrush(Colors.LightCoral);
    
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var convertedValue = ((WordAnalysisResult.VerdictType)value);
        
        if (convertedValue == WordAnalysisResult.VerdictType.Clean)
            return _cleanWordAnalysisColor;
        if(convertedValue == WordAnalysisResult.VerdictType.Exempted)
            return _exemptedWordAnalysisColor;
        if(convertedValue == WordAnalysisResult.VerdictType.Violation)
            return _violationWordAnalysisColor;
        
        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var convertedValue = ((SolidColorBrush)value).Color;
        
        if (convertedValue == _cleanWordAnalysisColor.Color)
            return WordAnalysisResult.VerdictType.Clean;
        if(convertedValue == _exemptedWordAnalysisColor.Color)
            return WordAnalysisResult.VerdictType.Exempted;
        if(convertedValue == _violationWordAnalysisColor.Color)
            return WordAnalysisResult.VerdictType.Violation;
        
        return null;
    }
}