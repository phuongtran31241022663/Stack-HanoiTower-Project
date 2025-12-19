// Visualizer.cs
// ------------------------------------------------------------
// 1. Cấu hình & Trạng thái
//    - Thành phần UI: Canvas chính, TextBlock thông báo bước.
//    - Thông số tọa độ: Tọa độ X của 3 cọc, baseY, chiều cao đĩa, chiều cao cọc.
//    - Quản lý logic UI: uiTowers (Stack vị trí đĩa), diskUIs (Stack đối tượng đồ họa).
//    - Điều khiển luồng: isPaused, isStopped, isExecuting, delayMs (tốc độ).
//
// 2. Khởi tạo & Vẽ UI
//    - Reset(): Làm sạch Canvas, giải phóng Stack, reset trạng thái hệ thống.
//    - DrawPegs(): Vẽ cọc 3D (đế, thân, nhãn) bằng hiệu ứng Gradient và Shadow.
//    - RegisterInitialDisks(n): Khởi tạo đĩa, gán màu hiện đại, nhãn số và vị trí ban đầu.
//
// 3. Animation & Điều khiển
//    - PlayFromHistory(): Duyệt lịch sử di chuyển và thực thi diễn họa tuần tự.
//    - AnimateMove() & RunAnimation(): Quy trình 3 chặng (Nhấc - Trượt - Hạ) bằng DoubleAnimation.
//    - Các phương thức điều khiển: Pause(), Resume(), Stop(), SetDelay().
//
// 4. Hỗ trợ nội bộ
//    - FindUI(): Thuật toán tìm kiếm đối tượng DiskUI từ Stack bằng bộ nhớ tạm.
//    - SetPosition(): Thiết lập tọa độ tĩnh cho đĩa (dùng khi khởi tạo).
//    - GetModernColor(): Bảng màu hiện đại cho các đĩa.
//    - DiskUI class: Lớp nội bộ đóng gói Rectangle, TextBlock và Size.
// ------------------------------------------------------------
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using WpfApp1.Hanoi;

namespace WpfApp1
{
    public sealed class Visualizer
    {
        #region 1. Cấu hình và trạng thái
        private readonly Canvas canvas;
        private readonly TextBlock stepText;
        private readonly double[] pegX;
        private readonly double baseY;
        private readonly int diskHeight;
        private const double PegHeight = 280;

        // Quản lý trạng thái bằng MyStack
        private readonly DataStructures.MyStack<int>[] uiTowers = { new(), new(), new() };
        private readonly DataStructures.MyStack<DiskUI> diskUIs = new();

        private bool isPaused;
        private bool isStopped;
        private bool isExecuting;
        private int delayMs = 400;
        private int stepCount;

        private readonly string[] pegNames = { "A", "B", "C" };
        #endregion

        #region Constructor
        public Visualizer(Canvas canvas, TextBlock stepText, double[] pegX, double baseY, int diskHeight)
        {
            this.canvas = canvas;
            this.stepText = stepText;
            this.pegX = pegX;
            this.baseY = baseY;
            this.diskHeight = diskHeight;
        }
        #endregion

        #region Khởi tạo và vẽ
        public void Reset()
        {
            isExecuting = false;
            isStopped = true;
            canvas.Children.Clear();

            // Dọn dẹp stack cũ
            while (!diskUIs.IsEmpty()) diskUIs.Pop();
            for (int i = 0; i < 3; i++)
                while (!uiTowers[i].IsEmpty()) uiTowers[i].Pop();

            stepCount = 0;
            isPaused = false;
            if (stepText != null) stepText.Text = "Sẵn sàng";
            DrawPegs();
        }

