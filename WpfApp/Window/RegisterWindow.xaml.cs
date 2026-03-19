using System.Windows;

namespace WpfApp.Window;

public partial class RegisterWindow : System.Windows.Window
{
    public RegisterWindow(DB.Entities.User user)
    {
        InitializeComponent();
    }
}