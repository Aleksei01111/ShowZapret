using System.Windows;
using System.Windows.Controls;
using DB.Entities;
using WpfApp.Pages.CreateDonos;

namespace WpfApp.Window;

public partial class CreateDonosWindow
{
    private List<IStepPage> _stepPages = new();
    
    public CreateDonosWindow(Note note, User sender)
    {
        var finalStepPage = new FinalStepPage(OnStepsDialogsDone, null);
        var firstStepPage = new ConfirmLoginStepPage(OnStepsDialogsDone, finalStepPage);
        
        InitializeComponent();

        DataContext = this;

        StepsFrame.Navigate(firstStepPage.ThisPage);
    }

    private void OnStepsDialogsDone(IStepPage thisPage)
    {
        if (thisPage.NextPage is not null)
        {
            StepsFrame.Navigate(thisPage.NextPage.ThisPage);
            MessageBox.Show("Следщ");
        }
        else
        {
            MessageBox.Show("Это конец");
        }
    }
}

public interface IStepPage
{
    public delegate void OnStepDialogDoneDelegate(IStepPage sender);
    
    public OnStepDialogDoneDelegate OnStepDialogDone { get; }
    public IStepPage? NextPage { get; }
    public Page ThisPage { get; }
}