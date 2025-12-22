using System.Windows;

namespace WpfApp1
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Chạy test
            WpfApp1.TimingTest.RunTests(15);
        }
    }
}