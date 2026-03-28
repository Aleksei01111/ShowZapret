using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using DB.Entities;
using DB.Service;
using TextProcess;
using Zapret;

namespace WpfApp.Pages.MainWindow;

public partial class RKNEmployeePage : Page, INotifyPropertyChanged
{
    private User _user;

    private RulesService _rulesService = new();

    private Rule _selectedRule;
    private string _inputTestText;
    
    public ObservableCollection<Rule> Rules { get; private set; } = new();

    public Dictionary<Sentence, List<WordAnalysisResult>> WordsAnalysisForTestText { get; private set; } = new();
    
    public Rule SelectedRule
    {
        get => _selectedRule;
        set
        {
            _selectedRule = value;
            OnPropertyChanged(nameof(SelectedRule));
        }
    }

    public string InputTestText
    {
        get => _inputTestText;
        set
        {
            _inputTestText = value;
            OnPropertyChanged(nameof(InputTestText));
        }
    }
    
    public RKNEmployeePage(User user)
    {
        _user = user;
        
        InitializeComponent();

        DataContext = this;
        
        LoadRules();
    }
    
    private void LoadRules()
    {
        Rules = new ObservableCollection<Rule>(_rulesService.GetRules());
        OnPropertyChanged(nameof(Rules));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void AddNewRule_OnClick(object sender, RoutedEventArgs e)
    {
        var newRule = new Rule()
        {
            UserCreator = _user
        };
        Rules.Add(newRule);
        SelectedRule = newRule;
    }

    private void SaveSelectedRule_OnClick(object sender, RoutedEventArgs e)
    {
        if (SelectedRule.NameOfRule.Length == 0 || SelectedRule.TriggerWord.Length == 0)
        {
            MessageBox.Show("Поля имя или слова триггера не могут быть пустыми");
            return;
        }
        
        try
        {
            _rulesService.SaveOrUpdateRule(SelectedRule);
            MessageBox.Show("Сохранено");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void RefreshRules_OnClick(object sender, RoutedEventArgs e)
    {
        LoadRules();
    }

    private void DeleteSelectedRule_OnClick(object sender, RoutedEventArgs e)
    {
        if (MessageBox.Show("Вы уверены?", "Вы уверены?", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            return;

        try
        {
            var selectedRule = SelectedRule;
            Rules.Remove(SelectedRule);
            _rulesService.DeleteRule(selectedRule, true);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void CheckTestInputTextForSelectedRule_OnClick(object sender, RoutedEventArgs e)
    {
        var sentences = new TextParser(_inputTestText, ['.', '?', '!'], [' ', ',']).Parse();
        var textAnalyze = new SentencesAnalyzer([SelectedRule]);
        var wordsAnalyses = textAnalyze.AnalyzeText(sentences);
        WordsAnalysisForTestText = wordsAnalyses;
        
        OnPropertyChanged(nameof(WordsAnalysisForTestText));
    }
}

public class SelectedElementToIsEnableConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
            return false;
        return true;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}