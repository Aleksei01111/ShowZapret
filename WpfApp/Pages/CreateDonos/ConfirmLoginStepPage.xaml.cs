using System.Windows;
using System.Windows.Controls;
using WpfApp.Window;

namespace WpfApp.Pages.CreateDonos;

public partial class ConfirmLoginStepPage : IStepPage
{
    public IStepPage.OnStepDialogDoneDelegate OnStepDialogDone { get; }
    public IStepPage? NextPage { get; }
    public Page ThisPage => this;

    public ConfirmLoginStepPage(IStepPage.OnStepDialogDoneDelegate onStepDialogDone, IStepPage? nextPage)
    {
        OnStepDialogDone = onStepDialogDone;
        NextPage = nextPage;
        
        InitializeComponent();
    }

    private void Done_OnClick(object sender, RoutedEventArgs e)
    {
        OnStepDialogDone(this);
    }
}