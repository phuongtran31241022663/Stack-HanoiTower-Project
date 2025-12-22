using System;
using System.Diagnostics;

namespace ThapHanoiProject
{
    // Node
    public class Node<T>
    {
        public T Data;
        public Node<T>? Next;
        public Node(T data) { Data = data; Next = null; }
    }

    // Stack THỦ CÔNG
    public class MyStack<T>
    {
        public Node<T>? Top;
        private int count = 0;

        public bool IsEmpty() => Top == null;
        public int Size() => count;

        public void Push(T item)
        {
            var node = new Node<T>(item);
            node.Next = Top;
            Top = node;
            count++;
        }

        public T Pop()
        {
            if (IsEmpty()) return default(T)!;

            var val = Top!.Data;
            Top = Top.Next;
            count--;
            return val;
        }

        public T Peek()
        {
            if (IsEmpty()) return default(T)!;
            return Top!.Data;
        }

        public T[] ToArray()
        {
            if (count == 0) return new T[0];
            T[] arr = new T[count];
            Node<T>? curr = Top;
            int i = 0;
            while (curr != null)
            {
                arr[i++] = curr.Data;
                curr = curr.Next;
            }
            return arr;
        }
    }

    // 2. LOGIC THÁP HÀ NỘI
    public class Disk { public int Size; public Disk(int s) => Size = s; }
    public class Move { public int From, To, DiskSize; public Move(int f, int t, int d) { From = f; To = t; DiskSize = d; } }

    public class Tower
    {
        public MyStack<Disk>[] Pegs = { new MyStack<Disk>(), new MyStack<Disk>(), new MyStack<Disk>() };
        public void CreateDisk(int size, int pegIndex)
        {
            Pegs[pegIndex].Push(new Disk(size));
        }
    }

    public class HanoiLogic
    {
        private MyStack<Move> moveHistory;

        public HanoiLogic(MyStack<Move> moveHistory)
        {
            this.moveHistory = moveHistory;
        }

        public void MoveBetween(Tower tower, int src, int dest)
        {
            var sPeg = tower.Pegs[src];
            var dPeg = tower.Pegs[dest];

            // Logic đĩa nhỏ đè đĩa lớn
            int sSize = sPeg.IsEmpty() ? int.MaxValue : sPeg.Peek().Size;
            int dSize = dPeg.IsEmpty() ? int.MaxValue : dPeg.Peek().Size;

            if (sPeg.IsEmpty() || (!dPeg.IsEmpty() && sSize > dSize))
            {
                MoveDisk(dPeg, sPeg, dest, src);
            }
            else
            {
                MoveDisk(sPeg, dPeg, src, dest);
            }
        }

        private void MoveDisk(MyStack<Disk> from, MyStack<Disk> to, int fIdx, int tIdx)
        {
            var d = from.Pop();
            if (d != null)
            {
                to.Push(d);
                moveHistory.Push(new Move(fIdx, tIdx, d.Size));
            }
        }

        public void SolveNonRecursive(int n, Tower tower)
        {
            int src = 0, aux = 1, dest = 2;
            if (n % 2 == 0) (aux, dest) = (dest, aux);

            int totalMoves = (1 << n) - 1;

            for (int i = 1; i <= totalMoves; i++)
            {
                if (i % 3 == 1) MoveBetween(tower, src, dest);
                else if (i % 3 == 2) MoveBetween(tower, src, aux);
                else MoveBetween(tower, aux, dest);
            }
        }
    }

