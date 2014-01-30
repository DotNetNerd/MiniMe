using System.IO;
using Microsoft.Ajax.Utilities;
using MiniMe.Common;

namespace MiniMe
{
    public class Mini : IMini
    {
        private readonly Minifier _mini = new Minifier();
        private readonly IFileSystem _fileSystem;

        public Mini() : this(new FileSystemWrapper()){}

        internal Mini(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public string MinifyJavaScriptFromPath(string path)
        {
            return MinifyJavaScript(_fileSystem.ReadAllText(path));
        }

        public string MinifyJavaScript(string file)
        {
            return _mini.MinifyJavaScript(file);
        }

        public string MinifyStyleSheetFromPath(string path)
        {
            return MinifyStyleSheet(_fileSystem.ReadAllText(path));
        }

        public string MinifyStyleSheet(string file)
        {
            return _mini.MinifyStyleSheet(file);
        }
    }
}