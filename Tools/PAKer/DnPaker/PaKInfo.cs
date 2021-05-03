namespace DnPaker
{
	internal class PaKInfo
	{
		public string[] FilePaths { get; set; }

		public int[] FileZipSizes { get; set; }

		public int[] FileOutSizes { get; set; }

		public int[] FileOffsets { get; set; }

		public int FilesCount { get; set; }

		public string Folder { get; set; }

		public PaKInfo()
		{
		}

		public PaKInfo(int filecount, string folder, string[] Paths, int[] ZipSizes, int[] OutSizes, int[] Offsets)
		{
			Folder = folder;
			FilePaths = Paths;
			FileZipSizes = ZipSizes;
			FileOutSizes = OutSizes;
			FileOffsets = Offsets;
			FilesCount = filecount;
		}
	}
}
