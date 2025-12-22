using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace THN_NHOM2
{
    public partial class Form1 : Form
    {
        TimeSpan time;
        int moveCount;
        //Ẩn đĩa bằng mảng
        PictureBox[] disks;
        Stack<PictureBox> disksA, disksB, disksC, firstClickedDisks, secondClickedDisks;
        const int FIRSTY = 418, DISKHEIGHT = 34, DISTXFROMRODTODISK = 11; //tọa đọ ban đầu và k/c các đĩa 
        public Form1()
        {
            InitializeComponent();
            //Tạo mảng 1 chiều tham chiếu đến các đĩa 
            disks = new PictureBox[] { dia1, dia2, dia3, dia4, dia5, dia6, dia7, dia8 };
            RodA.Tag = disksA = new Stack<PictureBox>();
            RodB.Tag = disksB = new Stack<PictureBox>();
            RodC.Tag = disksC = new Stack<PictureBox>();
        }
        private void dia1_Click(object sender, EventArgs e)
        {
            PictureBox clickedDisk = sender as PictureBox;

            if (clickedDisk == null) return;

            int diskSize = int.Parse(clickedDisk.Tag.ToString());

            MessageBox.Show("Bạn vừa click vào đĩa số: " + diskSize);
        }

        private void RodA_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Bạn vừa click vào trụ A");
        }

        private void picRod_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ShowRule_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
    "Luật chơi:\n" +
    "- Mỗi lần chỉ được di chuyển một đĩa trên cùng của cọc.\n" +
    "- Đĩa nằm trên phải nhỏ hơn đĩa nằm dưới.",
    "Luật Chơi",
    MessageBoxButtons.OK,
    MessageBoxIcon.Information
);
        }

        private void Rod_Click(object sender, EventArgs e)
        {
            if (nudLevel.Enabled) return; //is not playing 
            PictureBox clickedRod = (PictureBox)sender;
            Stack<PictureBox>  disksOfClickedRod = (Stack<PictureBox>) clickedRod.Tag;

            if (firstClickedDisks == null)     // lần click đầu
            {
                if (disksOfClickedRod.Count == 0) { return; } //kiểm tra 0 có đĩa 
                firstClickedDisks = disksOfClickedRod;
                clickedRod.BorderStyle = BorderStyle.FixedSingle;
            }
            else if (secondClickedDisks == null) //lần click thứ hai
            {
                if (disksOfClickedRod == firstClickedDisks) //Nhấn 1 lần là chọn, bỏ chọn thì nhấn 1 lần nữa 
                {
                    firstClickedDisks = null;
                    clickedRod.BorderStyle = BorderStyle.FixedSingle;
                    return;
                }
                secondClickedDisks = disksOfClickedRod;
                ProcessMovingDisk(clickedRod);//Di chuyển đĩa ntn 
            }
        }

        private void dia_Click(object sender, EventArgs e)
        {
            PictureBox clickedDisk = (PictureBox)sender;
            if (disksA.Contains(clickedDisk))
                Rod_Click(RodA, new EventArgs());
            else if (disksB.Contains(clickedDisk))
                Rod_Click(RodB, new EventArgs());
            else
                Rod_Click(RodC, new EventArgs());
        }

        private void ProcessMovingDisk(PictureBox clickedRod)
        {
            PictureBox firstTopDisk = firstClickedDisks.Peek();

            // Tính X chuẩn (canh giữa trụ)
            int x = clickedRod.Left + (clickedRod.Width - firstTopDisk.Width) / 2;

            int y;

            if (secondClickedDisks.Count == 0)
            {
                // Cọc trống → đặt đĩa ở đáy
                y = FIRSTY;
                MoveDisk(new Point(x, y));
            }
            else
            {
                PictureBox secondTopDisk = secondClickedDisks.Peek();

                // Kiểm tra luật
                if (int.Parse(firstTopDisk.Tag.ToString()) <
                    int.Parse(secondTopDisk.Tag.ToString()))
                {
                    y = secondTopDisk.Top - DISKHEIGHT;
                    MoveDisk(new Point(x, y));
                }
                else
                {
                    secondClickedDisks = null;
                }
            }
        }

        private void MoveDisk (Point point) 
            {
            PictureBox firstTopDisk = firstClickedDisks.Pop();
            firstTopDisk.Location = point;
            secondClickedDisks.Push(firstTopDisk);
            ++moveCount;
            lblMoveCount.Text = string.Format("Số Lần Di Chuyển: {0} lần", moveCount);
            firstClickedDisks = secondClickedDisks = null;
            RodA.BorderStyle = RodB.BorderStyle = RodC.BorderStyle = BorderStyle.None;

            if (disksC.Count == nudLevel.Value)
            {
                btnGiveIn.PerformClick();
                MessageBox.Show("Chúc mừng bạn đã hoàn thành trò chơi", "Chúc mừng");
            }
            } 

        private void tmrCountTime_Tick(object sender, EventArgs e)
        {
            time = time.Add(new TimeSpan(0, 0, 1));
            lblTime.Text = string.Format("Thời Gian: {0:00}:{1:00}:{2:00}", time.Hours,time.Minutes, time.Seconds);
        }

        private void a_Click(object sender, EventArgs e)
        {

        }

        private void GiveIn_Click(object sender, EventArgs e)
        {
            tmrCountTime.Stop();
            nudLevel.Enabled = true;
            btnGiveIn.Enabled = false;
            btnPlay.Text = "Chơi";

        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            //reset 
            tmrCountTime.Stop(); 
            foreach (PictureBox disk in disks)  // Ẩn đĩa
                disk.Visible = false;

            time = new TimeSpan(0);
            moveCount = 0;
            lblTime.Text = "Thời Gian: 00:00:00";
            lblMoveCount.Text = "Số Lần Di Chuyển: 0 lần";
            disksA.Clear(); disksB.Clear(); disksC.Clear();  //Xóa hết đĩa trước đó
            firstClickedDisks = secondClickedDisks = null;

            //Khởi tạo tt
            nudLevel.Enabled = false;
            btnGiveIn.Enabled = true;
            btnPlay.Text  = "Chơi Lại";
            int x = a.Location.X + DISTXFROMRODTODISK, y = FIRSTY;

            for (int i = (int)nudLevel.Value - 1; i>=0; --i, y -= DISKHEIGHT)
            {
                disks[i].Location = new Point(x, y); //Tọa độ
                disks[i].Visible = true;
                disksA.Push(disks[i]);
            }
            tmrCountTime.Start();
        }
    }
}
