using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using WpfApp1.Hanoi;

namespace WpfApp1
{
        public class Timing
        {
            TimeSpan startingTime;
            TimeSpan duration;
            public Timing()
            {
                startingTime = new TimeSpan(0);
                duration = new TimeSpan(0);
            }
            public void StopTime()
            {
                duration =
                Process.GetCurrentProcess().Threads[0].
                UserProcessorTime.
                Subtract(startingTime);
            }
            public void startTime()
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                startingTime =
                Process.GetCurrentProcess().Threads[0].
                UserProcessorTime;
            }
            public TimeSpan Result()
            {
                return duration;
            }
        }
    public static class TimingTest
    {
        public static void RunTests(int numDisks)
        {
            var timer = new Timing();
            var moveHistory = new DataStructures.MyStack<DataStructures.Move>();
            var hanoi = new HanoiLogic(moveHistory);

            int iterations = 3; // Số lần chạy để lấy trung bình
            double totalRec = 0;
            double totalNonRec = 0;

            for (int i = 1; i <= iterations; i++)
            {
                // --- 1. CHUẨN BỊ DỮ LIỆU ---
                Tower towerForRec = new Tower();
                for (int d = numDisks; d >= 1; d--)
                    towerForRec.Get(0).PushDisk(new DataStructures.Disk(d));

                Tower towerForNonRec = hanoi.CopyTower(towerForRec);

                // --- 2. ĐO ĐỆ QUY ---
                timer.startTime();
                hanoi.SolveRecursive(numDisks, 0, 2, 1, towerForRec);
                timer.StopTime();
                totalRec += timer.Result().TotalMilliseconds;

                // --- 3. ĐO PHI ĐỆ QUY ---
                timer.startTime();
                hanoi.SolveNonRecursive(numDisks, towerForNonRec);
                timer.StopTime();
                totalNonRec += timer.Result().TotalMilliseconds;
            }

            // --- 4. TÍNH TRUNG BÌNH ---
            double avgRec = totalRec / iterations;
            double avgNonRec = totalNonRec / iterations;

            string report = $"KẾT QUẢ TRUNG BÌNH QUA {iterations} LẦN CHẠY ({numDisks} ĐĨA):\n\n" +
                            $"- Đệ quy (Avg): {avgRec:F2} ms\n" +
                            $"- Lặp (Avg): {avgNonRec:F2} ms\n\n" +
                            $"Chênh lệch trung bình: {Math.Abs(avgRec - avgNonRec):F2} ms";

            MessageBox.Show(report, "Báo cáo hiệu năng trung bình");
        }
    }
}
