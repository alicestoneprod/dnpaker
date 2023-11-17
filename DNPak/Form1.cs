namespace DnPaker
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    public class Form1 : Form
    {
        private IContainer components = null;
        private ComponentResourceManager resources = new ComponentResourceManager(typeof(Form1));
        private PictureBox PBox;
        private Label label1;
        private Label label4;
        private Label label5;
        private TextBox textBox1;
        private Label label3;
        private LinkLabel linkLabel1;
        private BackgroundWorker PakGirlWorker;
        private LinkLabel linkLabel2;
        private string[] FilesToDo;
        private string[] FilesToPak;
        private int CurentID = 0;
        private int totalFiles = 0;
        private bool isDown = false;
        private Point OP;

        public Form1()
        {
            this.InitializeComponent();
            this.PBox.Image = null;
        }

        protected override void Dispose(bool disposing)
        {
            if (!(!disposing || ReferenceEquals(this.components, null)))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DoPakGirl(string CurentPath)
        {
            this.label3.Text = (this.CurentID + 1) + "@" + this.FilesToDo.Length;
            this.label4.Text = this.FilesToPak[this.CurentID] + ":" + Path.GetFileName(CurentPath);
            string[] argument = new string[] { CurentPath, this.FilesToPak[this.CurentID] };
            this.PakGirlWorker.RunWorkerAsync(argument);
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            bool isBusy = this.PakGirlWorker.IsBusy;
            if (isBusy)
            {
                MessageBox.Show("В данный момент программа занята. Подождите.");
            }
            else
            {
                this.textBox1.Text = "";
                try
                {
                    this.FilesToDo = e.Data.GetData(DataFormats.FileDrop, false) as string[];
                    this.FilesToPak = new string[this.FilesToDo.Length];
                    int index = 0;
                    while (true)
                    {
                        isBusy = index < this.FilesToDo.Length;
                        if (!isBusy)
                        {
                            if (this.FilesToDo.Length > 0)
                            {
                                this.PBox.Image = (Image) this.resources.GetObject("PBox.Image");
                                this.DoPakGirl(this.FilesToDo[0]);
                            }
                            break;
                        }
                        if (File.Exists(this.FilesToDo[index]))
                        {
                            this.FilesToPak[index] = "R";
                        }
                        else if (Directory.Exists(this.FilesToDo[index]))
                        {
                            this.FilesToPak[index] = "P";
                        }
                        else
                        {
                            MessageBox.Show("Что это за файл " + this.FilesToDo[index] + " , который вы перетащили?");
                        }
                        index++;
                    }
                }
                catch (Exception exception1)
                {
                    MessageBox.Show(exception1.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = !e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.None : DragDropEffects.Copy;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            this.isDown = true;
            this.OP = e.Location;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.isDown)
            {
                base.SetBounds((base.Left + e.X) - this.OP.X, (base.Top + e.Y) - this.OP.Y, base.Width, base.Height);
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            this.isDown = false;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(Form1));
            this.PBox = new PictureBox();
            this.label1 = new Label();
            this.label4 = new Label();
            this.label5 = new Label();
            this.textBox1 = new TextBox();
            this.label3 = new Label();
            this.linkLabel1 = new LinkLabel();
            this.PakGirlWorker = new BackgroundWorker();
            this.linkLabel2 = new LinkLabel();
            ((ISupportInitialize) this.PBox).BeginInit();
            base.SuspendLayout();
            this.PBox.BackgroundImage = (Image) manager.GetObject("PBox.BackgroundImage");
            this.PBox.Image = (Image) manager.GetObject("PBox.Image");
            this.PBox.Location = new Point(4, 12);
            this.PBox.Name = "PBox";
            this.PBox.Size = new Size(0x30, 0x30);
            this.PBox.TabIndex = 0;
            this.PBox.TabStop = false;
            this.label1.AutoSize = true;
            this.label1.ForeColor = SystemColors.AppWorkspace;
            this.label1.Location = new Point(0xab, 0x6c);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x41, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Переведен";
            this.label4.AutoSize = true;
            this.label4.Font = new Font("微软雅黑", 13f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.label4.ForeColor = SystemColors.ButtonHighlight;
            this.label4.Location = new Point(0x42, 12);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x88, 0x18);
            this.label4.TabIndex = 4;
            this.label4.Text = "Распаковщик и упаковщик файлов .pak";
            this.label5.AutoSize = true;
            this.label5.Font = new Font("微软雅黑", 24f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.label5.ForeColor = Color.GreenYellow;
            this.label5.Location = new Point(1, 0x3f);
            this.label5.Margin = new Padding(13, 0, 3, 0);
            this.label5.Name = "label5";
            this.label5.RightToLeft = RightToLeft.Yes;
            this.label5.Size = new Size(0x3a, 0x2a);
            this.label5.TabIndex = 5;
            this.label5.Text = "00";
            this.label5.TextAlign = ContentAlignment.MiddleCenter;
            this.textBox1.BackColor = Color.FromArgb(40, 40, 40);
            this.textBox1.BorderStyle = BorderStyle.None;
            this.textBox1.Enabled = false;
            this.textBox1.ForeColor = Color.DimGray;
            this.textBox1.Location = new Point(70, 0x27);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(0xe5, 0x42);
            this.textBox1.TabIndex = 6;
            this.textBox1.Text = "Перетащите файл (.Pak) для распаковки (R) или перетащите папку для запаковки (P); также поддерживается массовое перетаскивание и смешанное перетаскивание; распаковка/упаковка происходит в папку с тем же именем что и у оригинального файла";
            this.label3.AutoSize = true;
            this.label3.BackColor = Color.Transparent;
            this.label3.Dock = DockStyle.Right;
            this.label3.Font = new Font("微软雅黑", 12f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.label3.ForeColor = SystemColors.Info;
            this.label3.Location = new Point(0x109, 0);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x2e, 0x16);
            this.label3.TabIndex = 7;
            this.label3.Text = "0@0";
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.BackColor = Color.Transparent;
            this.linkLabel1.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.linkLabel1.LinkColor = Color.Red;
            this.linkLabel1.Location = new Point(10, 0x6c);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new Size(0x23, 12);
            this.linkLabel1.TabIndex = 8;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Нужна помощь?";
            this.linkLabel1.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            this.PakGirlWorker.WorkerReportsProgress = true;
            this.PakGirlWorker.WorkerSupportsCancellation = true;
            this.PakGirlWorker.DoWork += new DoWorkEventHandler(this.PakGirlWorker_DoWork);
            this.PakGirlWorker.ProgressChanged += new ProgressChangedEventHandler(this.PakGirlWorker_ProgressChanged);
            this.PakGirlWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.PakGirlWorker_RunWorkerCompleted);
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.LinkBehavior = LinkBehavior.HoverUnderline;
            this.linkLabel2.LinkColor = Color.Coral;
            this.linkLabel2.Location = new Point(0xea, 0x6c);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new Size(0x4d, 12);
            this.linkLabel2.TabIndex = 10;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "hsu苏@DuoWan";
            this.linkLabel2.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            this.AllowDrop = true;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(40, 40, 40);
            base.ClientSize = new Size(0x137, 0x7b);
            base.Controls.Add(this.linkLabel2);
            base.Controls.Add(this.linkLabel1);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.textBox1);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.PBox);
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "Form1";
            this.Text = "Распаковщик и упаковщик файлов .pak";
            base.TopMost = true;
            base.DragDrop += new DragEventHandler(this.Form1_DragDrop);
            base.DragEnter += new DragEventHandler(this.Form1_DragEnter);
            base.MouseDown += new MouseEventHandler(this.Form1_MouseDown);
            base.MouseMove += new MouseEventHandler(this.Form1_MouseMove);
            base.MouseUp += new MouseEventHandler(this.Form1_MouseUp);
            ((ISupportInitialize) this.PBox).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show(".NET 2.0, перевод: https://vk.com/ahegaololi (front-end developer).\n 【BUG】В случае ошибки нужно написать сообщение на rydeanvi@qq.com, (автор вряд ли вам ответит). Создатель часто упаковывал и запаковывал файлы, поэтому он создал свою программу.\n Исходный файл:【Resource-12345/resource/[files]】---> Resource-12345.pak (на выходе)");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://bbs.duowan.com/space-uid-23372083.html");
        }

        private void PakGirlWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string[] argument = (string[]) e.Argument;
            if (argument[1] == "P")
            {
                e.Result = PakGirl.File2Pak(argument[0], this.PakGirlWorker);
            }
            else if (argument[1] == "R")
            {
                e.Result = PakGirl.Pak2File(argument[0], this.PakGirlWorker);
            }
        }

        private void PakGirlWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.label5.Text = e.ProgressPercentage.ToString();
            this.textBox1.Text = e.UserState.ToString();
        }

        private void PakGirlWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.CurentID++;
            this.totalFiles += (int) e.Result;
            this.PakGirlWorker.Dispose();
            try
            {
                if (this.CurentID < this.FilesToDo.Length)
                {
                    this.DoPakGirl(this.FilesToDo[this.CurentID]);
                }
                else
                {
                    this.PakGirlWorker.Dispose();
                    this.CurentID = 0;
                    this.label4.Text = this.FilesToDo.Length + "Процесс завершен";
                    this.textBox1.Text = "Всего документов:" + this.totalFiles;
                    this.PBox.Image = null;
                    this.FilesToDo = null;
                    this.FilesToPak = null;
                    this.totalFiles = 0;
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message);
            }
        }
    }
}

