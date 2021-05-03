using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
namespace PathCreate
{
	public class Form1 : Form
	{
		private string FilePatch = "";
		private IContainer components;
		private Label label1;
		private Label label2;
		private Panel panel1;
		private Label label3;
		private Label md5label;
		private Label label4;
		private Label label5;
		private Label FileCountLabel;
		private Label label7;
		private Label label6;
		private Label FileNameLabel;
		private Label label8;
		public Form1()
		{
			this.InitializeComponent();
			this.panel1.AllowDrop = true;
			this.panel1.DragEnter += new DragEventHandler(this.panel1_DragEnter);
			this.panel1.DragDrop += new DragEventHandler(this.panel1_DragDrop);
			this.FileCountLabel.Text = "0";
		}
		protected string GetMD5HashFromFile(string fileName)
		{
			string result;
			using (MD5 mD = MD5.Create())
			{
				using (FileStream fileStream = File.OpenRead(fileName))
				{
					result = BitConverter.ToString(mD.ComputeHash(fileStream)).Replace("-", string.Empty);
				}
			}
			return result;
		}
		public void fileSove(string fileName, string text)
		{
			FileStream expr_07 = new FileStream(fileName, FileMode.OpenOrCreate);
			StreamWriter streamWriter = new StreamWriter(expr_07);
			expr_07.Seek(0L, SeekOrigin.End);
			streamWriter.WriteLine(text);
			streamWriter.Close();
		}
		private void panel1_DragDrop(object sender, DragEventArgs e)
		{
			string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
			this.FilePatch = "";
			for (int i = 0; i < array.Length; i++)
			{
				this.FilePatch += array[i];
				if (string.Equals(Path.GetExtension(array[i]), ".pak", StringComparison.InvariantCultureIgnoreCase))
				{
					this.CretePatch(this.FilePatch);
				}
			}
		}
		private void CretePatch(string PatchImput)
		{
			string text = this.GetMD5HashFromFile(PatchImput).ToLower();
			string fileName = Path.GetFileName(PatchImput);
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(PatchImput);
			string str = Path.GetDirectoryName(PatchImput) + "\\" + fileNameWithoutExtension;
			this.md5label.Text = text;
			this.FileNameLabel.Text = fileName;
			using (FileStream fileStream = File.OpenRead(PatchImput))
			{
				HEADER hEADER = fileStream.ReadStruct<HEADER>();
				this.FileCountLabel.Text = Convert.ToString(hEADER.FileCounts);
				if (File.Exists(str + ".pak.md5"))
				{
					File.Delete(str + ".pak.md5");
				}
				this.fileSove(str + ".pak.md5", text);
				if (File.Exists(str + ".txt"))
				{
					File.Delete(str + ".txt");
				}
				fileStream.Position = (long)((ulong)hEADER.TableOffset);
				int num = 0;
				while ((long)num < (long)((ulong)hEADER.FileCounts))
				{
					FILELIST fILELIST = fileStream.ReadStruct<FILELIST>();
					string str2 = this.ByteToString(fILELIST.NameFile);
					this.fileSove(str + ".txt", "D " + str2);
					num++;
				}
			}
		}
		public string ByteToString(byte[] bfiles)
		{
			string arg_05_0 = string.Empty;
			return Encoding.ASCII.GetString(bfiles).Trim(new char[1]).Split(new char[1]).First<string>().Remove(0, 1);
		}
		private void panel1_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop) && (e.AllowedEffect & DragDropEffects.Move) == DragDropEffects.Move)
			{
				e.Effect = DragDropEffects.Move;
			}
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(Form1));
			this.label1 = new Label();
			this.label2 = new Label();
			this.panel1 = new Panel();
			this.md5label = new Label();
			this.label3 = new Label();
			this.label4 = new Label();
			this.label5 = new Label();
			this.FileCountLabel = new Label();
			this.label6 = new Label();
			this.label7 = new Label();
			this.FileNameLabel = new Label();
			this.label8 = new Label();
			this.panel1.SuspendLayout();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Location = new Point(32, 27);
			this.label1.Name = "label1";
			this.label1.Size = new Size(27, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "md5";
			this.label2.AutoSize = true;
			this.label2.Location = new Point(66, 8);
			this.label2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
			this.label2.Font = new System.Drawing.Font("微软雅黑", 13f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 120);
			this.label2.Name = "label2";
			this.label2.Size = new Size(120, 24);
			this.label2.TabIndex = 3;
			this.label2.Text = "Drag the (.pak) to the file!";
			this.panel1.Controls.Add(this.label8);
			this.panel1.Controls.Add(this.FileNameLabel);
			this.panel1.Controls.Add(this.label7);
			this.panel1.Controls.Add(this.label6);
			this.panel1.Controls.Add(this.FileCountLabel);
			this.panel1.Controls.Add(this.label5);
			this.panel1.Controls.Add(this.label4);
			this.panel1.Controls.Add(this.md5label);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Location = new Point(-2, 1);
			this.panel1.Name = "panel1";
			this.panel1.Size = new Size(320, 120);
			this.panel1.TabIndex = 4;
			this.panel1.DragDrop += new DragEventHandler(this.panel1_DragDrop);
			this.panel1.DragEnter += new DragEventHandler(this.panel1_DragEnter);
			this.md5label.AutoSize = true;
			this.md5label.Location = new Point(75, 28);
			this.md5label.Name = "md5label";
			this.md5label.Size = new Size(0, 13);
			this.md5label.TabIndex = 5;
			this.label3.AutoSize = true;
			this.label3.Location = new Point(61, 28);
			this.label3.Name = "label3";
			this.label3.Size = new Size(10, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = ":";
			this.label4.AutoSize = true;
			this.label4.Location = new Point(5, 49);
			this.label4.Name = "label4";
			this.label4.Size = new Size(54, 13);
			this.label4.TabIndex = 6;
			this.label4.Text = "File Count";
			this.label5.AutoSize = true;
			this.label5.Location = new Point(61, 49);
			this.label5.Name = "label5";
			this.label5.Size = new Size(10, 13);
			this.label5.TabIndex = 7;
			this.label5.Text = ":";
			this.FileCountLabel.AutoSize = true;
			this.FileCountLabel.Location = new Point(75, 49);
			this.FileCountLabel.Name = "FileCountLabel";
			this.FileCountLabel.Size = new Size(0, 13);
			this.FileCountLabel.TabIndex = 8;
			this.label6.AutoSize = true;
			this.label6.Location = new Point(5, 72);
			this.label6.Name = "label6";
			this.label6.Size = new Size(54, 13);
			this.label6.TabIndex = 9;
			this.label6.Text = "File Name";
			this.label7.AutoSize = true;
			this.label7.Location = new Point(60, 72);
			this.label7.Name = "label7";
			this.label7.Size = new Size(10, 13);
			this.label7.TabIndex = 10;
			this.label7.Text = ":";
			this.FileNameLabel.AutoSize = true;
			this.FileNameLabel.Location = new Point(75, 72);
			this.FileNameLabel.Name = "FileNameLabel";
			this.FileNameLabel.Size = new Size(0, 13);
			this.FileNameLabel.TabIndex = 11;
			this.label8.AutoSize = true;
			this.label8.Location = new Point(7, 90);
			this.label8.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
			this.label8.ForeColor = System.Drawing.Color.DimGray;
			this.label8.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.label8.Enabled = false;
			this.label8.Name = "label8";
			this.label8.Size = new Size(281, 26);
			this.label8.TabIndex = 12;
			this.label8.Text = "Depending on the size of the file,\r\n may take longer to compute the hash";
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(319, 121);
			base.Controls.Add(this.panel1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
			base.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
			base.Name = "Form1";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "PAK Md5 & Text Generator";
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
