using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using WpfApp1; // Đảm bảo namespace này khớp với MainWindow của bạn

namespace WpfApp1.Test
{
    [TestClass]
    public class VisualizerTests
    {
        private MainWindow _window;

        [TestInitialize]
        public void Setup()
        {
            // Khởi tạo MainWindow trên luồng UI
            _window = new MainWindow();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _window.Close();
        }

        [TestMethod]
        public void Test_NumberOfDisks_CreatedCorrectly()
        {
            _window.Dispatcher.Invoke(() =>
            {
                // Giả lập click nút khởi tạo
                _window.BtnCreate_Click(null, null);

                var canvas = _window.MainCanvas;
                // Đĩa có chiều cao cố định là 22 (dựa theo code Visualizer của bạn)
                int diskCount = canvas.Children.OfType<Rectangle>().Count(r => r.Height == 22);

                Assert.AreEqual(3, diskCount, "Số lượng đĩa khởi tạo không đúng");
            });
        }

        [TestMethod]
        public void Test_ZIndex_LabelShouldBeAboveRectangle()
        {
            _window.Dispatcher.Invoke(() =>
            {
                _window.BtnCreate_Click(null, null);

                var canvas = _window.MainCanvas;
                // Tìm Rectangle đầu tiên có chiều cao 22 và TextBlock tương ứng
                var firstRect = canvas.Children.OfType<Rectangle>().First(r => r.Height == 22);
                var firstLabel = canvas.Children.OfType<TextBlock>().First();

                int rectIndex = canvas.Children.IndexOf(firstRect);
                int labelIndex = canvas.Children.IndexOf(firstLabel);

                // Trong WPF, object nào được add vào sau sẽ nằm trên (Z-index cao hơn)
                Assert.IsTrue(labelIndex > rectIndex, "Số đang bị nằm dưới đĩa!");
            });
        }
    }
}