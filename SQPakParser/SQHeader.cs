using System;
using System.Linq;
using System.Text;

namespace SQPakParser
{
    public enum FileType
    {
        SQDB,
        Data,
        Index
    }
    
    public class SQHeader
    {
        public FileType FileType;
        public byte[] SHA1;
        
        public SQHeader(byte[] headerBytes)
        {
            // Ensure we only get 0x400 bytes
            if (headerBytes.Length != 0x400)
                throw new ArgumentException("Header must be 0x400 bytes long");

            // Convert the 16 bytes to a string
            var indexString = Encoding.ASCII.GetString(headerBytes.Take(12).ToArray());
            
            // Ensure the string begins with SqPack
            if (!indexString.StartsWith("SqPack"))
            {
                throw new Exception("Invalid index file! Are you sure this is a SQPak file?");
            }
            
            // Get the next 4 bytes that correspond to the header length
            var headerLength = BitConverter.ToInt32(headerBytes.Skip(12).Take(4).ToArray(), 0);
            
            // Ensure the header length is 0x400
            if (headerLength != 0x400)
            {
                throw new Exception("Invalid index file. Unexpected header length.");
            }
            
            // Get the filetype by skipping 4 bytes and then reading 4 as an int
            FileType = (FileType)BitConverter.ToInt32(headerBytes.Skip(20).Take(4).ToArray(), 0);

            // Skip to 0x3c0 and read 20 bytes. This is the SHA-1 of the header
            SHA1 = headerBytes.Skip(0x3c0).Take(20).ToArray();
        }
    }
}