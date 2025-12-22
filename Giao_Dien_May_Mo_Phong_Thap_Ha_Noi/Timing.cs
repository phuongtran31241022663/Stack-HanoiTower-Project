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

            // Tăng số lần lặp nội bộ để kéo dài thời gian thực thi
            int innerLoops = (numDisks < 10) ? 5000 : 100;
            int iterations = 7;
            double totalRec = 0;
            double totalNonRec = 0;

            for (int i = 1; i <= iterations; i++)
            {
                // --- ĐO ĐỆ QUY ---
                timer.startTime();
                for (int j = 0; j < innerLoops; j++)
                {
                    Tower t = new Tower(); // Khởi tạo nhẹ
                    hanoi.SolveRecursive(numDisks, 0, 2, 1, t);
                }
                timer.StopTime();
                // Chia cho innerLoops để ra thời gian thực của 1 lần giải
                totalRec += (timer.Result().TotalMilliseconds / innerLoops);

                // --- ĐO PHI ĐỆ QUY ---
                timer.startTime();
                for (int j = 0; j < innerLoops; j++)
                {
                    Tower t = new Tower();
                    hanoi.SolveNonRecursive(numDisks, t);
                }
                timer.StopTime();
                totalNonRec += (timer.Result().TotalMilliseconds / innerLoops);
            }

            double avgRec = totalRec / iterations;
            double avgNonRec = totalNonRec / iterations;

            // Dùng định dạng F6 để thấy các con số cực nhỏ (micro-seconds)
            string report = $"KẾT QUẢ TRUNG BÌNH ({numDisks} ĐĨA - Lặp {innerLoops} lần):\n\n" +
                            $"- Đệ quy: {avgRec:F6} ms\n" +
                            $"- Lặp: {avgNonRec:F6} ms\n\n" +
                            "Ghi chú: Đã dùng kỹ thuật lặp để bù đắp độ phân giải CPU.";

            MessageBox.Show(report, "Báo cáo hiệu năng");
        }
    }
}
