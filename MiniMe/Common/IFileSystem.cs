using System.Collections.Generic;
using System.IO;

namespace MiniMe.Common
{
    public interface IFileSystem
    {
        string ReadAllText(string path);
        void WriteAllText(string path, string contents);
        bool FileExists(string path);

        FileInfo[] GetExistingFiles(IEnumerable<string> paths);
        void CheckCreateDirectory(DirectoryInfo directory);
    }
}