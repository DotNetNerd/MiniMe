using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MiniMe.Common
{
    public class FileSystemWrapper : IFileSystem
    {
        public string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }

        public void WriteAllText(string path, string contents)
        {
            File.WriteAllText(path, contents);
        }

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public FileInfo[] GetExistingFiles(IEnumerable<string> paths)
        {
            if (paths == null)
                return new FileInfo[0];

            return paths.Select(x => new FileInfo(x)).Where(x => x.Exists).ToArray();
        }

        public void CheckCreateDirectory(DirectoryInfo directory)
        {
            if (directory == null) throw new ArgumentNullException("directory");

            if (!directory.Exists)
                directory.Create();
        }
    }
}