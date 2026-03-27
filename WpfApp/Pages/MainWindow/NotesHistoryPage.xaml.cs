using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using DB.Entities;
using DB.Service;

namespace WpfApp.Pages.MainWindow;

public partial class NotesHistoryPage : Page, INotifyPropertyChanged
{
    private User _user;
    private NotesService _notesService = new NotesService();

    private Note? _selectedNote;

    public List<Note> Notes { get; private set; } = new();

    public Note? SelectedNote
    {
        get => _selectedNote;
        set
        {
            _selectedNote = value;
            OnPropertyChanged(nameof(SelectedNote));
        }
    }
    
    public NotesHistoryPage(User user)
    {
        _user = user;
        
        InitializeComponent();

        DataContext = this;
        LoadAllNotes();
    }

    private void LoadAllNotes()
    {
        Notes = _notesService.GetNotesForUser(_user);
        OnPropertyChanged(nameof(Notes));
    }

    private void RefreshNotesHistory_OnClick(object sender, RoutedEventArgs e)
    {
        LoadAllNotes();
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}