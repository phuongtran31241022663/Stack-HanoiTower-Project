// Hanoi.cs
// ------------------------------------------------------------
// Có thể bỏ sau dấu :
// 1. DataStructures
//    - Node<T>        : Nút Linked List, nền tảng cài đặt Stack.
//    - MyStack<T>     : Stack tự cài đặt (Push, Pop, Peek, Size, ToArray).
//                       Không dùng thư viện có sẵn, đáp ứng yêu cầu đề bài.
//    - Disk           : Mô hình đĩa, xác định bằng kích thước (Size).
//    - Move           : Biểu diễn một bước di chuyển (From, To, DiskSize).
//    - DiskStat       : Phục vụ thống kê số lần di chuyển của từng đĩa.
//
// 2. HanoiStack
//    - Kế thừa MyStack<Disk>.
//    - Áp đặt luật Tháp Hà Nội: Không cho phép đĩa lớn đặt lên đĩa nhỏ.
//    - Cung cấp truy vấn tầng (level) của đĩa trong cọc.
//
// 3. Tower
//    - Quản lý trạng thái bài toán.
//    - Gồm 3 cọc (peg 0, 1, 2), mỗi cọc là một HanoiStack.
//    - Chịu trách nhiệm khởi tạo và truy cập các cọc.
//
// 4. HanoiLogic
//    - Chứa toàn bộ giải thuật xử lý.
//    - Quản lý lịch sử di chuyển bằng MyStack<Move>.
//    - Bao gồm:
//        + Di chuyển hợp lệ giữa 2 cọc (MoveBetween).
//        + Giải bài toán:
//            * Đệ quy (SolveRecursive).
//            * Phi đệ quy (SolveNonRecursive).
//        + Thuật toán bổ sung:
//            * GetMoveStats   : thống kê số lần di chuyển.
//            * SolveFromArbitraryState : Giải bài toán từ trạng thái bất kỳ.
// ------------------------------------------------------------
using static WpfApp1.Hanoi.DataStructures;

namespace WpfApp1.Hanoi
{
    #region 1. Cấu trúc dữ liệu
    public static class DataStructures
    {
        // Node
        public class Node<T>
        {
            public T Data;
            public Node<T>? Next;
            public Node(T data) { Data = data; Next = null; }
        }
        // Stack
        public class MyStack<T>
        {
            public Node<T>? Top;
            public int count = 0; // Biến này để quản lý số lượng phần tử
            public bool IsEmpty() => Top == null;
            public int Size() => count; // Thay cho hàm Count() chạy vòng lặp
            public void Push(T item)
            {
                var node = new Node<T>(item);
                node.Next = Top;
                Top = node;
                count++;
            }
            public T? Pop()
            {
                if (IsEmpty()) return default;
                var val = Top.Data;
                Top = Top.Next;
                count--;
                return val;
            }
            public T? Peek()
            {
                return Top == null ? default : Top.Data;
            }
            // Chuyển đổi Stack thành mảng để phục vụ vẽ UI (từ đỉnh xuống đáy).
            public T[] ToArray()
            {
                T[] elements = new T[count];
                Node<T>? current = Top;
                int index = 0;

                while (current != null)
                {
                    elements[index] = current.Data;
                    current = current.Next;
                    index++;
                }
                return elements;
            }
        }
        // Model
        public class Disk { public int Size; public Disk(int size) { Size = size; } }
        // Lưu trữ thông tin một bước di chuyển giữa các cọc.
        public class Move { public int From, To, DiskSize; public Move(int f, int t, int d) { From = f; To = t; DiskSize = d; } }
        // Đối tượng phục vụ mục đích thống kê số lần di chuyển của từng đĩa.
        public class DiskStat { public int Size; public int Count; public DiskStat(int size) { Size = size; Count = 1; } }
    }
    #endregion

