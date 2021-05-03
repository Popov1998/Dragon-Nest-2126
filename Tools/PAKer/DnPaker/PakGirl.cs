using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;
using zlib;

namespace DnPaker
{
	internal class PakGirl
	{
		private static Encoding DEncoding = Encoding.Default;

		public static char[] chars;

		public static int File2Pak(string FolderPath, BackgroundWorker backgroundWorker1)
		{
			string text;
			string path;
			if (Path.GetFileName(FolderPath) == "resource")
			{
				text = Path.GetDirectoryName(FolderPath);
				DateTime now = DateTime.Now;
				path = text + "\\Resource-" + now.DayOfYear + now.Hour + now.Minute + now.Second + ".pak";
			}
			else
			{
				if (Directory.GetDirectories(FolderPath, "resource").Length != 1 && Directory.GetDirectories(FolderPath, "mapdata").Length != 1)
				{
					MessageBox.Show("Directory structure is wrong");
					return 0;
				}
				text = FolderPath;
				path = FolderPath + ".pak";
			}
			string[] files = Directory.GetFiles(FolderPath, "*", SearchOption.AllDirectories);
			backgroundWorker1.ReportProgress(2, "The documents to be packaged are :" + files.Length + "in total");
			PaKInfo paKInfo = new PaKInfo();
			paKInfo.FilePaths = new string[files.Length];
			paKInfo.FileOutSizes = new int[files.Length];
			paKInfo.FileZipSizes = new int[files.Length];
			paKInfo.FileOffsets = new int[files.Length];
			FileStream fileStream = File.Create(path);
			BinaryWriter binaryWriter = new BinaryWriter(fileStream, DEncoding);
			binaryWriter.Write(WriteString("EyedentityGames Packing File 0.1"));
			binaryWriter.Seek(224, SeekOrigin.Current);
			binaryWriter.Write(11);
			binaryWriter.Write(files.Length);
			binaryWriter.Write(0);
			binaryWriter.Seek(756, SeekOrigin.Current);
			backgroundWorker1.ReportProgress(5, "Packaging.....");
			for (int i = 0; i < files.Length; i++)
			{
				paKInfo.FileOffsets[i] = (int)fileStream.Position;
				byte[] array = compressFile(files[i], out paKInfo.FileOutSizes[i]);
				binaryWriter.Write(array);
				paKInfo.FileZipSizes[i] = array.Length;
				paKInfo.FilePaths[i] = files[i].Replace(text, "");
				backgroundWorker1.ReportProgress(i * 80 / files.Length + 5, "Packaging:" + i + ":" + files.Length + "\r\n" + files[i]);
			}
			binaryWriter.Seek(264, SeekOrigin.Begin);
			binaryWriter.Write(binaryWriter.BaseStream.Length);
			binaryWriter.Seek(0, SeekOrigin.End);
			for (int i = 0; i < files.Length; i++)
			{
				long position = binaryWriter.BaseStream.Position;
				binaryWriter.Write(WriteString(paKInfo.FilePaths[i]));
				binaryWriter.Seek(256 - paKInfo.FilePaths[i].Length, SeekOrigin.Current);
				binaryWriter.Write(paKInfo.FileZipSizes[i]);
				binaryWriter.Write(paKInfo.FileOutSizes[i]);
				binaryWriter.Write(paKInfo.FileZipSizes[i]);
				binaryWriter.Write(paKInfo.FileOffsets[i]);
				binaryWriter.Write(WriteNull(44));
				backgroundWorker1.ReportProgress(i * 10 / files.Length + 85, "Organizing files:" + i + ":" + files.Length);
			}
			paKInfo.FileZipSizes = null;
			paKInfo.FileOutSizes = null;
			paKInfo.FilePaths = null;
			paKInfo.FileOffsets = null;
			paKInfo = null;
			fileStream.Flush();
			fileStream.Close();
			binaryWriter.Close();
			backgroundWorker1.ReportProgress(100, "Totally packaged files: " + files.Length + "in total");
			return files.Length;
		}

