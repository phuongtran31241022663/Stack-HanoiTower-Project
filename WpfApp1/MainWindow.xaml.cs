using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        // DiskNode – Nút đĩa
        public class DiskNode
        {
            public int size;        // kích thước của đĩa
            public DiskNode next;   // trỏ đến đĩa bên dưới trong stack
            //UI
            public Rectangle visual;// hình chữ nhật tương ứng trên Canva
            public TextBlock label;

        }
        /*DiskNode = “nút đĩa” – giống như đĩa trong stack.

        size = số nguyên biểu thị kích thước.

        visual = đối tượng Rectangle vẽ đĩa lên Canvas.

        next = trỏ đến đĩa bên dưới (giống linked list).*/
        // ---------------- Stack ----------------
        public class MyStack
        {
            public DiskNode top; // đĩa trên cùng

            public bool IsEmpty() => top == null;

            public bool Push(DiskNode node)
            {
                // Nếu cọc trống hoặc đĩa trên cùng lớn hơn → hợp lệ
                if (top == null || top.size > node.size)
                {
                    node.next = top;
                    top = node;
                    return true;
                }

                // Ngược lại → không hợp lệ
                return false;
            }


            public DiskNode Pop()
            {
                if (IsEmpty()) return null;
                DiskNode temp = top;
                top = top.next;
                return temp;
            }

            public int Count() // đếm số đĩa trong stack
            {
                int c = 0;
                for (DiskNode current = top; current != null; current = current.next) c++;
                return c;
            }
        }

        MyStack A = new MyStack();
        MyStack B = new MyStack();
        MyStack C = new MyStack();

        double pegAX = 150, pegBX = 400, pegCX = 650; // tọa độ X của 3 cọc
        double baseY = 350; // đáy cọc
        double diskHeight = 20; // chiều cao mỗi đĩa
        int step = 0; // bước hiện tại

        bool stopRequested = false; // cờ tạm dừng
        bool continueRequested = true; // cờ tiếp tục

        public MainWindow()
        {
            InitializeComponent();
        }

        // ---------------- Buttons ----------------
        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            stopRequested = true;
            continueRequested = true;
            await Task.Delay(100);

            if (!int.TryParse(InputBox.Text, out int n) || n <= 0)
            {
                MessageBox.Show("Nhập số đĩa hợp lệ (>0)");
                return;
            }

            GameArea.Children.Clear();
            A.top = B.top = C.top = null;
            step = 0;
            StepLabel.Content = "Bước: 0";
            stopRequested = false;

            DrawPegs();
            DrawDisks(n);

            await Task.Delay(500);
            await Hanoi(n, A, C, B);
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            stopRequested = true;
            continueRequested = false;
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            stopRequested = false;
            continueRequested = true;
        }

        // DrawPegs – Vẽ cọc
        private void DrawPegs()
        {
            for (int i = 0; i < 3; i++)
            {
                var peg = new Rectangle
                {
                    Width = 10,
                    Height = 200,
                    Fill = Brushes.SaddleBrown
                };
                /* Vẽ 3 cọc bằng Rectangle.
                Canvas là khung vẽ 2D, SetLeft/SetTop = đặt vị trí.
                Mỗi cọc là một hình chữ nhật nâu.*/
                double pegX = i == 0 ? pegAX : i == 1 ? pegBX : pegCX;
                Canvas.SetLeft(peg, pegX);
                Canvas.SetTop(peg, 150);
                GameArea.Children.Add(peg);
            }
        }

        // DrawDisks – Vẽ đĩa
        private void DrawDisks(int n)
        {
            for (int i = n; i >= 1; i--)
            {
                double width = 50 + i * 25;

                // Tính vị trí trước
                double x = pegAX - width / 2;
                double y = baseY - (n - i + 1) * (diskHeight + 3);

                // Tạo hình chữ nhật
                var disk = new Rectangle
                {
                    Width = width,
                    Height = diskHeight,
                    Fill = new LinearGradientBrush(
                        Color.FromRgb((byte)(50 + i * 20), (byte)(100 + i * 10), (byte)(200 - i * 10)),
                        Colors.White, 90),
                    Stroke = Brushes.Black,
                    StrokeThickness = 2,
                    RadiusX = 5,
                    RadiusY = 5
                };

                Canvas.SetLeft(disk, x);
                Canvas.SetTop(disk, y);
                GameArea.Children.Add(disk);

                // -------- Tạo số trên đĩa --------
                TextBlock label = new TextBlock
                {
                    Text = i.ToString(),
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                    Foreground = Brushes.Black
                };

                GameArea.Children.Add(label);
                Canvas.SetZIndex(label, 10);

                // Đặt label ngay giữa đĩa
                double cx = x + width / 2;
                double cy = y + diskHeight / 2;

                Canvas.SetLeft(label, cx - 8);
                Canvas.SetTop(label, cy - 10);

                // Tạo node
                DiskNode node = new DiskNode
                {
                    size = i,
                    visual = disk,
                    label = label
                };

                A.Push(node);
            }
        }


        // Hàm Hanoi – Thuật toán Tháp Hà Nội
        private async Task Hanoi(int n, MyStack src, MyStack dest, MyStack aux)
        {
            if (n == 0) return;

            await Hanoi(n - 1, src, aux, dest);
            await MoveDisk(src, dest);
            await Hanoi(n - 1, aux, dest, src);
        }
        /*n = số đĩa cần di chuyển.
        src = cọc nguồn, dest = cọc đích, aux = cọc phụ.
        Thuật toán đệ quy:
        Chuyển n-1 đĩa sang cọc phụ.
        Di chuyển đĩa lớn nhất sang cọc đích.
        Chuyển n-1 đĩa từ cọc phụ sang cọc đích.*/

        // MoveDisk – Animation di chuyển đĩa
        private async Task MoveDisk(MyStack src, MyStack dest)
        {
            if (src.IsEmpty()) return;

            DiskNode diskNode = src.Pop();
            Rectangle disk = diskNode.visual;

            step++;
            StepLabel.Content = $"Bước {step}: Chuyển đĩa {diskNode.size}";

            double srcX = GetPegX(src);
            double destX = GetPegX(dest);

            double startX = Canvas.GetLeft(disk);
            double startY = Canvas.GetTop(disk);

            int steps = 15;
            double liftHeight = 120;

            // Nâng lên
            for (int i = 0; i < steps; i++)
            {
                await WaitIfStopped();
                double t = (i + 1) / (double)steps;
                Canvas.SetTop(disk, startY - liftHeight * t);
                UpdateLabelPosition(diskNode);
                await Task.Delay(40);
            }

            // Di chuyển ngang
            double dx = destX - startX;
            for (int i = 0; i < steps; i++)
            {
                await WaitIfStopped();
                double t = (i + 1) / (double)steps;
                Canvas.SetLeft(disk, startX + dx * t);
                UpdateLabelPosition(diskNode);
                await Task.Delay(40);
            }

            // Hạ xuống
            int count = dest.Count(); // số đĩa hiện tại
            double targetY = baseY - (count) * (diskHeight + 3);
            double targetX = destX - disk.Width / 2;

            startY = Canvas.GetTop(disk);
            startX = Canvas.GetLeft(disk);
            for (int i = 0; i < steps; i++)
            {
                await WaitIfStopped();
                double t = (i + 1) / (double)steps;
                Canvas.SetTop(disk, startY + (targetY - startY) * t);
                Canvas.SetLeft(disk, startX + (targetX - startX) * t);
                UpdateLabelPosition(diskNode);

                await Task.Delay(50);
            }

            if (!dest.Push(diskNode))
            {
                throw new Exception("Lỗi: cố đặt đĩa lớn lên đĩa nhỏ!");
            }
        }
        /*src.Pop(): lấy đĩa trên cùng ra khỏi stack.
        Canvas.SetTop / SetLeft: di chuyển đĩa trên Canvas.
        dest.Push(): đặt đĩa lên cọc đích (stack quyết định vị trí).*/
        private void UpdateLabelPosition(DiskNode node)
        {
            Rectangle disk = node.visual;

            double cx = Canvas.GetLeft(disk) + disk.Width / 2;
            double cy = Canvas.GetTop(disk) + disk.Height / 2;

            Canvas.SetLeft(node.label, cx - 8);
            Canvas.SetTop(node.label, cy - 10);
        }

        private async Task WaitIfStopped()
        {
            while (stopRequested && !continueRequested)
                await Task.Delay(100);
        }

        private double GetPegX(MyStack peg)
        {
            if (peg == A) return pegAX;
            if (peg == B) return pegBX;
            return pegCX;
        }
    }
}