    // ĐO THOI GIAN VÀ BỘ NHỚ
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
            duration = Process.GetCurrentProcess().Threads[0].UserProcessorTime.Subtract(startingTime);
        }


        public void startTime()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            startingTime = Process.GetCurrentProcess().Threads[0].UserProcessorTime;
        }


        public TimeSpan Result()
        {
            return duration;
        }
    }


    public static class PerformanceTester
    {
        public static void MeasureTime(string taskName, Action action)
        {
            Timing t = new Timing();
            t.startTime();


            action();


            t.StopTime();


            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"-> [THOI GIAN] {taskName}: {t.Result().TotalMilliseconds:F4} ms");
            Console.ResetColor();
        }


        public static void MeasureMemoryAndRun(string taskName, Action action)
        {
            long startMem = GC.GetTotalMemory(true);


            Timing t = new Timing();
            t.startTime();


            action();


            t.StopTime();


            long endMem = GC.GetTotalMemory(false);
            long memUsed = Math.Max(0, endMem - startMem);


            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"-> [THOI GIAN] {taskName}: {t.Result().TotalMilliseconds:F4} ms");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"-> [BO NHO]    {taskName}: ~{memUsed} bytes");
            Console.ResetColor();
        }
    }

    // 6 THUẬT TOÁN BỔ SUNG
    public static class AlgorithmSet
    {
        // 1. Sort Stack
        public static MyStack<int> SortStack(MyStack<int> input)
        {
            MyStack<int> tmpStack = new MyStack<int>();
            while (!input.IsEmpty())
            {
                int tmp = input.Pop();
                while (!tmpStack.IsEmpty() && tmpStack.Peek() > tmp)
                {
                    input.Push(tmpStack.Pop());
                }
                tmpStack.Push(tmp);
            }
            return tmpStack;
        }

        // 2. Reverse String
        public static string ReverseString(string s)
        {
            MyStack<int> sk = new MyStack<int>();
            foreach (char c in s) sk.Push((int)c);
            string res = "";
            while (!sk.IsEmpty()) res += (char)sk.Pop();
            return res;
        }

        // 3. Decimal to Binary
        public static string DecimalToBinary(int n)
        {
            if (n == 0) return "0";
            MyStack<int> sk = new MyStack<int>();
            while (n > 0) { sk.Push(n % 2); n /= 2; }
            string res = "";
            while (!sk.IsEmpty()) res += sk.Pop().ToString();
            return res;
        }

        // 4. Parentheses
        public static bool IsValidParentheses(string s)
        {
            MyStack<int> sk = new MyStack<int>();
            foreach (char c in s)
            {
                if ("([{".Contains(c)) sk.Push(c);
                else
                {
                    if (sk.IsEmpty()) return false;
                    char open = (char)sk.Pop();
                    if ((c == ')' && open != '(') ||
                        (c == ']' && open != '[') ||
                        (c == '}' && open != '{')) return false;
                }
            }
            return sk.IsEmpty();
        }

        // 5. Postfix
        public static int EvaluatePostfix(string exp)
        {
            MyStack<int> sk = new MyStack<int>();
            string buffer = "";
            exp += " ";

            for (int i = 0; i < exp.Length; i++)
            {
                char c = exp[i];
                if (char.IsDigit(c)) buffer += c;
                else if (c == ' ')
                {
                    if (buffer.Length > 0)
                    {
                        sk.Push(int.Parse(buffer));
                        buffer = "";
                    }
                }
                else
                {
                    if (sk.Size() < 2) return 0;
                    // [SỬA LỖI]: Bỏ ?? 0
                    int b = sk.Pop();
                    int a = sk.Pop();
                    if (c == '+') sk.Push(a + b);
                    else if (c == '-') sk.Push(a - b);
                    else if (c == '*') sk.Push(a * b);
                    else if (c == '/') sk.Push(a / b);
                }
            }
            return sk.Pop();
        }

        // 6. QuickSort
        public static void QuickSortIterative(int[] arr)
        {
            MyStack<int> s = new MyStack<int>();
            s.Push(0);
            s.Push(arr.Length - 1);

            while (!s.IsEmpty())
            {
                // [SỬA LỖI]: Bỏ ?? 0
                int end = s.Pop();
                int start = s.Pop();
                if (end - start < 1) continue;

                int pivot = arr[end];
                int i = start - 1;

                for (int j = start; j < end; j++)
                {
                    if (arr[j] <= pivot)
                    {
                        i++;
                        int temp = arr[i]; arr[i] = arr[j]; arr[j] = temp;
                    }
                }
                int t2 = arr[i + 1]; arr[i + 1] = arr[end]; arr[end] = t2;
                int pIndex = i + 1;

                if (pIndex - 1 > start) { s.Push(start); s.Push(pIndex - 1); }
                if (pIndex + 1 < end) { s.Push(pIndex + 1); s.Push(end); }
            }
        }

        public static void PrintStack(MyStack<int> s, string label)
        {
            Console.Write($"{label}: ");
            var arr = s.ToArray();
            foreach (var item in arr) Console.Write(item + " ");
            Console.WriteLine();
        }
    }

    // Main
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("=== ĐỒ ÁN CẤU TRÚC DỮ LIỆU VÀ GIẢI THUẬT ===");
                Console.ResetColor();
                Console.WriteLine("\n--- BÀI TOÁN THÁP HÀ NỘI ---");
                Console.WriteLine("1. Giải Tháp Hà Nội");
                Console.WriteLine("\n--- THUẬT TOÁN BỔ SUNG ---");
                Console.WriteLine("2. Sort Stack");
                Console.WriteLine("3. Reverse String");
                Console.WriteLine("4. Decimal to Binary");
                Console.WriteLine("5. Parentheses");
                Console.WriteLine("6. Postfix");
                Console.WriteLine("7. Quick Sort");
                Console.WriteLine("0. Thoát");
                Console.Write("--> Chọn chức năng (0 đến 7): ");

                string choice = Console.ReadLine() ?? "";
                try
                {
                    switch (choice)
                    {
                        case "1": // HANOI
                            Console.Write("Nhập số đĩa n: ");
                            if (int.TryParse(Console.ReadLine(), out int n))
                            {
                                // Giữ nguyên string không dấu trong code của bạn
                                PerformanceTester.MeasureMemoryAndRun($"Giai Thap Ha Noi {n} dia", () =>
                                {
                                    Tower t = new Tower();
                                    for (int i = n; i >= 1; i--) t.CreateDisk(i, 0);
                                    MyStack<Move> history = new MyStack<Move>();
                                    HanoiLogic logic = new HanoiLogic(history);

                                    logic.SolveNonRecursive(n, t);

                                    //dsach buoc giai
                                    if (n <= 10)
                                    {
                                        Console.WriteLine("\n--- CÁC BƯỚC GIẢI ---");
                                        var moves = history.ToArray();
                                        Array.Reverse(moves);

                                        string[] poleNames = { "A", "B", "C" };
                                        int stepCount = 0;

                                        foreach (var m in moves)
                                        {
                                            stepCount++;
                                            string from = poleNames[m.From];
                                            string to = poleNames[m.To];
                                            Console.WriteLine($"Bước {stepCount}: Chuyển đĩa {m.DiskSize} từ {from} sang {to}");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine($"\n(Đã ẩn chi tiết {history.Size()} bước giải vì số lượng quá lớn)");
                                    }
                                });
                            }
                            else Console.WriteLine("Số đĩa không hợp lệ!");
                            break;

                        case "2": // SORT STACK
                            Console.Write("Nhập các số nguyên muốn sắp xếp, cách nhau bởi khoảng trắng (VD: 5 2 9 1): ");
                            string input2 = Console.ReadLine() ?? "";
                            if (!string.IsNullOrWhiteSpace(input2))
                            {
                                string[] p2 = input2.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                MyStack<int> s2 = new MyStack<int>();
                                foreach (var x in p2) if (int.TryParse(x, out int v)) s2.Push(v);

                                AlgorithmSet.PrintStack(s2, "Input");
                                PerformanceTester.MeasureTime("Sort Stack", () =>
                                {
                                    var res = AlgorithmSet.SortStack(s2);
                                    AlgorithmSet.PrintStack(res, "Sorted");
                                });
                            }
                            break;

                        case "3": // REVERSE
                            Console.Write("Nhập chuỗi bất kì: ");
                            string input3 = Console.ReadLine() ?? "";
                            PerformanceTester.MeasureTime("Reverse", () =>
                                Console.WriteLine(AlgorithmSet.ReverseString(input3)));
                            break;

                        case "4": // BINARY
                            Console.Write("Nhập số nguyên: ");
                            if (int.TryParse(Console.ReadLine(), out int nb))
                                PerformanceTester.MeasureTime("Binary", () =>
                                    Console.WriteLine("Số nhị phân: " + AlgorithmSet.DecimalToBinary(nb)));
                            break;

                        case "5": // PARENTHESES
                            Console.Write("Nhập biểu thức ngoặc (VD: {[]} ): ");
                            string input5 = Console.ReadLine() ?? "";
                            PerformanceTester.MeasureTime("Check Parentheses", () =>
                                // [FIXED] Sửa lỗi cú pháp: Thêm ngoặc bao quanh biểu thức tam phân
                                Console.WriteLine("Kết quả: " + (AlgorithmSet.IsValidParentheses(input5) ? "Hợp lệ" : "Không hợp lệ")));
                            break;

                        case "6": // POSTFIX
                            Console.WriteLine("Nhập hậu tố (VD: 10 5 + 3 *):");
                            string post = Console.ReadLine() ?? "";
                            PerformanceTester.MeasureTime("Eval Postfix", () =>
                                Console.WriteLine("Kết quả: " + AlgorithmSet.EvaluatePostfix(post)));
                            break;

                        case "7": // QUICKSORT
                            Console.Write("Nhập mảng số nguyên cần sắp xếp, cách nhau bởi khoảng trắng (VD: 10 7 8 9 1 5): ");
                            string inputArr = Console.ReadLine() ?? "";
                            if (!string.IsNullOrWhiteSpace(inputArr))
                            {
                                string[] p7 = inputArr.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                int[] arr = new int[p7.Length];
                                for (int i = 0; i < p7.Length; i++) int.TryParse(p7[i], out arr[i]);

                                Console.WriteLine("Mảng ban đầu: " + string.Join(" ", arr));
                                Console.WriteLine("Mảng sau khi sắp xếp: " + string.Join(" ", arr));
                                PerformanceTester.MeasureTime("QuickSort Iterative", () => AlgorithmSet.QuickSortIterative(arr));
                            }
                            break;

                        case "0": return;
                    }
                }
                catch (Exception ex) { Console.WriteLine("LỖI: " + ex.Message); }
                Console.WriteLine("\nẤn Enter để tiếp tục...");
                Console.ReadLine();
            }
        }
    }
}