using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using DB.Entities;
using DB.Service;

namespace WpfApp.Pages.MainWindow;

public partial class MizulinaPage : Page, INotifyPropertyChanged
{
    private User _user;
    private NotesService _notesService = new();

    private Note _selectedNote; 
    
    public List<Note> Notes { get; private set; }

    public Note SelectedNote
    {
        get => _selectedNote;
        set
        {
            _selectedNote = value;
            OnPropertyChanged();
        }
    }

    public MizulinaPage(User user)
    {
        _user = user;
        
        InitializeComponent();

        DataContext = this;
        LoadAllNotes();
    }

    private void LoadAllNotes()
    {
        Notes = _notesService.GetAllNotes();
        OnPropertyChanged(nameof(Notes));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void MakeReport_OnClick(object sender, RoutedEventArgs e)
    {
        new Window.CreateDonosWindow(SelectedNote, _user).ShowDialog();
    }

    private void RefreshNotes_OnClick(object sender, RoutedEventArgs e)
    {
        LoadAllNotes();
    }
}