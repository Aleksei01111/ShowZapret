using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using DB.Entities;
using DB.Service;
using TextProcess;
using Zapret;
using Color = System.Drawing.Color;

namespace WpfApp.Pages.MainWindow;

public partial class InspectPhrase : Page, INotifyPropertyChanged
{
    private NotesService _notesService = new();
    private RulesService _rulesService = new();
    
    private User _user;
    
    private char[] _sentencesSeparators = ['.', '!', '?'];
    private char[] _wordsSeparators = [' ', ','];
    
    private string _inputText;
    private double? _finalFreedomPunishmentInMonth;
    private double? _finalMoneyPunishmentInRubles;
    
    private List<Rule> _rules = new();
    
    public ObservableCollection<Sentence> Sentences { get; } = new();
    public Dictionary<Sentence, List<WordAnalysisResult>> WordsAnalysis { get; private set; } = new();
    
    public string InputText
    {
        get => _inputText;
        set
        {
            _inputText = value;
            OnPropertyChanged();
        }
    }

    public double? FinalFreedomPunishmentInMonth
    {
        get => _finalFreedomPunishmentInMonth;
        set
        {
            _finalFreedomPunishmentInMonth = value;
            OnPropertyChanged();
        }
    }

    public double? FinalMoneyPunishmentInRubles
    {
        get => _finalMoneyPunishmentInRubles;
        set
        {
            _finalMoneyPunishmentInRubles = value;
            OnPropertyChanged();
        }
    }

    public InspectPhrase(User user)
    {
        _user = user;
        
        InitializeComponent();
        
        DataContext = this;

        LoadRules();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void InspectPhrase_OnClick(object sender, RoutedEventArgs e)
    {
        var sentences = new TextParser(InputText, _sentencesSeparators, _wordsSeparators).Parse();
        SetWordsAnalysis(sentences);

        var punishmentCalculator = new PunishmentCalculator(WordsAnalysis.Values.ToList());
        var punishmentCalculatorResult = punishmentCalculator.GetPunishments();
        FinalFreedomPunishmentInMonth = punishmentCalculatorResult.finalFreedomPunishmentInMonth;
        FinalMoneyPunishmentInRubles = punishmentCalculatorResult.finalMoneyPunishmentInRubles;

        SaveAsNote();
    }

    private void SaveAsNote()
    {
        var note = new Note
        {
            Date = DateTime.Now,
            Text = _inputText,
            User = _user,
        };
        _notesService.SaveNote(note);
    }
    
    private void SetWordsAnalysis(List<Sentence> sentences)
    {
        var textAnalyze = new SentencesAnalyzer(_rules);
        var wordsAnalyses = textAnalyze.AnalyzeText(sentences);
        WordsAnalysis = wordsAnalyses;
        
        OnPropertyChanged(nameof(WordsAnalysis));
    }

    private void LoadRules()
    {
        _rules = _rulesService.GetRules();
    }
}

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