using System;
using System.Buffers.Text;
using System.Net.NetworkInformation;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using static WpfApp1.MainWindow.DT;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public class DT // Cấu trúc dữ liệu
        {
            // Disk – Đĩa
            public class Disk
            {
                public int Size;        // kích thước của đĩa
                public Disk? Next;   // trỏ đến đĩa bên dưới trong stack
                public Disk(int size)
                {
                    Size = size;
                    Next = null;
                }
            }
            /*Disk = “nút đĩa” – giống như đĩa trong stack.

            size = số nguyên biểu thị kích thước.

            next = trỏ đến đĩa bên dưới (giống linked list).*/
            // ---------------- Stack ----------------
            public class MyStack
            {
                public Disk? Top; // đĩa trên cùng

                public bool IsEmpty() => Top == null;

                public bool Push(Disk node)
                {
                    // Nếu cọc trống hoặc đĩa trên cùng lớn hơn → hợp lệ
                    if (Top == null || Top.Size > node.Size)
                    {
                        node.Next = Top;
                        Top = node;
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
            }
            // Tower chứa 3 cọc
            public class Tower
            {
                public MyStack A = new MyStack();
                public MyStack B = new MyStack();
                public MyStack C = new MyStack();
            }

        }

        public class HanoiLogic //BACKEND -- Thuật toán
        {
            public class HanoiFrame
            {
                public int n;
                public int src, dest, aux;
                public int state;

                public HanoiFrame(int n, int src, int dest, int aux, int state = 0)
                {
                    this.n = n;
                    this.src = src;
                    this.dest = dest;
                    this.aux = aux;
                    this.state = state;
                }
            }

            public class FrameNode
            {
                public HanoiFrame Data;
                public FrameNode? Next;

                public FrameNode(HanoiFrame data)
                {
                    Data = data;
                    Next = null;
                }
            }

            public class FrameStack
            {
                private FrameNode? top;

                public bool IsEmpty() => top == null;

                public void Push(HanoiFrame frame)
                {
                    FrameNode node = new FrameNode(frame);
                    node.Next = top;
                    top = node;
                }

                public HanoiFrame? Pop()
                {
                    if (top == null) return null;
                    HanoiFrame f = top.Data;
                    top = top.Next;
                    return f;
                }
            }
            public class MoveDisk
            {
                public int From, To;
                public MoveDisk? Next;

                public MoveDisk(int from, int to)
                {
                    From = from;
                    To = to;
                    Next = null;
                }
            }

            public class MoveStack
            {
                public MoveDisk? Top;

                public bool IsEmpty() => Top == null;

                public void Push(int from, int to)
                {
                    MoveDisk node = new MoveDisk(from, to);
                    node.Next = Top;
                    Top = node;
                }
                public MoveDisk? Pop()
                {
                    if (Top == null) return null;
                    var t = Top;
                    Top = Top.Next;
                    return t;
                }
            }
            public int Step { get; private set; } = 0;
            public MoveStack Moves = new MoveStack();

            private void AddMove(int from, int to)
            {
                Moves.Push(from, to);
                Step++;
            }

            // Hàm lặp – Iterative
            public void SolveIterative(int n)
            {
                MyStack[] towers = { new MyStack(), new MyStack(), new MyStack() };
                for (int i = n; i >= 1; i--)
                    towers[0].Push(new Disk(i));

                int src = 0, aux = 1, dest = 2;
                if (n % 2 == 0) (aux, dest) = (dest, aux);

                int totalMoves = (1 << n) - 1;

                for (int i = 1; i <= totalMoves; i++)
                {
                    int from = -1, to = -1;

                    switch (i % 3)
                    {
                        case 1: from = src; to = dest; break;
                        case 2: from = src; to = aux; break;
                        case 0: from = aux; to = dest; break;
                    }

                    // Chọn hướng hợp lệ
                    int topFrom = towers[from].Top?.Size ?? int.MaxValue;
                    int topTo = towers[to].Top?.Size ?? int.MaxValue;

                    if (topFrom < topTo)
                    {
                        towers[to].Push(towers[from].Pop()!);
                        AddMove(from, to);
                    }
                    else
                    {
                        towers[from].Push(towers[to].Pop()!);
                        AddMove(to, from);
                    }
                }
            }


            // Hàm Hanoi – Thuật toán Tháp Hà Nội
            public void SolveRecursive(int n, int src, int dest, int aux)
            {
                if (n == 0) return;

                SolveRecursive(n - 1, src, aux, dest);

                AddMove(src, dest);

                SolveRecursive(n - 1, aux, dest, src);
            }
            /*n = số đĩa cần di chuyển.
            src = cọc nguồn, dest = cọc đích, aux = cọc phụ.
            Thuật toán đệ quy:
            Chuyển n-1 đĩa sang cọc phụ.
            Di chuyển đĩa lớn nhất sang cọc đích.
            Chuyển n-1 đĩa từ cọc phụ sang cọc đích.*/

            public void SolveNonRecursive(int n)
            {
                FrameStack st = new FrameStack();

                st.Push(new HanoiFrame(n, 0, 2, 1));

                while (!st.IsEmpty())
                {
                    HanoiFrame f = st.Pop()!;

                    if (f.n == 0) continue;

                    if (f.state == 0)
                    {
                        f.state = 1;
                        st.Push(f);

                        st.Push(new HanoiFrame(
                            f.n - 1,
                            f.src,
                            f.aux,
                            f.dest
                        ));
                    }
                    else
                    {
                        AddMove(f.src, f.dest);

                        st.Push(new HanoiFrame(
                            f.n - 1,
                            f.aux,
                            f.dest,
                            f.src
                        ));
                    }
                }
            }
        }
        //-----------UI------------
        public MainWindow()
        {
            InitializeComponent();
        }
        DT.Tower tower = new DT.Tower();
        HanoiLogic logic = new HanoiLogic();

        int totalDisks = 0;
        const int DiskHeight = 20;
        const int BaseY = 350;
        bool isPaused = false;
        bool stepMode = false;
        bool nextStep = false;


        // --- Vẽ 3 cọc + đế ---
        void DrawPegs()
        {
            double pegHeight = 100 + totalDisks * 25;

            for (int i = 0; i < 3; i++)
            {
                double xCenter = 150 + i * 250;

                // Thân cọc
                Rectangle peg = new Rectangle
                {
                    Width = 10,
                    Height = pegHeight,
                    Fill = Brushes.SaddleBrown,
                    RadiusX = 3,
                    RadiusY = 3
                };
                Canvas.SetLeft(peg, xCenter - peg.Width / 2);
                Canvas.SetTop(peg, BaseY - pegHeight);
                Canvas.SetZIndex(peg, 0);
                MainCanvas.Children.Add(peg);

                // Đế
                Rectangle basePlate = new Rectangle
                {
                    Width = 120,
                    Height = 10,
                    Fill = Brushes.Peru,
                    RadiusX = 2,
                    RadiusY = 2
                };
                Canvas.SetLeft(basePlate, xCenter - basePlate.Width / 2);
                Canvas.SetTop(basePlate, BaseY);
                Canvas.SetZIndex(basePlate, 0);
                MainCanvas.Children.Add(basePlate);
            }
        }
        // --- Dictionary ánh xạ Disk -> UI ---
        Dictionary<DT.Disk, Rectangle> diskUI = new();
        Dictionary<DT.Disk, TextBlock> labelUI = new();

        // --- Khởi tạo đĩa UI ---
        void InitDisksUI(int n)
        {
            tower = new DT.Tower();
            diskUI.Clear();
            labelUI.Clear();

            for (int i = n; i >= 1; i--)
            {
                var d = new DT.Disk(i);
                tower.A.Push(d);

                double w = DiskWidth(d);
                Rectangle rect = new Rectangle
                {
                    Width = w,
                    Height = DiskHeight,
                    RadiusX = 5,
                    RadiusY = 5,
                    Fill = new LinearGradientBrush(
                        Color.FromRgb((byte)(50 + d.Size * 20),
                                      (byte)(100 + d.Size * 10),
                                      (byte)(200 - d.Size * 10)),
                        Colors.White, 90),
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };
                Canvas.SetZIndex(rect, 1);

                TextBlock label = new TextBlock
                {
                    Text = d.Size.ToString(),
                    Width = w,
                    FontWeight = FontWeights.Bold,
                    Foreground = Brushes.White,
                    TextAlignment = TextAlignment.Center
                };
                Canvas.SetZIndex(label, 2);

                diskUI[d] = rect;
                labelUI[d] = label;
                MainCanvas.Children.Add(rect);
                MainCanvas.Children.Add(label);
            }
        }
        double DiskWidth(DT.Disk d)
        {
            double maxWidth = 150; // tối đa, tránh chồng
            double minWidth = 50;
            return minWidth + (maxWidth - minWidth) * (d.Size - 1) / (totalDisks - 1);
        }
        void DrawDisks()
        {
            DrawStackUI(tower.A, 0);
            DrawStackUI(tower.B, 1);
            DrawStackUI(tower.C, 2);
        }

        // --- Vẽ lại stack ---
        void DrawStackUI(DT.MyStack stack, int pegIndex)
        {
            var disks = new List<DT.Disk>();
            for (var d = stack.Top; d != null; d = d.Next)
                disks.Add(d);
            disks.Reverse();

            double xCenter = 150 + pegIndex * 250;

            for (int i = 0; i < disks.Count; i++)
            {
                var d = disks[i];
                double x = xCenter - diskUI[d].Width / 2;
                double y = BaseY - (i + 1) * (DiskHeight + 3);
                Canvas.SetLeft(diskUI[d], x);
                Canvas.SetTop(diskUI[d], y);
                Canvas.SetLeft(labelUI[d], x);
                Canvas.SetTop(labelUI[d], y + 2);
            }
        }

        // --- Reset UI khi Run ---
        async void Run_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(DiskCountTextBox.Text, out int n) || n <= 0)
            {
                MessageBox.Show("Nhập số đĩa hợp lệ (>0)");
                return;
            }

            totalDisks = n;

            // --- Reset UI & tower & logic ---
            MainCanvas.Children.Clear();
            tower = new DT.Tower();
            diskUI.Clear();
            labelUI.Clear();
            logic = new HanoiLogic();
            isPaused = false;
            stepMode = false;
            nextStep = false;

            // --- Vẽ cọc + đế ---
            DrawPegs();

            // --- Khởi tạo đĩa ---
            InitDisksUI(n);

            // --- Vẽ stack ban đầu ---
            DrawStackUI(tower.A, 0);
            DrawStackUI(tower.B, 1);
            DrawStackUI(tower.C, 2);

            // --- Tính moves theo thuật toán ---
            string algo = (AlgorithmComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Recursive";
            switch (algo)
            {
                case "Recursive":
                    logic.SolveRecursive(n, 0, 2, 1);
                    break;
                case "Iterative":
                    logic.SolveIterative(n);
                    break;
                case "NonRecursive":
                    logic.SolveNonRecursive(n);
                    break;
            }

            // --- Chạy animation ---
            await AnimateMovesOptimized();
        }

        // --- Lấy stack theo chỉ số ---
        DT.MyStack GetStack(int index)
        {
            return index switch
            {
                0 => tower.A,
                1 => tower.B,
                _ => tower.C
            };
        }
        // --- Chạy animation ---
        // --- AnimateMoves tối ưu ---
        async Task AnimateMovesOptimized()
        {
            Stack<HanoiLogic.MoveDisk> temp = new();
            while (!logic.Moves.IsEmpty())
                temp.Push(logic.Moves.Pop()!);

            while (temp.Count > 0)
            {
                while (isPaused && !stepMode) await Task.Delay(50);
                if (stepMode && !nextStep) { await Task.Delay(50); continue; }
                nextStep = false;

                var move = temp.Pop();
                var from = GetStack(move.From);
                var to = GetStack(move.To);

                int fromCount = from.Count();
                int toCount = to.Count();

                var disk = from.Pop();
                if (disk == null) continue;

                var movingRect = diskUI[disk];
                var movingLabel = labelUI[disk];

                double fromX = 150 + move.From * 250;
                double toX = 150 + move.To * 250;
                double fromY = BaseY - fromCount * (DiskHeight + 3);
                double toY = BaseY - (toCount + 1) * (DiskHeight + 3);

                var originalBrush = movingRect.Fill;
                movingRect.Fill = Brushes.Red;

                int frames = 25;
                double lift = 80;

                // nhấc lên
                for (int i = 0; i < frames; i++)
                {
                    double y = fromY - (lift * i / frames);
                    Canvas.SetTop(movingRect, y);
                    Canvas.SetTop(movingLabel, y + 2);
                    await Task.Delay(10);
                }

                // đi ngang
                for (int i = 0; i < frames; i++)
                {
                    double x = fromX + (toX - fromX) * i / frames;
                    Canvas.SetLeft(movingRect, x - movingRect.Width / 2);
                    Canvas.SetLeft(movingLabel, x - movingRect.Width / 2);
                    await Task.Delay(10);
                }

                // hạ xuống
                for (int i = 0; i < frames; i++)
                {
                    double y = fromY - lift + (toY - (fromY - lift)) * i / frames;
                    Canvas.SetTop(movingRect, y);
                    Canvas.SetTop(movingLabel, y + 2);
                    await Task.Delay(10);
                }

                movingRect.Fill = originalBrush;

                to.Push(disk);
                DrawStackUI(from, move.From);
                DrawStackUI(to, move.To);

                await Task.Delay(150);
            }
        }

        void Pause_Click(object sender, RoutedEventArgs e) => isPaused = true;
        void Step_Click(object sender, RoutedEventArgs e)
        {
            stepMode = true;
            nextStep = true;
        }
        void Resume_Click(object sender, RoutedEventArgs e)
        {
            isPaused = false;
            stepMode = false;
            nextStep = false;
        }
    }
}
            
