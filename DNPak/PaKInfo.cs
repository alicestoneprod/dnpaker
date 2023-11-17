namespace DnPaker
{
    using System;
    using System.Runtime.CompilerServices;

    internal class PaKInfo
    {
        public PaKInfo()
        {
        }

        public PaKInfo(int filecount, string folder, string[] Paths, int[] ZipSizes, int[] OutSizes, int[] Offsets)
        {
            this.Folder = folder;
            this.FilePaths = Paths;
            this.FileZipSizes = ZipSizes;
            this.FileOutSizes = OutSizes;
            this.FileOffsets = Offsets;
            this.FilesCount = filecount;
        }

        public string[] FilePaths { get; set; }

        public int[] FileZipSizes { get; set; }

        public int[] FileOutSizes { get; set; }

        public int[] FileOffsets { get; set; }

        public int FilesCount { get; set; }

        public string Folder { get; set; }
    }
}

