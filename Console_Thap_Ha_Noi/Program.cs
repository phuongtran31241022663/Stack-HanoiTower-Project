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
        private MyStack<Move> history;
        public HanoiLogic(MyStack<Move> history) => this.history = history;
        public void MoveBetween(Tower tower, int src, int dest)
        {
            var sPeg = tower.Pegs[src];
            var dPeg = tower.Pegs[dest];

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
                history.Push(new Move(fIdx, tIdx, d.Size));
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

    // Đo thời gian
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
        public static TimeSpan MeasureTime(string taskName, Action action)
        {
            Timing t = new Timing();
            t.startTime();
            action();
            t.StopTime();

            return t.Result();
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
            int loops = 500000;
            TimeSpan Time;
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
                Console.WriteLine("7. Quick Sort Iterative");
                Console.WriteLine("0. Thoát");
                Console.Write("--> Chọn chức năng (0 đến 7): ");

                string choice = Console.ReadLine() ?? "";
                try
                {
                    switch (choice)
                    {
                        case "1": // Hanoi
                            Console.Write("Nhập số đĩa n: ");
                            if (!int.TryParse(Console.ReadLine(), out int n)) { Console.WriteLine("Số đĩa không hợp lệ!"); break; }

                            int loop;
                            if (n <= 10) loop = 500000;
                            else if (n <= 15) loop = 1000;
                            else loop = 1;

                            Time = PerformanceTester.MeasureTime("Hanoi", () =>
                            {
                                for (int i = 0; i < loop; i++)
                                {
                                    MyStack<Move> hist = new MyStack<Move>();
                                    Tower t = new Tower();
                                    for (int d = n; d >= 1; d--) t.CreateDisk(d, 0);
                                    HanoiLogic l = new HanoiLogic(hist);
                                    l.SolveNonRecursive(n, t);
                                }
                            });

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"\n-> Thời gian: {Time.TotalMilliseconds / loop:F6} ms");
                            Console.ResetColor();

                            if (n <= 10)
                            {
                                MyStack<Move> hist = new MyStack<Move>();
                                Tower t = new Tower();
                                for (int d = n; d >= 1; d--) t.CreateDisk(d, 0);
                                HanoiLogic l = new HanoiLogic(hist);
                                l.SolveNonRecursive(n, t);

                                Console.WriteLine("\n--- Các bước giải ---");
                                var moves = hist.ToArray();
                                Array.Reverse(moves);
                                string[] pole = { "A", "B", "C" };
                                int step = 0;
                                foreach (var m in moves)
                                {
                                    step++;
                                    Console.WriteLine($"Bước {step}: Chuyển đĩa {m.DiskSize} từ {pole[m.From]} sang {pole[m.To]}");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"(Ẩn bước giải do số lượng quá lớn)");
                            }
                            break;
                        case "2": // SORT STACK
                            Console.Write("Nhập các số nguyên muốn sắp xếp, cách nhau bởi khoảng trắng (VD: 5 2 9 1): ");
                            string input2 = Console.ReadLine() ?? "";
                            if (!string.IsNullOrWhiteSpace(input2))
                            {
                                int[] arr2 = Array.ConvertAll(input2.Split(' ', StringSplitOptions.RemoveEmptyEntries), int.Parse);
                                MyStack<int> s2 = new MyStack<int>();

                                Time = PerformanceTester.MeasureTime("Sort Stack", () =>
                                {
                                    for (int i = 0; i < loops; i++)
                                    {
                                        s2 = new MyStack<int>();
                                        foreach (var x in arr2) s2.Push(x);
                                        AlgorithmSet.SortStack(s2);
                                    }
                                });
                                MyStack<int> sorted = new MyStack<int>();
                                foreach (var x in arr2) sorted.Push(x);
                                sorted = AlgorithmSet.SortStack(sorted);
                                var sortedArr = sorted.ToArray();
                                Array.Reverse(sortedArr);
                                Console.WriteLine("Kết quả sắp xếp: " + string.Join(" ", sortedArr));
                                Console.WriteLine($"Thời gian: {Time.TotalMilliseconds / loops:F6} ms");
                            }
                            else
                            {
                                Console.WriteLine("Không được để trống!");
                            }
                            break;
                        case "3": // REVERSE
                            Console.Write("Nhập chuỗi bất kì: ");
                            string input3 = Console.ReadLine() ?? "";
                            string rev = AlgorithmSet.ReverseString(input3);
                            Console.WriteLine($"Chuỗi đảo: {rev}");
                            Time = PerformanceTester.MeasureTime("Reverse String", () =>
                            {
                                for (int i = 0; i < loops; i++)
                                    AlgorithmSet.ReverseString(input3);
                            });
                            Console.WriteLine($"Thời gian: {Time.TotalMilliseconds/loops:F6} ms");
                            break;

                        case "4": // BINARY
                            Console.Write("Nhập số nguyên: ");
                            if (int.TryParse(Console.ReadLine(), out int nb))
                            Console.WriteLine($"Kết quả nhị phân: {AlgorithmSet.DecimalToBinary(nb)}");
                            {
                                Time = PerformanceTester.MeasureTime("Decimal to Binary", () =>
                            {
                                for (int i = 0; i < loops; i++)
                                    AlgorithmSet.DecimalToBinary(nb);
                            });
                                Console.WriteLine($"Thời gian: {Time.TotalMilliseconds / loops:F6} ms");
                            }
                            break;

                        case "5": // PARENTHESES
                            Console.Write("Nhập biểu thức ngoặc (VD: {[]} ): ");
                            string input5 = Console.ReadLine() ?? "";
                            Time = PerformanceTester.MeasureTime($"Check Parentheses", () =>
                            {
                                for (int i = 0; i < loops; i++)
                                    AlgorithmSet.IsValidParentheses(input5);
                            });
                            Console.WriteLine($"Thời gian: {Time.TotalMilliseconds / loops:F6} ms");
                            bool ok = AlgorithmSet.IsValidParentheses(input5);
                            Console.WriteLine($"Hợp lệ: {(ok ? "Đúng" : "Sai")}");
                            break;

                        case "6": // POSTFIX
                            Console.WriteLine("Nhập biểu thức hậu tố (VD: 10 5 + 3 *):");
                            string post = Console.ReadLine() ?? "";

                            if (string.IsNullOrWhiteSpace(post))
                            {
                                Console.WriteLine("Biểu thức trống, vui lòng nhập lại!");
                                break;
                            }

                            Time = PerformanceTester.MeasureTime("Evaluate Postfix", () =>
                            {
                                for (int i = 0; i < loops; i++)
                                    AlgorithmSet.EvaluatePostfix(post);
                            });

                            int result = 0;
                            try
                            {
                                result = AlgorithmSet.EvaluatePostfix(post);
                                Console.WriteLine($"Kết quả: {result}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Lỗi khi tính biểu thức: {ex.Message}");
                            }

                            Console.WriteLine($"Thời gian: {Time.TotalMilliseconds / loops:F6} ms");
                            break;
                        case "7": // QUICKSORT
                            Console.Write("Nhập mảng số nguyên cần sắp xếp, cách nhau bởi khoảng trắng (VD: 10 7 8 9 1 5): ");
                            string inputArr = Console.ReadLine() ?? "";
                            if (!string.IsNullOrWhiteSpace(inputArr))
                            {
                                int[] arr = Array.ConvertAll(inputArr.Split(' ', StringSplitOptions.RemoveEmptyEntries), int.Parse);
                                int[] copy = (int[])arr.Clone();
                                Time = PerformanceTester.MeasureTime("QuickSort Iterative", () =>
                                {
                                    for (int i = 0; i < loops; i++)
                                    {
                                        int[] temp = (int[])arr.Clone();
                                        AlgorithmSet.QuickSortIterative(temp);
                                        if (i == loops - 1)
                                            copy = temp;
                                    }
                                });
                                Console.WriteLine("Sau sắp xếp: " + string.Join(" ", copy));
                                Console.WriteLine($"Thời gian: {Time.TotalMilliseconds / loops:F6} ms");
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