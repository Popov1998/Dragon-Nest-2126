using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DnPaker
{
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

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DnPaker.Form1));
			PBox = new System.Windows.Forms.PictureBox();
			label1 = new System.Windows.Forms.Label();
			label4 = new System.Windows.Forms.Label();
			label5 = new System.Windows.Forms.Label();
			textBox1 = new System.Windows.Forms.TextBox();
			label3 = new System.Windows.Forms.Label();
			linkLabel1 = new System.Windows.Forms.LinkLabel();
			PakGirlWorker = new System.ComponentModel.BackgroundWorker();
			linkLabel2 = new System.Windows.Forms.LinkLabel();
			((System.ComponentModel.ISupportInitialize)PBox).BeginInit();
			SuspendLayout();
			PBox.BackgroundImage = (System.Drawing.Image)resources.GetObject("PBox.BackgroundImage");
			PBox.Image = (System.Drawing.Image)resources.GetObject("PBox.Image");
			PBox.Location = new System.Drawing.Point(4, 12);
			PBox.Name = "PBox";
			PBox.Size = new System.Drawing.Size(48, 48);
			PBox.TabIndex = 0;
			PBox.TabStop = false;
			label1.AutoSize = true;
			label1.ForeColor = System.Drawing.SystemColors.AppWorkspace;
			label1.Location = new System.Drawing.Point(171, 108);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(65, 12);
			label1.TabIndex = 1;
			label1.Text = "powered by";
			label4.AutoSize = true;
			label4.Font = new System.Drawing.Font("微软雅黑", 13f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			label4.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
			label4.Location = new System.Drawing.Point(66, 12);
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size(136, 24);
			label4.TabIndex = 4;
			label4.Text = "c#";
			label5.AutoSize = true;
			label5.Font = new System.Drawing.Font("微软雅黑", 24f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);
			label5.ForeColor = System.Drawing.Color.GreenYellow;
			label5.Location = new System.Drawing.Point(1, 63);
			label5.Margin = new System.Windows.Forms.Padding(13, 0, 3, 0);
			label5.Name = "label5";
			label5.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			label5.Size = new System.Drawing.Size(58, 42);
			label5.TabIndex = 5;
			label5.Text = "00";
			label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			textBox1.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
			textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			textBox1.Enabled = false;
			textBox1.ForeColor = System.Drawing.Color.DimGray;
			textBox1.Location = new System.Drawing.Point(70, 39);
			textBox1.Multiline = true;
			textBox1.Name = "textBox1";
			textBox1.Size = new System.Drawing.Size(229, 66);
			textBox1.TabIndex = 6;
			textBox1.Text = "Drag in the patch folder";
			label3.AutoSize = true;
			label3.BackColor = System.Drawing.Color.Transparent;
			label3.Dock = System.Windows.Forms.DockStyle.Right;
			label3.Font = new System.Drawing.Font("微软雅黑", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);
			label3.ForeColor = System.Drawing.SystemColors.Info;
			label3.Location = new System.Drawing.Point(265, 0);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(46, 22);
			label3.TabIndex = 7;
			label3.Text = "0:0";
			linkLabel1.AutoSize = true;
			linkLabel1.BackColor = System.Drawing.Color.Transparent;
			linkLabel1.Font = new System.Drawing.Font("宋体", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			linkLabel1.LinkColor = System.Drawing.Color.Red;
			linkLabel1.Location = new System.Drawing.Point(10, 108);
			linkLabel1.Name = "linkLabel1";
			linkLabel1.Size = new System.Drawing.Size(35, 12);
			linkLabel1.TabIndex = 8;
			linkLabel1.TabStop = true;
			linkLabel1.Text = "FAQ?";
			linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel1_LinkClicked);
			PakGirlWorker.WorkerReportsProgress = true;
			PakGirlWorker.WorkerSupportsCancellation = true;
			PakGirlWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(PakGirlWorker_DoWork);
			PakGirlWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(PakGirlWorker_ProgressChanged);
			PakGirlWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(PakGirlWorker_RunWorkerCompleted);
			linkLabel2.AutoSize = true;
			linkLabel2.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			linkLabel2.LinkColor = System.Drawing.Color.Coral;
			linkLabel2.Location = new System.Drawing.Point(234, 108);
			linkLabel2.Name = "linkLabel2";
			linkLabel2.Size = new System.Drawing.Size(77, 12);
			linkLabel2.TabIndex = 10;
			linkLabel2.TabStop = true;
			linkLabel2.Text = "C# sharp engine";
			linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel2_LinkClicked);
			AllowDrop = true;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
			base.ClientSize = new System.Drawing.Size(311, 123);
			base.Controls.Add(linkLabel2);
			base.Controls.Add(linkLabel1);
			base.Controls.Add(label3);
			base.Controls.Add(textBox1);
			base.Controls.Add(label5);
			base.Controls.Add(label4);
			base.Controls.Add(label1);
			base.Controls.Add(PBox);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.Name = "Form1";
			Text = "PAK Packing System";
			base.TopMost = true;
			base.DragDrop += new System.Windows.Forms.DragEventHandler(Form1_DragDrop);
			base.DragEnter += new System.Windows.Forms.DragEventHandler(Form1_DragEnter);
			base.MouseDown += new System.Windows.Forms.MouseEventHandler(Form1_MouseDown);
			base.MouseMove += new System.Windows.Forms.MouseEventHandler(Form1_MouseMove);
			base.MouseUp += new System.Windows.Forms.MouseEventHandler(Form1_MouseUp);
			((System.ComponentModel.ISupportInitialize)PBox).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		public Form1()
		{
			InitializeComponent();
			PBox.Image = null;
		}

		private void Form1_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.Copy;
			}
			else
			{
				e.Effect = DragDropEffects.None;
			}
		}

		private void Form1_DragDrop(object sender, DragEventArgs e)
		{
			if (!PakGirlWorker.IsBusy)
			{
				textBox1.Text = "";
				try
				{
					FilesToDo = e.Data.GetData(DataFormats.FileDrop, autoConvert: false) as string[];
					FilesToPak = new string[FilesToDo.Length];
					for (int i = 0; i < FilesToDo.Length; i++)
					{
						if (File.Exists(FilesToDo[i]))
						{
							FilesToPak[i] = "R";
						}
						else if (Directory.Exists(FilesToDo[i]))
						{
							FilesToPak[i] = "P";
						}
						else
						{
							MessageBox.Show("You dragged in " + FilesToDo[i] + "？");
						}
					}
					if (FilesToDo.Length > 0)
					{
						PBox.Image = (Image)resources.GetObject("PBox.Image");
						DoPakGirl(FilesToDo[0]);
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				MessageBox.Show("Busy, wait!");
			}
		}

		private void DoPakGirl(string CurentPath)
		{
			label3.Text = CurentID + 1 + ":" + FilesToDo.Length;
			label4.Text = FilesToPak[CurentID] + ":" + Path.GetFileName(CurentPath);
			PakGirlWorker.RunWorkerAsync(new string[2]
			{
				CurentPath,
				FilesToPak[CurentID]
			});
		}

		private void PakGirlWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			string[] array = (string[])e.Argument;
			if (array[1] == "P")
			{
				e.Result = PakGirl.File2Pak(array[0], PakGirlWorker);
			}
			else if (array[1] == "R")
			{
				e.Result = PakGirl.Pak2File(array[0], PakGirlWorker);
			}
		}

		private void PakGirlWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			label5.Text = e.ProgressPercentage.ToString();
			textBox1.Text = e.UserState.ToString();
		}

		private void PakGirlWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			CurentID++;
			totalFiles += (int)e.Result;
			PakGirlWorker.Dispose();
			try
			{
				if (CurentID < FilesToDo.Length)
				{
					DoPakGirl(FilesToDo[CurentID]);
					return;
				}
				PakGirlWorker.Dispose();
				CurentID = 0;
				label4.Text = FilesToDo.Length + "Item processed";
				textBox1.Text = "Related documents are: " + totalFiles + " in total";
				PBox.Image = null;
				FilesToDo = null;
				FilesToPak = null;
				totalFiles = 0;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			string text = "This program requires .net framework 2.0 or higher, which can be cross-platform.";
			MessageBox.Show(text);
		}

		private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("https://www.facebook.com/MysticOP?");
		}

		private void Form1_MouseDown(object sender, MouseEventArgs e)
		{
			isDown = true;
			OP = e.Location;
		}

		private void Form1_MouseMove(object sender, MouseEventArgs e)
		{
			if (isDown)
			{
				SetBounds(base.Left + e.X - OP.X, base.Top + e.Y - OP.Y, base.Width, base.Height);
			}
		}

		private void Form1_MouseUp(object sender, MouseEventArgs e)
		{
			isDown = false;
		}
	}
}
