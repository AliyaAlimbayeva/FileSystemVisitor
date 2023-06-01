using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace FileSystem
{

    public class FileSystemVisitor : IEnumerable<string>
    {
        private string path;

        public Predicate<string> filterAlgorithm;

        public class FilterEventArgs : EventArgs
        {
            public string FileName;
            public bool StopSearch;
            public bool ExcludeFinded;
        }
        public event EventHandler Started;
        public event EventHandler Finished;
        public event EventHandler<FilterEventArgs> FileFound;
        public event EventHandler<FilterEventArgs> DirectoryFound;
        public event EventHandler<FilterEventArgs> FilteredFileFound;
        public event EventHandler<FilterEventArgs> FilteredDirectoryFound;

        public FileSystemVisitor(string folder, Predicate<string> filter = null)
        {
            path = folder;
            filterAlgorithm = filter;
        }


        private IEnumerable<string> GetFiles()
        {
            foreach (var item in Directory.GetFiles(path))
            {
                FilterEventArgs filterArgs = new FilterEventArgs { FileName = item };

                var eventToInvoke = filterAlgorithm == null ? FileFound : FilteredFileFound;

                if (filterAlgorithm == null || filterAlgorithm(item))
                {
                    eventToInvoke?.Invoke(this, filterArgs);
                    if (filterArgs.StopSearch)
                    {
                        Finished?.Invoke(this, new EventArgs());
                        yield break;
                    }
                    yield return filterArgs.ExcludeFinded ? null : item;
                }
            }
        }

        private IEnumerable<string> GetDirectories()
        {
            foreach (var item in Directory.GetDirectories(path))
            {
                FilterEventArgs filterArgs = new FilterEventArgs { FileName = item };
                
                var eventToInvoke = filterAlgorithm == null ? DirectoryFound : FilteredDirectoryFound;
                
                if (filterAlgorithm == null || filterAlgorithm(item))
                {
                    eventToInvoke?.Invoke(this, filterArgs);
                    if (filterArgs.StopSearch)
                    {
                        Finished?.Invoke(this, new EventArgs());
                        yield break;
                    }
                    yield return filterArgs.ExcludeFinded ? null : item;
                }
            }
        }
        private IEnumerable<string> GetFilesAndDirsNames()
        {
            if (!Directory.Exists(path))
            {
                throw new ArgumentException($"{path} is not a valid directory");
            }
            Started?.Invoke(this, new EventArgs());

            foreach (var item in GetFiles())
            {
                yield return item;
            }

            foreach (var item in GetDirectories())
            {
                yield return item;
            }

            Finished?.Invoke(this, new EventArgs());
        }
        public IEnumerator<string> GetEnumerator()
        {
            return GetFilesAndDirsNames().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

