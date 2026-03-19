using System.Windows;
using DB.Entities;

namespace WpfApp.Window;

public partial class MainWindow : System.Windows.Window
{
    public MainWindow()
    {
        var loginDialogResult = new LoginWindow(new User()).ShowDialog();

        if (loginDialogResult != true)
        {
            MessageBox.Show("Вы не вошли");
            Close();
        }
        
        InitializeComponent();
    }
}