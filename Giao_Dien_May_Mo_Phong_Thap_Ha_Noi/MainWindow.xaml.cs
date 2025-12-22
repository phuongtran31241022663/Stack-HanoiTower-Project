// MainWindow.xaml.cs
// ------------------------------------------------------------
// 1. Trường dữ liệu: Tower, Logic, History, Visualizer, Token.
// 2. Khởi tạo: Constructor và InitCore setup hệ thống ban đầu.
// 3. Sự kiện chính: Create (Khởi tạo đĩa), Start (Giải & Diễn họa), Stop.
// 4. Diễn họa: Pause, Resume, Speed Control (Slider).
// 5. Hệ thống: Thống kê, Khóa điều khiển, Hủy tiến trình.
// ------------------------------------------------------------
using System.Diagnostics;
using System.Windows;
using WpfApp1.Hanoi;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        #region 1. Trường dữ liệu & Trạng thái
        private Tower tower;
        private HanoiLogic logic;
        private DataStructures.MyStack<DataStructures.Move> moveHistory;
        private Visualizer visualizer;

        // CancellationTokenSource để hủy Task diễn hoạt khi nhấn Stop
        private CancellationTokenSource cts;
        #endregion

        #region 2. Khởi tạo
        public MainWindow()
        {
            InitializeComponent();
            // Khởi tạo visualizer với tọa độ 3 cọc A, B, C
            visualizer = new Visualizer(MainCanvas, StepText, new double[] { 180, 450, 720 }, 480, 22);
            InitCore();
        }
        private void InitCore()
        {
            moveHistory = new DataStructures.MyStack<DataStructures.Move>();
            logic = new HanoiLogic(moveHistory);
            tower = new Tower();
        }
        #endregion

        #region 3. Xử lý sự kiện chính
        // Tạo bài toán mới
        public void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            StopCurrentProcess(); // Dừng mọi hoạt động đang diễn ra

            if (!int.TryParse(TxtDisk.Text, out int n) || n < 1 || n > 10)
            {
                MessageBox.Show("Vui lòng nhập số đĩa từ 1 đến 10.");
                return;
            }

            InitCore();

            // Tạo dữ liệu đĩa cho cọc logic (Cọc 0)
            for (int i = n; i >= 1; i--) tower.CreateDisk(i, 0);

            // Vẽ đĩa lên Canvas
            visualizer.Reset();
            visualizer.RegisterInitialDisks(n);

            LockControls(false);
            if (StepText != null) StepText.Text = "Đã khởi tạo tháp với " + n + " đĩa.";
        }
        // Bắt đầu giải và diễn họa
        public async void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(TxtDisk.Text, out int n) || n < 1) return;

            // Dọn dẹp tiến trình cũ
            StopCurrentProcess(); // Dừng animation cũ
            visualizer.Reset();   // Xóa canvas
            InitCore();           // Reset stack logic

            // Khởi tạo lại đĩa ở trạng thái ban đầu (Cọc A)
            for (int i = n; i >= 1; i--) tower.CreateDisk(i, 0);
            visualizer.RegisterInitialDisks(n);

            try
            {
                LockControls(true);
                cts = new CancellationTokenSource();

                // A. Chạy thuật toán
                await Task.Run(() => {
                    int algoIndex = 0;
                    Dispatcher.Invoke(() => algoIndex = CbAlgorithm.SelectedIndex);

                    // Xóa lịch sử cũ trước khi giải
                    moveHistory = new DataStructures.MyStack<DataStructures.Move>();
                    logic = new HanoiLogic(moveHistory);

                    if (algoIndex == 0)
                        logic.SolveRecursive(n, 0, 2, 1, tower);
                    else
                        logic.SolveNonRecursive(n, tower);
                });

                // B. Thực hiện diễn họa (Chạy tuần tự theo moveHistory)
                if (moveHistory != null && !moveHistory.IsEmpty())
                {
                    visualizer.Resume();
                    await visualizer.PlayFromHistory(moveHistory);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Lỗi: " + ex.Message);
            }
            finally
            {
                LockControls(false);
            }
        }
        // Dừng tiến trình
        public void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            StopCurrentProcess();
            if (StepText != null) StepText.Text = "Đã dừng tiến trình.";
        }
        #endregion

        #region 4. Điều khiển Diễn họa (Animation Controls)
        public void BtnPause_Click(object sender, RoutedEventArgs e) => visualizer.Pause();

        public void BtnResume_Click(object sender, RoutedEventArgs e) => visualizer.Resume();

        private void DelaySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Cập nhật tốc độ diễn hoạt khi kéo thanh slider
            visualizer?.SetDelay((int)e.NewValue);
        }
        #endregion

        #region 5. Thống kê & Hỗ trợ nội bộ
        public void BtnStats_Click(object sender, RoutedEventArgs e)
        {
            if (logic == null || moveHistory == null || moveHistory.IsEmpty())
            {
                MessageBox.Show("Không có dữ liệu thống kê. Vui lòng chạy thuật toán trước.");
                return;
            }

            // Lấy dữ liệu thống kê từ Logic (Sử dụng MyStack tự chế)
            var stats = logic.GetMoveStats();
            string msg = "BẢNG THỐNG KÊ CHI TIẾT:\n";
            msg += "----------------------------\n";

            while (!stats.IsEmpty())
            {
                var s = stats.Pop();
                msg += $"• Đĩa cỡ {s.Size}: Di chuyển {s.Count} lần\n";
            }
            MessageBox.Show(msg, "Thống kê di chuyển");
        }
        private void StopCurrentProcess()
        {
            cts?.Cancel();
            visualizer.Stop();
            LockControls(false);
        }
        private void LockControls(bool running)
        {
            BtnCreate.IsEnabled = !running;
            BtnStart.IsEnabled = !running;
            CbAlgorithm.IsEnabled = !running;
            TxtDisk.IsEnabled = !running;

            BtnPause.IsEnabled = running;
            BtnResume.IsEnabled = running;
            BtnStop.IsEnabled = running;
        }
        #endregion
    }
}