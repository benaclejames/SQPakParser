using System;

namespace SQPakParser
{
    public class FolderSegmentItem
    {
        public int FolderIDHash, FilesOffset, TotalFilesSize;
        public SqSegmentArray<FileSegmentItem> fileSeg;

        public FolderSegmentItem(byte[] fileBytes)
        {
            FolderIDHash = BitConverter.ToInt32(fileBytes, 0);
            FilesOffset = BitConverter.ToInt32(fileBytes, 4);
            TotalFilesSize = BitConverter.ToInt32(fileBytes, 8);

            byte[] fileSegItemBytes = new byte[TotalFilesSize];
            SQParserMain.FileBytes.CopyTo(FilesOffset, fileSegItemBytes, 0, TotalFilesSize);
            //Array.Copy(Program.fileBytes.toa, FilesOffset, fileSegItemBytes, 0, TotalFilesSize);
            fileSeg = new SqSegmentArray<FileSegmentItem>(fileSegItemBytes);
            
            // Ensure the amount of files parsed matches the expected
            var expectedFiles = TotalFilesSize / 16;
            if (fileSeg.Elems[expectedFiles-1] == null)
                throw new Exception("FileSegmentItem count mismatch");
        }
    }
}