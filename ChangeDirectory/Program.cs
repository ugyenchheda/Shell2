using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ChangeDirectory
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                return;
            }
            else
            {
                MemoryMappedFile mmf = MemoryMappedFile.CreateOrOpen("sharedVar", 1024, MemoryMappedFileAccess.ReadWrite);
                try
                {
                    Directory.SetCurrentDirectory(args[0]);
                    //TODO: test print -- to be removed in final version or put behind a flag
                    Console.WriteLine(Directory.GetCurrentDirectory());
                    //Insert current directory to MemoryMappedFile
                    MemoryMappedViewStream mmvStream = mmf.CreateViewStream(0, 1024);
                    //Create binary formatter to serialize the string and write it to stream
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(mmvStream, Directory.GetCurrentDirectory());
                    return;
                }
                catch (Exception e)
                {
                    if (e is System.IO.DirectoryNotFoundException)
                    {
                        Console.WriteLine("Directory not found");
                        return;
                    }
                    else
                    {
                        Console.WriteLine(e);
                        return;
                    }
                }
            }
        }
    }
}