		public static int Pak2File(string PakPath, BackgroundWorker backgroundWorker1)
		{
			try
			{
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(PakPath);
				string text = Path.GetDirectoryName(PakPath) + "\\" + fileNameWithoutExtension;
				FileStream fileStream = new FileStream(PakPath, FileMode.Open);
				if (fileStream.Length < 1024)
				{
					throw new Exception("File size error!");
				}
				BinaryReader binaryReader = new BinaryReader(fileStream);
				backgroundWorker1.ReportProgress(2, "load: " + fileNameWithoutExtension);
				byte[] array = new byte[256];
				fileStream.Seek(0L, SeekOrigin.Begin);
				fileStream.Read(array, 0, 256);
				string text2 = clear(DEncoding.GetString(array).ToString());
				if (text2 != "EyedentityGames Packing File 0.1")
				{
					throw new Exception("The file type is wrong!");
				}
				binaryReader.BaseStream.Seek(4L, SeekOrigin.Current);
				int num = binaryReader.ReadInt32();
				int num2 = binaryReader.ReadInt32();
				fileStream.Seek(num2, SeekOrigin.Begin);
				backgroundWorker1.ReportProgress(5, "Found files:" + num + "in total");
				long position = fileStream.Position;
				string text3;
				for (int i = 0; i < num; i++)
				{
					fileStream.Seek(position, SeekOrigin.Begin);
					fileStream.Read(array, 0, 256);
					text3 = clear(DEncoding.GetString(array));
					int num3 = binaryReader.ReadInt32();
					int num4 = binaryReader.ReadInt32();
					fileStream.Seek(4L, SeekOrigin.Current);
					int num5 = binaryReader.ReadInt32();
					fileStream.Seek(44L, SeekOrigin.Current);
					position = fileStream.Position;
					byte[] array2 = new byte[num3];
					fileStream.Seek(num5, SeekOrigin.Begin);
					fileStream.Read(array2, 0, num3);
					string outFile = text + "/" + text3;
					decompressFile(array2, outFile);
					int percentProgress = 5 + i * 90 / num;
					backgroundWorker1.ReportProgress(percentProgress, "Unpack: " + i + "/" + num + "\r\n" + text3);
				}
				array = null;
				text3 = null;
				binaryReader.Close();
				fileStream.Close();
				fileStream.Dispose();
				fileStream = null;
				backgroundWorker1.ReportProgress(100, "Unpacking is complete! The Pak solves a total of files: " + num + "in total");
				return num;
			}
			catch (Exception ex)
			{
				MessageBox.Show(PakPath + ":" + ex.Message);
				backgroundWorker1.ReportProgress(100, "Unpacking error," + ex.Message + "：\r\n" + PakPath);
				backgroundWorker1.Dispose();
				backgroundWorker1.CancelAsync();
				return 0;
			}
		}

		public static void CopyStream(Stream input, Stream output)
		{
			byte[] buffer = new byte[2000];
			int count;
			while ((count = input.Read(buffer, 0, 2000)) > 0)
			{
				output.Write(buffer, 0, count);
			}
			output.Flush();
		}

		private static void decompressFile(byte[] inBytes, string outFile)
		{
			MemoryStream memoryStream = new MemoryStream(inBytes);
			string directoryName = Path.GetDirectoryName(outFile);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			FileStream fileStream = new FileStream(outFile, FileMode.Create);
			ZOutputStream zOutputStream = new ZOutputStream(fileStream);
			try
			{
				CopyStream(memoryStream, zOutputStream);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + "\r\nUnPacked: " + outFile);
			}
			finally
			{
				zOutputStream.Close();
				fileStream.Close();
				memoryStream.Close();
			}
		}

		private static byte[] compressFile(string inFile, out int fileLength)
		{
			MemoryStream memoryStream = new MemoryStream();
			ZOutputStream zOutputStream = new ZOutputStream(memoryStream, 1);
			FileStream fileStream = new FileStream(inFile, FileMode.Open);
			fileLength = (int)fileStream.Length;
			try
			{
				CopyStream(fileStream, zOutputStream);
				zOutputStream.Close();
				fileStream.Close();
				byte[] result = memoryStream.ToArray();
				memoryStream.Close();
				return result;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				return null;
			}
			finally
			{
				fileStream.Close();
			}
		}

		private static string clear(string str)
		{
			int num = str.IndexOf(Convert.ToChar((byte)0));
			if (num > 0)
			{
				return str.Remove(num);
			}
			return str;
		}

		private static byte[] WriteNull(int length)
		{
			byte[] array = new byte[length];
			for (int i = 0; i < length; i++)
			{
				array.SetValue((byte)0, i);
			}
			return array;
		}

		private static byte[] WriteString(string Num)
		{
			return DEncoding.GetBytes(Num);
		}

		static PakGirl()
		{
			Encoding dEncoding = DEncoding;
			byte[] bytes = new byte[1];
			chars = dEncoding.GetChars(bytes);
		}
	}
}