    #region 2. Tower of Hanoi
    // Lớp mở rộng từ MyStack để áp dụng quy tắc đặc thù của Tháp Hà Nội.
    public class HanoiStack : DataStructures.MyStack<DataStructures.Disk>
    {
        // Thêm đĩa vào cọc, kiểm tra nếu đĩa lớn đặt lên đĩa nhỏ sẽ báo lỗi.
        public bool PushDisk(DataStructures.Disk disk)
        {
            var top = Peek();
            // Kiểm tra quy tắc: Đĩa lớn không được nằm trên đĩa nhỏ
            if (top != null && top.Size < disk.Size)
            {
                throw new InvalidOperationException($"Vi phạm quy tắc: Đĩa size {disk.Size} không thể đặt lên đĩa size {top.Size}");
            }
            Push(disk);
            return true;
        }
        // Tìm kiếm vị trí (tầng) của một đối tượng đĩa cụ thể trong cọc.
        public int GetDiskLevel(DataStructures.Disk disk)
        {
            int level = 0;
            foreach (var d in ToArray()) { if (d == disk) return level; level++; }
            return -1;
        }
        public bool ContainsDisk(int size)
        {
            bool found = false;
            MyStack<Disk> temp = new MyStack<Disk>();

            while (!IsEmpty())
            {
                Disk d = Pop();
                if (d.Size == size) found = true;
                temp.Push(d);
            }

            // Khôi phục Stack ban đầu
            while (!temp.IsEmpty())
                Push(temp.Pop());

            return found;
        }
    }
    // Quản lý 3 cọc
    public class Tower
    {
        private readonly HanoiStack[] pegs = { new HanoiStack(), new HanoiStack(), new HanoiStack() };
        public HanoiStack Get(int i)
        {
            if (i < 0 || i > 2) throw new IndexOutOfRangeException();
            return pegs[i];
        }
        // Khởi tạo một đĩa mới và đặt vào cọc chỉ định.
        public DataStructures.Disk CreateDisk(int size, int pegIndex = 0)
        {
            var disk = new DataStructures.Disk(size);
            Get(pegIndex).PushDisk(disk);
            return disk;
        }
    }
    #endregion
    #region 3. Logic và Giải thuật
    public class HanoiLogic
    {
        // Sử dụng MyStack để lưu lịch sử
        private DataStructures.MyStack<DataStructures.Move> moveHistory;

