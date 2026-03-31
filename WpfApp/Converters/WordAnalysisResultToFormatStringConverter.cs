using System.Globalization;
using System.Windows.Data;
using Zapret;

namespace WpfApp.Converters;

public class WordAnalysisResultToFormatStringConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var valueAsWordAnalysis = (WordAnalysisResult)value;

        if (valueAsWordAnalysis == null || valueAsWordAnalysis.Verdict == WordAnalysisResult.VerdictType.Clean)
            return "";
        
        var resStr = $"Правило нарушено: {valueAsWordAnalysis.ViolationRule.NameOfRule}";

        if (valueAsWordAnalysis.Verdict == WordAnalysisResult.VerdictType.Exempted)
        {
            resStr += "\nУсловие выполнено: ";
            if (valueAsWordAnalysis.NextRightWord != null)
                resStr += $"Слово после: {valueAsWordAnalysis.NextRightWord}\t";
            if(valueAsWordAnalysis.PreviousRightWord != null)
                resStr += $"Слово до: {valueAsWordAnalysis.PreviousRightWord}\n";

        }

        if (valueAsWordAnalysis.Verdict == WordAnalysisResult.VerdictType.ViolationWithNegativeValue)
            resStr += "\nНо вы вообще крутой если честно";
        return resStr;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}