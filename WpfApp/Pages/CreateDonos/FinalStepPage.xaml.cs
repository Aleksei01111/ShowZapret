using System.Windows;
using System.Windows.Controls;
using WpfApp.Window;

namespace WpfApp.Pages.CreateDonos;

public partial class FinalStepPage : Page, IStepPage
{
    public FinalStepPage(IStepPage.OnStepDialogDoneDelegate onStepDialogDone, IStepPage? nextPage)
    {
        OnStepDialogDone = onStepDialogDone;
        NextPage = nextPage;
        
        InitializeComponent();
    }

    public IStepPage.OnStepDialogDoneDelegate OnStepDialogDone { get; }
    public IStepPage? NextPage { get; }
    public Page ThisPage => this;

    private void Done_OnClick(object sender, RoutedEventArgs e)
    {
        OnStepDialogDone(this);
    }
}