        public HanoiLogic(DataStructures.MyStack<DataStructures.Move> history)
        {
            moveHistory = history;
        }
        #region Hàm bổ trợ
        // Thực hiện di chuyển đĩa giữa 2 cọc
        public void MoveBetween(Tower tower, int fromIndex, int toIndex)
        {
            var from = tower.Get(fromIndex);
            var to = tower.Get(toIndex);

            if (fromIndex < 0 || fromIndex > 2 || toIndex < 0 || toIndex > 2)
                throw new IndexOutOfRangeException("Chỉ số cọc phải từ 0 đến 2");

            if (from.IsEmpty() && to.IsEmpty()) return;

            // Xác định hướng di chuyển
            if (from.IsEmpty() || (!to.IsEmpty() && from.Peek()!.Size > to.Peek()!.Size))
            {
                (from, to) = (to, from);
                (fromIndex, toIndex) = (toIndex, fromIndex);
            }

            var disk = from.Pop();
            if (disk != null)
            {
                try
                {
                    to.PushDisk(disk);
                    // Lưu lịch sử để bên ngoài UI lấy ra dùng
                    moveHistory.Push(new DataStructures.Move(fromIndex, toIndex, disk.Size));
                }
                catch (Exception ex)
                {
                    from.Push(disk); // Trả lại đĩa nếu có lỗi
                    throw new Exception("Lỗi Logic: " + ex.Message);
                }
            }
        }
        #endregion
        // Solvers
        // Giải thuật Đệ quy
        public void SolveRecursive(int n, int src, int dest, int aux, Tower tower)
        {
            if (n <= 0) return;
            SolveRecursive(n - 1, src, aux, dest, tower);

            var disk = tower.Get(src).Pop();
            if (disk != null)
            {
                tower.Get(dest).PushDisk(disk);
                moveHistory.Push(new DataStructures.Move(src, dest, disk.Size));
            }

            SolveRecursive(n - 1, aux, dest, src, tower);
        }
        /*
         n   : số đĩa
         src : cọc nguồn
         dest: cọc đích
         aux : cọc phụ
         Ý tưởng: chuyển n-1 → aux, đĩa lớn nhất → dest, n-1 → dest
        */
        // Giải thuật Lặp (Phi đệ quy)
        public void SolveNonRecursive(int n, Tower tower)
        {
            int src = 0, aux = 1, dest = 2;
            // Nếu số đĩa chẵn, hoán đổi cọc Đích và cọc Phụ
            if (n % 2 == 0) (aux, dest) = (dest, aux);

            int totalMoves = (1 << n) - 1;
            for (int i = 1; i <= totalMoves; i++)
            {
                if (i % 3 == 1) MoveBetween(tower, src, dest);
                else if (i % 3 == 2) MoveBetween(tower, src, aux);
                else MoveBetween(tower, aux, dest);
            }
        }
        #endregion
        #region Thuật toán bổ sung
        // Thuật toán thống kê số lần di chuyển từng đĩa
        // Thống kê tổng số lần di chuyển của mỗi đĩa dựa trên lịch sử.
        public DataStructures.MyStack<DataStructures.DiskStat> GetMoveStats()
        {
            var statsStack = new DataStructures.MyStack<DataStructures.DiskStat>();
            if (moveHistory.IsEmpty()) return statsStack;

            int[] counts = new int[32]; // Tăng kích thước mảng cho an toàn
            var tempHistory = new DataStructures.MyStack<DataStructures.Move>();

            while (!moveHistory.IsEmpty())
            {
                var move = moveHistory.Pop()!;
                if (move.DiskSize < counts.Length) counts[move.DiskSize]++;
                tempHistory.Push(move);
            }
            while (!tempHistory.IsEmpty()) moveHistory.Push(tempHistory.Pop()!);

            for (int i = 1; i < counts.Length; i++)
                if (counts[i] > 0) statsStack.Push(new DataStructures.DiskStat(i) { Count = counts[i] });

            return statsStack;
        }
        // Thuật toán giải Tháp Hà Nội từ trạng thái bất kỳ về cọc đích
        public void SolveFromArbitraryState(Tower tower, int n, int targetPeg)
        {
            if (n <= 0) return;

            // 1. Xác định vị trí hiện tại của đĩa n
            int currentPeg = -1;
            for (int i = 0; i < 3; i++)
            {
                if (tower.Get(i).ContainsDisk(n))
                {
                    currentPeg = i;
                    break;
                }
            }

            // Không đủ dữ liệu để xác minh nếu không tìm thấy đĩa n
            if (currentPeg == -1) return;

            // 2. Nếu đĩa n đã ở cọc đích → xử lý n-1
            if (currentPeg == targetPeg)
            {
                SolveFromArbitraryState(tower, n - 1, targetPeg);
                return;
            }

            // 3. Xác định cọc trung gian
            int helperPeg = 3 - currentPeg - targetPeg;

            // 4. Đưa n-1 đĩa về cọc trung gian (từ trạng thái bất kỳ)
            SolveFromArbitraryState(tower, n - 1, helperPeg);

            // 5. Di chuyển đĩa n sang cọc đích
            MoveBetween(tower, currentPeg, targetPeg);

            // 6. Đưa n-1 đĩa từ trung gian về cọc đích (giải chuẩn)
            SolveRecursive(n - 1, helperPeg, targetPeg, currentPeg, tower);
        }
    }
    #endregion
}