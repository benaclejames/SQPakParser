using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SQPakParser
{
    internal static class SQParserMain
    {
        public static List<byte> FileBytes;
        
        public static void Main(string[] args)
        {
            if (!Directory.Exists(args[0]))
                throw new Exception("File path does not exist");
            
            // Load all bytes from specified path
            FileBytes = File.ReadAllBytes(args[0]).ToList();
            
            // Create a new SQHeader
            var sqHeader = new SQHeader(FileBytes.Take(0x400).ToArray());

            if (sqHeader.FileType != FileType.Index)
                throw new Exception("Only Index files are currently supported!");
            
            // We can now discard the first 0x400 bytes
            var fileBytesWithoutHeader = FileBytes.Skip(0x400).ToArray();
            
            // Get length of segments headers
            var segHeadersLength = BitConverter.ToInt32(fileBytesWithoutHeader, 0);
            
            // Get all bytes after this, skipping the length of the segment headers
            var fileBytesWithoutSegHeaders = fileBytesWithoutHeader.Skip(4).Take(segHeadersLength).ToArray();

            var fileSeg = new SQSegmentHeader(ref fileBytesWithoutSegHeaders, true);
            var unknown1Seg = new SQSegmentHeader(ref fileBytesWithoutSegHeaders);
            var unknown2Seg = new SQSegmentHeader(ref fileBytesWithoutSegHeaders);
            var folderSeg = new SQSegmentHeader(ref fileBytesWithoutSegHeaders);

            var folderSegActual = new SqSegmentArray<FolderSegmentItem>(FileBytes.Skip(folderSeg.SegOffset).Take(folderSeg.SegSize).ToArray());

            Console.WriteLine("Finished reading file of type: "+sqHeader.FileType);
            Console.WriteLine("We have {0} folders with {1} files combined.", folderSegActual.Elems.Length, folderSegActual.Elems.Sum(x => x.TotalFilesSize/16));
            Console.WriteLine($"These are contained within {unknown1Seg.NumDats} dat file(s).");
        }
    }
}