using System;

namespace SQPakParser
{
    public class FileSegmentItem
    {
        public int FileID1Hash, FileID2Hash, FileDataOffset;
        
        public FileSegmentItem(byte[] fileSegItemBytes)
        {
            FileID1Hash = BitConverter.ToInt32(fileSegItemBytes, 0);
            FileID2Hash = BitConverter.ToInt32(fileSegItemBytes, 4);
            FileDataOffset = BitConverter.ToInt32(fileSegItemBytes, 8) * 0x08;
        }
    }
}