        private void DrawPegs()
        {
            var woodGradient = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 0),
                GradientStops =
                {
                    new GradientStop(Color.FromRgb(78, 52, 46), 0.0),
                    new GradientStop(Color.FromRgb(121, 85, 72), 0.4),
                    new GradientStop(Color.FromRgb(93, 64, 55), 0.7),
                    new GradientStop(Color.FromRgb(62, 39, 35), 1.0)
                }
            };
            for (int i = 0; i < 3; i++)
            {
                // Đế cọc
                var baseRect = new Rectangle
                {
                    Width = 220, Height = 30, Fill = woodGradient,
                    RadiusX = 8, RadiusY = 8,
                    Effect = new DropShadowEffect { Color = Colors.Black, BlurRadius = 10, Opacity = 0.4 }
                };
                    //BlurRadius = 15,
                    //ShadowDepth = 5,
                    //Opacity = 0.5
                Canvas.SetLeft(baseRect, pegX[i] - 110);
                Canvas.SetTop(baseRect, baseY);
                canvas.Children.Add(baseRect);

                // Đế thân cọc
                var peg = new Rectangle
                {
                    Width = 16, Height = PegHeight, Fill = woodGradient,
                    RadiusX = 8, RadiusY = 8
                };
                Canvas.SetLeft(peg, pegX[i] - 8);
                Canvas.SetTop(peg, baseY - PegHeight + 5);
                canvas.Children.Add(peg);

                // Nhãn tên cọc
                var label = new TextBlock
                {
                    Text = $"Cọc {pegNames[i]}", FontSize = 18,
                    FontWeight = FontWeights.ExtraBold, Foreground = new SolidColorBrush(Color.FromRgb(60, 30, 15)),
                    Width = 220, TextAlignment = TextAlignment.Center
                };
                Canvas.SetLeft(label, pegX[i] - 110);
                Canvas.SetTop(label, baseY + 38);
                canvas.Children.Add(label);
            }
        }

        public void RegisterInitialDisks(int n)
        {
            Reset();
            isStopped = false;

            double maxPossibleWidth = (pegX[1] - pegX[0]) * 0.85; // Chỉ cho phép chiếm 85% khoảng cách
            double minWidth = 45;
            double step = (maxPossibleWidth - minWidth) / Math.Max(n, 1);

            for (int i = n; i >= 1; i--)
            {
                double w = minWidth + i * step;
                Color baseColor = GetModernColor(i);

                var diskBrush = new LinearGradientBrush(
                    Color.Add(baseColor, Color.FromRgb(40, 40, 40)), baseColor, 90);
                var rect = new Rectangle
                {
                    Width = w, Height = diskHeight,
                    Fill = diskBrush,
                    Stroke = Brushes.Black, StrokeThickness = 1,
                    RadiusX = diskHeight / 2, RadiusY = diskHeight / 2,
                    Effect = new DropShadowEffect { BlurRadius = 5, ShadowDepth = 2, Opacity = 0.3 }
                };

                var label = new TextBlock
                {
                    Text = i.ToString(), Width = w, TextAlignment = TextAlignment.Center,
                    Foreground = Brushes.White, FontWeight = FontWeights.Bold, FontSize = 14,
                    VerticalAlignment = VerticalAlignment.Center
                };
                canvas.Children.Add(rect);
                canvas.Children.Add(label);

                var ui = new DiskUI(i, rect, label);
                diskUIs.Push(ui);
                uiTowers[0].Push(i);

                SetPosition(ui, 0, uiTowers[0].Size() - 1);
            }
        }
        #endregion

        #region Animation và phát lại lịch sử
        public async Task PlayFromHistory(DataStructures.MyStack<DataStructures.Move> history)
        {
            if (isExecuting || history.IsEmpty()) return;
            isExecuting = true; isStopped = false;

            try
            {
                // Chuyển history Stack sang List để duyệt xuôi (vì history là Stack LIFO)
                var moves = new List<DataStructures.Move>();
                var temp = new DataStructures.MyStack<DataStructures.Move>();

                while (!history.IsEmpty()) temp.Push(history.Pop()!);
                while (!temp.IsEmpty())
                {
                    var m = temp.Pop()!;
                    moves.Add(m);
                    history.Push(m); // Trả lại để không mất dữ liệu gốc
                }

                foreach (var move in moves)
                {
                    if (isStopped) break;
                    while (isPaused) await Task.Delay(200);

                    // Cập nhật trạng thái logic của UI
                    uiTowers[move.From].Pop();
                    uiTowers[move.To].Push(move.DiskSize);

                    var ui = FindUI(move.DiskSize);
                    int level = uiTowers[move.To].Size() - 1;

                    if (stepText != null)
                    {
                        stepCount++;
                        stepText.Dispatcher.Invoke(() =>
                            stepText.Text = $"Bước {stepCount}: Đĩa {move.DiskSize} ({pegNames[move.From]} → {pegNames[move.To]})");
                    }

                    await AnimateMove(ui, move.To, level);
                    await Task.Delay(Math.Max(10, delayMs / 10));
                }
            }
            finally { isExecuting = false; }
        }

        private async Task AnimateMove(DiskUI ui, int toPeg, int level)
        {
            // Tọa độ đích
            double liftY = baseY - PegHeight - 30;
            double targetX = pegX[toPeg] - ui.Rect.Width / 2;
            double targetY = baseY - (level + 1) * (diskHeight + 2);
            var duration = TimeSpan.FromMilliseconds(delayMs / 3);

            // Quy trình: Nhấc lên -> Di chuyển ngang -> Hạ xuống
            await RunAnimation(ui, Canvas.TopProperty, liftY, duration);
            await RunAnimation(ui, Canvas.LeftProperty, targetX, duration);
            await RunAnimation(ui, Canvas.TopProperty, targetY, duration);
        }

        private Task RunAnimation(DiskUI ui, DependencyProperty prop, double toValue, TimeSpan duration)
        {
            var tcs = new TaskCompletionSource<bool>();
            ui.Rect.Dispatcher.Invoke(() =>
            {
                var anim = new DoubleAnimation(toValue, duration)
                {
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
                };
                anim.Completed += (s, e) => tcs.TrySetResult(true);
                // Chạy animation đồng thời cho cả hình chữ nhật và số trên đĩa
                ui.Rect.BeginAnimation(prop, anim);
                ui.Label.BeginAnimation(prop, anim);
            });
            return tcs.Task;
        }

        public void Pause() => isPaused = true;
        public void Resume() => isPaused = false;
        public void Stop() { isStopped = true; isPaused = false; }
        public void SetDelay(int ms) => delayMs = ms;
        #endregion

        #region Hỗ trợ nội bộ
        // Tìm DiskUI theo kích thước đĩa
        private DiskUI FindUI(int size)
        {
            DiskUI? found = null;
            var temp = new DataStructures.MyStack<DiskUI>();
            while (!diskUIs.IsEmpty())
            {
                var item = diskUIs.Pop()!;
                if (item.Size == size) found = item;
                temp.Push(item);
            }
            while (!temp.IsEmpty()) diskUIs.Push(temp.Pop()!);
            return found ?? throw new Exception($"Không tìm thấy đĩa {size}");
        }
        private void SetPosition(DiskUI ui, int pegIndex, int level)
        {
            double x = pegX[pegIndex] - ui.Rect.Width / 2;
            double y = baseY - (level + 1) * (diskHeight + 2);
            Canvas.SetLeft(ui.Rect, x); Canvas.SetTop(ui.Rect, y);
            Canvas.SetLeft(ui.Label, x); Canvas.SetTop(ui.Label, y);
        }
        private Color GetModernColor(int index)
        {
            Color[] colors = {
                Color.FromRgb(231, 76, 60), Color.FromRgb(230, 126, 34), Color.FromRgb(241, 196, 15),
                Color.FromRgb(46, 204, 113), Color.FromRgb(52, 152, 219), Color.FromRgb(155, 89, 182)
            };
            return colors[(index - 1) % colors.Length];
        }
        private sealed class DiskUI
        {
            public int Size { get; }
            public Rectangle Rect { get; }
            public TextBlock Label { get; }
            public DiskUI(int s, Rectangle r, TextBlock l) { Size = s; Rect = r; Label = l; }
        }
        #endregion
    }
}