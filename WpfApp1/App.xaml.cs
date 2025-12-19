using System.Windows;

namespace WpfApp1
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Chạy test
            WpfApp1.TimingTest.RunTests(17);

            // Giữ App lại bằng cách hiện MainWindow
            var main = new MainWindow();
            main.Show();
        }
    }
}