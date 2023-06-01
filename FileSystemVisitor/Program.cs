using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
namespace FileSystem
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Console.WriteLine("Input path: ");
            string path = Console.ReadLine();
            Console.WriteLine("Input filter: ");
            string filt = Console.ReadLine();
            FileSystemVisitor visitor = new FileSystemVisitor(@path);
            visitor.Started += (o, e) =>  Console.WriteLine("Traversing started ");
            visitor.Finished += (o, e) => Console.WriteLine("Traversing finished ");
            visitor.FileFound += (o, e) => Console.Write("File found ");
            visitor.DirectoryFound += (o, e) =>
            {
                e.StopSearch = e.FileName.EndsWith("44sql");
                e.ExcludeFinded = e.FileName.EndsWith("44sql");
                if (!e.ExcludeFinded && !e.StopSearch)
                    Console.Write("Directory found ");
            };
            foreach (var item in visitor)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("Filter");
            Predicate<string> filter = name => name.EndsWith(filt);
            visitor = new FileSystemVisitor(@path, filter);
            visitor.Started += (o, e) => Console.WriteLine("Traversing started ");
            visitor.Finished += (o, e) => Console.WriteLine("Traversing finished ");
            visitor.FileFound += (o, e) => Console.Write("File found ");
            visitor.FilteredFileFound += (o, e) =>
            {
                e.StopSearch = e.FileName.EndsWith("~sql");
                e.ExcludeFinded = e.FileName.EndsWith("~sql");
                Console.Write("Filtered file found ");
            }; 
            visitor.DirectoryFound += (o,  e) => Console.Write("Directory found ");
            visitor.FilteredDirectoryFound += (o, e) =>
            {
                e.StopSearch = e.FileName.EndsWith(".sql");
                e.ExcludeFinded = e.FileName.EndsWith(".sql");
                if (!e.ExcludeFinded&&!e.StopSearch)
                    Console.Write("Filtered directory found "); 
            };
            
            foreach (var item in visitor)
            {
                Console.WriteLine(item);
            }
        }
    }
}

