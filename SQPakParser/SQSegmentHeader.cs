using System;
using System.Linq;

namespace SQPakParser
{
    public class SQSegmentHeader
    {
        public int NumDats, SegOffset, SegSize;
        public byte[] SHA1;
        
        public SQSegmentHeader(ref byte[] segBytes, bool isFirst = false)
        {
            // Read the num dats as an int32
            NumDats = BitConverter.ToInt32(segBytes, 0);
            segBytes = segBytes.Skip(4).ToArray();
            
            // Read the segment offset as an int32
            SegOffset = BitConverter.ToInt32(segBytes, 0);
            segBytes = segBytes.Skip(4).ToArray();
            
            // Read the segment size as an int32
            SegSize = BitConverter.ToInt32(segBytes, 0);
            segBytes = segBytes.Skip(4).ToArray();
            
            // Read the SHA1 hash as a 20 byte array
            SHA1 = segBytes.Take(20).ToArray();
            segBytes = segBytes.Skip(20).ToArray();
            
            segBytes = segBytes.Skip(isFirst ? 44 : 40).ToArray();
        }
    }
}