using System;
using System.Linq;

namespace SQPakParser
{
    public class SqSegmentArray<T>
    {
        public readonly T[] Elems;
        
        public SqSegmentArray(byte[] segmentBytes)
        {
            Elems = new T[segmentBytes.Length / 16];
            // For every 16 bytes in this segment, create a new T object and pass in the 16 bytes
            int iters = 0;
            for (int i = 0; i < segmentBytes.Length; i += 16)
            {
                byte[] newArray = new byte[16];
                Array.Copy(segmentBytes, i, newArray, 0, 16);
                Elems[iters] = (T)Activator.CreateInstance(typeof(T), newArray);
                iters++;
            }
        }
    }
}