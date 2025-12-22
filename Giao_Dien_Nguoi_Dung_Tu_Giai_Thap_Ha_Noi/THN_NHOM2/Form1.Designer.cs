namespace THN_NHOM2
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.a = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.b = new System.Windows.Forms.PictureBox();
            this.c = new System.Windows.Forms.PictureBox();
            this.RodA = new System.Windows.Forms.PictureBox();
            this.RodB = new System.Windows.Forms.PictureBox();
            this.RodC = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dia1 = new System.Windows.Forms.PictureBox();
            this.dia3 = new System.Windows.Forms.PictureBox();
            this.dia4 = new System.Windows.Forms.PictureBox();
            this.dia5 = new System.Windows.Forms.PictureBox();
            this.dia6 = new System.Windows.Forms.PictureBox();
            this.dia7 = new System.Windows.Forms.PictureBox();
            this.dia8 = new System.Windows.Forms.PictureBox();
            this.dia2 = new System.Windows.Forms.PictureBox();
            this.lblTime = new System.Windows.Forms.Label();
            this.tmrCountTime = new System.Windows.Forms.Timer(this.components);
            this.lblMoveCount = new System.Windows.Forms.Label();
            this.nudLevel = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnGiveIn = new System.Windows.Forms.Button();
            this.ShowRule = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.a)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.b)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RodA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RodB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RodC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dia1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dia3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dia4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dia5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dia6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dia7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dia8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dia2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLevel)).BeginInit();
            this.SuspendLayout();
            // 
            // a
            // 
            this.a.Image = global::THN_NHOM2.Properties.Resources.đế;
            this.a.Location = new System.Drawing.Point(83, 443);
            this.a.Name = "a";
            this.a.Size = new System.Drawing.Size(236, 32);
            this.a.TabIndex = 0;
            this.a.TabStop = false;
            this.a.Click += new System.EventHandler(this.a_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(189, 478);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "A";
            // 
            // b
            // 
            this.b.Image = global::THN_NHOM2.Properties.Resources.đế;
            this.b.Location = new System.Drawing.Point(330, 443);
            this.b.Name = "b";
            this.b.Size = new System.Drawing.Size(236, 32);
            this.b.TabIndex = 2;
            this.b.TabStop = false;
            // 
            // c
            // 
            this.c.Image = global::THN_NHOM2.Properties.Resources.đế;
            this.c.Location = new System.Drawing.Point(580, 443);
            this.c.Name = "c";
            this.c.Size = new System.Drawing.Size(236, 32);
            this.c.TabIndex = 3;
            this.c.TabStop = false;
            // 
            // RodA
            // 
            this.RodA.Image = global::THN_NHOM2.Properties.Resources.trụ;
            this.RodA.Location = new System.Drawing.Point(190, 204);
            this.RodA.Name = "RodA";
            this.RodA.Size = new System.Drawing.Size(22, 242);
            this.RodA.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.RodA.TabIndex = 4;
            this.RodA.TabStop = false;
            this.RodA.Click += new System.EventHandler(this.Rod_Click);
            // 
            // RodB
            // 
            this.RodB.Image = global::THN_NHOM2.Properties.Resources.trụ;
            this.RodB.Location = new System.Drawing.Point(437, 204);
            this.RodB.Name = "RodB";
            this.RodB.Size = new System.Drawing.Size(22, 242);
            this.RodB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.RodB.TabIndex = 5;
            this.RodB.TabStop = false;
            this.RodB.Click += new System.EventHandler(this.Rod_Click);
            // 
            // RodC
            // 
            this.RodC.Image = global::THN_NHOM2.Properties.Resources.trụ;
            this.RodC.Location = new System.Drawing.Point(687, 204);
            this.RodC.Name = "RodC";
            this.RodC.Size = new System.Drawing.Size(22, 242);
            this.RodC.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.RodC.TabIndex = 6;
            this.RodC.TabStop = false;
            this.RodC.Click += new System.EventHandler(this.Rod_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(433, 478);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "B";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(697, 478);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(21, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "C";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // dia1
            // 
            this.dia1.Image = global::THN_NHOM2.Properties.Resources._1;
            this.dia1.Location = new System.Drawing.Point(94, 418);
            this.dia1.Name = "dia1";
            this.dia1.Size = new System.Drawing.Size(214, 28);
            this.dia1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.dia1.TabIndex = 9;
            this.dia1.TabStop = false;
            this.dia1.Tag = "1";
            this.dia1.Visible = false;
            this.dia1.Click += new System.EventHandler(this.dia1_Click);
            // 
            // dia3
            // 
            this.dia3.Image = global::THN_NHOM2.Properties.Resources._3;
            this.dia3.Location = new System.Drawing.Point(591, 248);
            this.dia3.Name = "dia3";
            this.dia3.Size = new System.Drawing.Size(214, 28);
            this.dia3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.dia3.TabIndex = 10;
            this.dia3.TabStop = false;
            this.dia3.Tag = "3";
            this.dia3.Visible = false;
            this.dia3.Click += new System.EventHandler(this.dia1_Click);
            // 
            // dia4
            // 
            this.dia4.Image = global::THN_NHOM2.Properties.Resources._4;
            this.dia4.Location = new System.Drawing.Point(591, 282);
            this.dia4.Name = "dia4";
            this.dia4.Size = new System.Drawing.Size(214, 28);
            this.dia4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.dia4.TabIndex = 11;
            this.dia4.TabStop = false;
            this.dia4.Tag = "4";
            this.dia4.Visible = false;
            this.dia4.Click += new System.EventHandler(this.dia1_Click);
            // 
            // dia5
            // 
            this.dia5.Image = global::THN_NHOM2.Properties.Resources._5;
            this.dia5.Location = new System.Drawing.Point(591, 316);
            this.dia5.Name = "dia5";
            this.dia5.Size = new System.Drawing.Size(214, 28);
            this.dia5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.dia5.TabIndex = 12;
            this.dia5.TabStop = false;
            this.dia5.Tag = "5";
            this.dia5.Visible = false;
            this.dia5.Click += new System.EventHandler(this.dia1_Click);
            // 
            // dia6
            // 
            this.dia6.Image = global::THN_NHOM2.Properties.Resources._6;
            this.dia6.Location = new System.Drawing.Point(591, 350);
            this.dia6.Name = "dia6";
            this.dia6.Size = new System.Drawing.Size(214, 28);
            this.dia6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.dia6.TabIndex = 13;
            this.dia6.TabStop = false;
            this.dia6.Tag = "6";
            this.dia6.Visible = false;
            this.dia6.Click += new System.EventHandler(this.dia1_Click);
            // 
            // dia7
            // 
            this.dia7.Image = global::THN_NHOM2.Properties.Resources._7;
            this.dia7.Location = new System.Drawing.Point(591, 384);
            this.dia7.Name = "dia7";
            this.dia7.Size = new System.Drawing.Size(214, 28);
            this.dia7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.dia7.TabIndex = 14;
            this.dia7.TabStop = false;
            this.dia7.Tag = "7";
            this.dia7.Visible = false;
            this.dia7.Click += new System.EventHandler(this.dia1_Click);
            // 
            // dia8
            // 
            this.dia8.Image = global::THN_NHOM2.Properties.Resources._8;
            this.dia8.Location = new System.Drawing.Point(591, 418);
            this.dia8.Name = "dia8";
            this.dia8.Size = new System.Drawing.Size(214, 28);
            this.dia8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.dia8.TabIndex = 15;
            this.dia8.TabStop = false;
            this.dia8.Tag = "8";
            this.dia8.Visible = false;
            this.dia8.Click += new System.EventHandler(this.dia1_Click);
            // 
            // dia2
            // 
            this.dia2.Image = global::THN_NHOM2.Properties.Resources._2;
            this.dia2.Location = new System.Drawing.Point(341, 418);
            this.dia2.Name = "dia2";
            this.dia2.Size = new System.Drawing.Size(214, 28);
            this.dia2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.dia2.TabIndex = 16;
            this.dia2.TabStop = false;
            this.dia2.Tag = "2";
            this.dia2.Visible = false;
            this.dia2.Click += new System.EventHandler(this.dia1_Click);
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.BackColor = System.Drawing.Color.White;
            this.lblTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.ForeColor = System.Drawing.Color.Blue;
            this.lblTime.Location = new System.Drawing.Point(375, 9);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(149, 22);
            this.lblTime.TabIndex = 17;
            this.lblTime.Text = "Thời Gian: 00:00:00";
            // 
            // tmrCountTime
            // 
            this.tmrCountTime.Interval = 1000;
            this.tmrCountTime.Tick += new System.EventHandler(this.tmrCountTime_Tick);
            // 
            // lblMoveCount
            // 
            this.lblMoveCount.AutoSize = true;
            this.lblMoveCount.BackColor = System.Drawing.Color.White;
            this.lblMoveCount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblMoveCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMoveCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lblMoveCount.Location = new System.Drawing.Point(359, 31);
            this.lblMoveCount.Name = "lblMoveCount";
            this.lblMoveCount.Size = new System.Drawing.Size(181, 22);
            this.lblMoveCount.TabIndex = 18;
            this.lblMoveCount.Text = "Số Lần Di Chuyển: 0 lần";
            // 
            // nudLevel
            // 
            this.nudLevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudLevel.Location = new System.Drawing.Point(450, 60);
            this.nudLevel.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.nudLevel.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudLevel.Name = "nudLevel";
            this.nudLevel.Size = new System.Drawing.Size(40, 26);
            this.nudLevel.TabIndex = 19;
            this.nudLevel.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(391, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 20);
            this.label4.TabIndex = 20;
            this.label4.Text = "Số đĩa";
            // 
            // btnPlay
            // 
            this.btnPlay.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPlay.Location = new System.Drawing.Point(293, 96);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(100, 30);
            this.btnPlay.TabIndex = 21;
            this.btnPlay.Text = "Chơi";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // btnGiveIn
            // 
            this.btnGiveIn.Enabled = false;
            this.btnGiveIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGiveIn.Location = new System.Drawing.Point(412, 96);
            this.btnGiveIn.Name = "btnGiveIn";
            this.btnGiveIn.Size = new System.Drawing.Size(100, 30);
            this.btnGiveIn.TabIndex = 22;
            this.btnGiveIn.Text = "Chịu Thua";
            this.btnGiveIn.UseVisualStyleBackColor = true;
            this.btnGiveIn.Click += new System.EventHandler(this.GiveIn_Click);
            // 
            // ShowRule
            // 
            this.ShowRule.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShowRule.Location = new System.Drawing.Point(531, 96);
            this.ShowRule.Name = "ShowRule";
            this.ShowRule.Size = new System.Drawing.Size(100, 30);
            this.ShowRule.TabIndex = 23;
            this.ShowRule.Text = "Luật Chơi";
            this.ShowRule.UseVisualStyleBackColor = true;
            this.ShowRule.Click += new System.EventHandler(this.ShowRule_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(880, 507);
            this.Controls.Add(this.ShowRule);
            this.Controls.Add(this.btnGiveIn);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.nudLevel);
            this.Controls.Add(this.lblMoveCount);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.dia2);
            this.Controls.Add(this.dia8);
            this.Controls.Add(this.dia7);
            this.Controls.Add(this.dia6);
            this.Controls.Add(this.dia5);
            this.Controls.Add(this.dia4);
            this.Controls.Add(this.dia3);
            this.Controls.Add(this.dia1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.RodC);
            this.Controls.Add(this.RodB);
            this.Controls.Add(this.RodA);
            this.Controls.Add(this.c);
            this.Controls.Add(this.b);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.a);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Tháp Hà Nội";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.a)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.b)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RodA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RodB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RodC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dia1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dia3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dia4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dia5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dia6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dia7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dia8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dia2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLevel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox a;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox b;
        private System.Windows.Forms.PictureBox c;
        private System.Windows.Forms.PictureBox RodA;
        private System.Windows.Forms.PictureBox RodB;
        private System.Windows.Forms.PictureBox RodC;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox dia1;
        private System.Windows.Forms.PictureBox dia3;
        private System.Windows.Forms.PictureBox dia4;
        private System.Windows.Forms.PictureBox dia5;
        private System.Windows.Forms.PictureBox dia6;
        private System.Windows.Forms.PictureBox dia7;
        private System.Windows.Forms.PictureBox dia8;
        private System.Windows.Forms.PictureBox dia2;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Timer tmrCountTime;
        private System.Windows.Forms.Label lblMoveCount;
        private System.Windows.Forms.NumericUpDown nudLevel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Button btnGiveIn;
        private System.Windows.Forms.Button ShowRule;
    }
}

