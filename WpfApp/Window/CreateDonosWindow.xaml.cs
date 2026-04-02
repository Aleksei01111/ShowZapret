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

    private DateTime _reportCreationDate = DateTime.Now;
    
    private string _textOfDonos;
    
    public CreateDonosWindow(Note note, User sender)
    {
        _sender = sender;
        
        var reallyFinalStepPage = new ReallyFinalStepPage(OnStepsDialogsDone, null);
        var finalStepPage = new FinalStepPage(OnStepsDialogsDone, reallyFinalStepPage);
        var firstStepPage = new ConfirmLoginStepPage(OnStepsDialogsDone, finalStepPage);
        
        InitializeComponent();

        DataContext = this;

        StepsFrame.Navigate(firstStepPage.ThisPage);

        _textOfDonos =
            "Я - мизулина екатирина екатериновна хочу сообщить о неподабающем поведении и привечь к ответственности всех причастных к данному тексту (см ниже).\n" +
            $"Дата: {_reportCreationDate}\n" +
            $"Учетная запись: {_sender.Login}\n" +
            $"Текст: {note.Text}";
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
                // senderMizulina.Id = 0;
                // senderMizulina.Role.Id = 0;
                var report = new DB.External.EntitiesExternal.Report
                {
                    Date = _reportCreationDate,
                    UserSender = senderMizulina,
                    Text = _textOfDonos,
                };
                new ReportService().SendReport(report);

                new PopUpWindow(_textOfDonos).ShowDialog();
                
                MessageBox.Show("Отчет отправлен");
                DialogResult = true;
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