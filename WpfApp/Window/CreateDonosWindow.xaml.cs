using System.Windows;
using System.Windows.Controls;
using DB.Entities;
using DB.External.Service;
using WpfApp.Pages.CreateDonos;

namespace WpfApp.Window;

public partial class CreateDonosWindow
{
    private User _sender;
    private List<IStepPage> _stepPages = new();

    /// TODO
    private string _textOfDonos = "todo";
    
    public CreateDonosWindow(Note note, User sender)
    {
        _sender = sender;
        
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
        }
        else
        {
            try
            {
                var userServiceLayer = new DB.External.Service.UserServiceLayer();
                var senderMizulina = userServiceLayer.GetUserMizulina(_sender);
                senderMizulina.Id = 0;
                senderMizulina.Role.Id = 0;
                var report = new DB.External.EntitiesExternal.Report
                {
                    Date = DateTime.Now,
                    UserSender = senderMizulina,
                    Text = _textOfDonos,
                };
                new ReportService().SendReport(report);

                MessageBox.Show("Отчет отправлен");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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