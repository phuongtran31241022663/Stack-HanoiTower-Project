using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using static WpfApp1.MainWindow.DataStructures;


namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        DataStructures.Tower tower;
        HanoiLogic logic;
        TowerVisualizer visualizer;
        public MainWindow()
        {
            InitializeComponent();
            tower ??= new DataStructures.Tower();
            visualizer = new TowerVisualizer(
    MainCanvas,
    PegX,
    BaseY,
    DiskHeight
);
            visualizer.DrawStatic();

            logic = new HanoiLogic();
        }

        // Cấu trúc dữ liệu
        public static class DataStructures
        {
            // Disk – Đĩa
            public class Disk
            {
                public int Size { get; }        // kích thước của đĩa
                internal Disk? Next;   // trỏ đến đĩa bên dưới trong stack
                // internal: chỉ dùng trong cấu trúc dữ liệu, UI không truy cập
                public Disk(int size)
                {
                    Size = size;
                }
            }
            /*Disk = “nút đĩa” – giống như đĩa trong stack.
            size = số nguyên biểu thị kích thước.
            next = trỏ đến đĩa bên dưới (giống linked list).*/
            // Stack
            public class MyStack
            {
                private Disk? Top; // đĩa trên cùng
                public bool IsEmpty() => Top == null;
                public bool Push(Disk d)
                {
                    // Nếu cọc trống hoặc đĩa trên cùng lớn hơn → hợp lệ
                    if (Top == null || Top.Size > d.Size)
                    {
                        d.Next = Top;
                        Top = d;
                        return true;
                    }
                    // Ngược lại → không hợp lệ
                    return false;
                }
                public Disk? Pop()
                {
                    if (IsEmpty()) return null;
                    Disk temp = Top;
                    Top = Top.Next;
                    return temp;
                }
                public int Count() // đếm số đĩa trong stack
                {
                    int c = 0;
                    for (Disk? current = Top; current != null; current = current.Next)
                        c++;
                    return c;
                }
                public IEnumerable<Disk> ToList()
                {
                    for (var cur = Top; cur != null; cur = cur.Next)
                        yield return cur;
                }

                // Phục vụ thuật toán so sánh
                public Disk? Peek() => Top;
                public int PeekSize() => Top?.Size ?? int.MaxValue;
            }
            // Tower chứa 3 cọc
            public class Tower
            {
                private readonly MyStack[] pegs =
                {
                new MyStack(),
                new MyStack(),
                new MyStack()
                };
                public MyStack Get(int i) => pegs[i];
            }
        }
        // Thuật toán
        public class HanoiLogic
        {
            public List<(int From, int To)> Moves = new List<(int From, int To)>();
            public Func<int, int, DataStructures.Disk, Task>? OnMove;

            public async Task MoveBetween(DataStructures.Tower tower, int fromIndex, int toIndex)
            {
                var from = tower.Get(fromIndex);
                var to = tower.Get(toIndex);
                DataStructures.Disk? disk = null;

                // Pop & push thật
                if (!from.IsEmpty() && (to.IsEmpty() || from.PeekSize() < to.PeekSize()))
                {
                    disk = from.Pop();
                    to.Push(disk);
                }
                else if (!to.IsEmpty())
                {
                    disk = to.Pop();
                    from.Push(disk);
                    (fromIndex, toIndex) = (toIndex, fromIndex);
                }

                if (disk != null && OnMove != null)
                    await OnMove(fromIndex, toIndex, disk);
            }
            // Hàm lặp – Iterative
            public async Task SolveIterative(int n, DataStructures.Tower tower)
            {
                int src = 0, aux = 1, dest = 2;
                if (n % 2 == 0) (aux, dest) = (dest, aux);
                int totalMoves = (1 << n) - 1;

                for (int i = 1; i <= totalMoves; i++)
                {
                    if (i % 3 == 1) await MoveBetween(tower, src, dest);
                    else if (i % 3 == 2) await MoveBetween(tower, src, aux);
                    else await MoveBetween(tower, aux, dest);
                }
            }
            // Hàm đệ quy
            public async Task SolveRecursive(int n, int src, int dest, int aux, DataStructures.Tower tower)
            {
                if (n == 0) return;

                await SolveRecursive(n - 1, src, aux, dest, tower);
                await MoveBetween(tower, src, dest);
                await SolveRecursive(n - 1, aux, dest, src, tower);
            }
            /*n = số đĩa cần di chuyển.
            src = cọc nguồn, dest = cọc đích, aux = cọc phụ.
            Thuật toán đệ quy:
            Chuyển n-1 đĩa sang cọc phụ.
            Di chuyển đĩa lớn nhất sang cọc đích.
            Chuyển n-1 đĩa từ cọc phụ sang cọc đích.*/

            // Hàm không đệ quy
            public async Task SolveNonRecursive(int n, DataStructures.Tower tower)
            {
                int src = 0, aux = 1, dest = 2;
                if (n % 2 == 0) (aux, dest) = (dest, aux);

                int totalMoves = (1 << n) - 1;

                for (int i = 1; i <= totalMoves; i++)
                {
                    // Lặp theo thứ tự 3 cặp cọc
                    if (i % 3 == 1)
                        await MoveBetween(tower, src, dest);
                    else if (i % 3 == 2)
                        await MoveBetween(tower, src, aux);
                    else
                        await MoveBetween(tower, aux, dest);
                }
            }
        }

        // UI

        private int totalDisks = 0;
        private readonly double[] PegX = { 225, 450, 675 };
        private const double BaseY = 420;
        private const double pegHeight = 220;
        private const int DiskHeight = 25;

        private int moveDelay = 500;

        private bool isPaused = false;
        private bool stepMode = false;
        private bool nextStep = false;
        private CancellationTokenSource? cts;
        private bool isRestarting = false;

        class TowerVisualizer
        {
            private readonly Canvas canvas;
            private readonly Dictionary<DataStructures.Disk, Rectangle> diskUI = new();
            private readonly Dictionary<DataStructures.Disk, TextBlock> labelUI = new();

            private readonly double[] PegX;
            private readonly double BaseY;
            private readonly int DiskHeight;
            private const double PegHeight = 220;

            public TowerVisualizer(Canvas canvas, double[] pegX, double baseY, int diskHeight)
            {
                this.canvas = canvas;
                PegX = pegX;
                BaseY = baseY;
                DiskHeight = diskHeight;
            }
            public void ClearAll()
            {
                canvas.Children.Clear();
                diskUI.Clear();
                labelUI.Clear();
            }

            public void RegisterDisk(DataStructures.Disk d)
            {
                double width = 40 + d.Size * 20;

                var rect = new Rectangle
                {
                    Width = width,
                    Height = DiskHeight,
                    Fill = new SolidColorBrush(Color.FromRgb((byte)(60 + d.Size * 20), 100, 200)),
                    Stroke = Brushes.Black,
                    RadiusX = 4,
                    RadiusY = 4
                };

                var lbl = new TextBlock
                {
                    Text = d.Size.ToString(),
                    Width = width,
                    TextAlignment = TextAlignment.Center,
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.Bold
                };

                diskUI[d] = rect;
                labelUI[d] = lbl;

                canvas.Children.Add(rect);
                canvas.Children.Add(lbl);
            }
            public void DrawStatic()
            {
                string[] names = { "A", "B", "C" };

                for (int i = 0; i < 3; i++)
                {
                    // Cọc
                    var peg = new Rectangle
                    {
                        Width = 10,
                        Height = PegHeight,
                        Fill = Brushes.Sienna
                    };
                    Canvas.SetLeft(peg, PegX[i] - 5);
                    Canvas.SetTop(peg, BaseY - PegHeight);
                    canvas.Children.Add(peg);

                    // Đế
                    var baseRect = new Rectangle
                    {
                        Width = 160,
                        Height = 10,
                        Fill = Brushes.Peru
                    };
                    Canvas.SetLeft(baseRect, PegX[i] - 80);
                    Canvas.SetTop(baseRect, BaseY);
                    canvas.Children.Add(baseRect);

                    // Nhãn
                    var label = new TextBlock
                    {
                        Text = names[i],
                        FontSize = 16,
                        FontWeight = FontWeights.Bold
                    };
                    Canvas.SetLeft(label, PegX[i] - 8);
                    Canvas.SetTop(label, BaseY + 12);
                    canvas.Children.Add(label);
                }
            }
            // 2️⃣ Khởi tạo đĩa (chỉ làm 1 lần)
            public void InitDisks(DataStructures.Tower tower, int n)
            {
                for (int i = n; i >= 1; i--)
                {
                    var d = new DataStructures.Disk(i);
                    tower.Get(0).Push(d);
                    RegisterDisk(d);
                    SetDiskPosition(d, 0, n - i);
                }
            }

            public void SetDiskPosition(DataStructures.Disk d, int pegIndex, int level)
            {
                var rect = diskUI[d];
                var lbl = labelUI[d];

                double x = PegX[pegIndex] - rect.Width / 2;
                double y = BaseY - (level + 1) * (DiskHeight + 4);

                Canvas.SetLeft(rect, x);
                Canvas.SetTop(rect, y);
                Canvas.SetLeft(lbl, x);
                Canvas.SetTop(lbl, y);
            }
            public async Task MoveDisk(DataStructures.Disk d, int pegIndex, int level)
            {
                if (!diskUI.ContainsKey(d)) return; // tránh lỗi null
                var rect = diskUI[d];
                var lbl = labelUI[d];

                double targetX = PegX[pegIndex] - rect.Width / 2;
                double targetY = BaseY - (level + 1) * (DiskHeight + 4);
                double liftY = BaseY - PegHeight - DiskHeight;

                await AnimateY(rect, lbl, liftY);
                await AnimateX(rect, lbl, targetX);
                await AnimateY(rect, lbl, targetY);
            }
            private Task AnimateX(UIElement r, UIElement l, double x)
            {
                var tcs = new TaskCompletionSource<bool>();
                var anim = new DoubleAnimation(x, TimeSpan.FromMilliseconds(300));
                anim.Completed += (_, _) => tcs.TrySetResult(true);
                r.BeginAnimation(Canvas.LeftProperty, anim);
                l.BeginAnimation(Canvas.LeftProperty, anim);
                return tcs.Task;
            }

            private Task AnimateY(UIElement r, UIElement l, double y)
            {
                var tcs = new TaskCompletionSource<bool>();
                var anim = new DoubleAnimation(y, TimeSpan.FromMilliseconds(300));
                anim.Completed += (_, _) => tcs.TrySetResult(true);
                r.BeginAnimation(Canvas.TopProperty, anim);
                l.BeginAnimation(Canvas.TopProperty, anim);
                return tcs.Task;
            }
        }
        private async void Run_Click(object sender, RoutedEventArgs e)
        {
            // Nếu đang chạy tiến trình cũ -> dừng và chạy lại mới
            if (cts != null)
            {
                isRestarting = true;       // 🔸 Đánh dấu là restart
                cts.Cancel();              // 🔸 Dừng tiến trình hiện tại
                return;                    // 🔸 Đợi tiến trình cũ dừng
            }

            // 🔹 Nếu không có tiến trình nào -> bắt đầu chạy mới
            await StartNewRun();
        }

        private async Task StartNewRun()
        {
            cts = new CancellationTokenSource();

            // 🔹 RESET trạng thái trước khi chạy
            isPaused = false;          // ✅ rất quan trọng!
            stepMode = false;
            nextStep = false;

            RunButton.IsEnabled = true;
            PauseButton.IsEnabled = true;
            ResumeButton.IsEnabled = false;

            try
            {
                await RunAsync(cts.Token);
            }
            catch (OperationCanceledException)
            {
                if (isRestarting)
                {
                    isRestarting = false;
                    cts = null;
                    await StartNewRun();   // 🔁 chạy lại ngay
                    return;
                }

                await Dispatcher.InvokeAsync(() =>
                {
                    StepDescription.Text = "🛑 Thuật toán cũ đã dừng.";
                });
            }
            finally
            {
                cts = null;
                RunButton.IsEnabled = true;
                PauseButton.IsEnabled = false;
                ResumeButton.IsEnabled = false;
            }
        }
        private async Task RunAsync(CancellationToken token)
        {
            logic.Moves.Clear();
            visualizer.ClearAll();

            if (!int.TryParse(DiskCountTextBox.Text, out int n) || n < 1 || n > 8)
            {
                MessageBox.Show("Nhập số đĩa từ 1 đến 8!", "Lỗi nhập liệu");
                return;
            }

            tower = new DataStructures.Tower();
            visualizer.DrawStatic();
            visualizer.InitDisks(tower, n);

            string algo = (AlgorithmComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Recursive";
            await Dispatcher.InvokeAsync(() => StepDescription.Text = $"Thuật toán: {algo}");

            logic.OnMove = async (from, to, disk) =>
            {
                token.ThrowIfCancellationRequested();
                await WaitIfPausedAsync(token);

                await Dispatcher.InvokeAsync(() =>
                {
                    StepDescription.Text = $"→ Di chuyển đĩa {disk.Size} từ {(char)('A' + from)} sang {(char)('A' + to)}";
                });

                int level = tower.Get(to).Count() - 1;
                await visualizer.MoveDisk(disk, to, level);
                await Task.Delay(moveDelay, token);
            };

            await Task.Yield();

            switch (algo)
            {
                case "Recursive":
                    await logic.SolveRecursive(n, 0, 2, 1, tower);
                    break;
                case "Iterative":
                    await logic.SolveIterative(n, tower);
                    break;
                case "NonRecursive":
                    await logic.SolveNonRecursive(n, tower);
                    break;
            }

            await Dispatcher.InvokeAsync(() => StepDescription.Text = "✅ Hoàn thành!");
        }
        private async Task WaitIfPausedAsync(CancellationToken token)
        {
            while (isPaused || (stepMode && !nextStep))
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(50, token);
            }
            nextStep = false;
        }
        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            isPaused = true;
            PauseButton.IsEnabled = false;
            ResumeButton.IsEnabled = true;
            RunButton.IsEnabled = true;   // ✅ Cho phép bấm Run trong khi Pause
        }

        private void Resume_Click(object sender, RoutedEventArgs e)
        {
            isPaused = false;
            PauseButton.IsEnabled = true;
            ResumeButton.IsEnabled = false;
            RunButton.IsEnabled = true;   // ✅ vẫn bật để có thể chạy lại nếu muốn
        }
        private void NumberOnly(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(char.IsDigit);
        }
    }
}