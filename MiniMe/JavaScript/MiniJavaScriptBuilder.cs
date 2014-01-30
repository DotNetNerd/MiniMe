using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using MiniMe.Common;

namespace MiniMe.JavaScript
{
    internal class MiniJavaScriptBuilder : BaseReferenceBuilder, IMiniJavaScriptBuilder
    {
        internal override string HttpKey { get { return "__javascript__"; } }

        public MiniJavaScriptBuilder() { }

        public MiniJavaScriptBuilder(IMini mini) : base(mini) { }

        internal MiniJavaScriptBuilder(HttpContextBase httpContext, IMini mini, IFileSystem fileSystem, ICache cache)
            : base(httpContext, mini, fileSystem, cache) { }

        private static bool IsAlreadyMinifiedVersion(string file)
        {
            return Regex.IsMatch(file, "\\.min.js$", RegexOptions.IgnoreCase);
        }

        private bool IfDebugVersionExists(string fullpath)
        {
            return FileSystem.FileExists(Regex.Replace(fullpath, "\\.js$", ".debug.js", RegexOptions.IgnoreCase));
        }

        private static string FileNameForMinifiedVersion(string fullpath)
        {
            return Regex.Replace(fullpath, "\\.js$", ".min.js", RegexOptions.IgnoreCase);
        }

        protected override string GenerateReferenceMarkup(string relativeFilePath)
        {
            return string.Format("<script src=\"{0}\"></script>", relativeFilePath);
        }

        protected override string Minify(FileInfo[] files)
        {
            var sb = new StringBuilder();

            foreach (FileInfo file in files)
            {
                var text = string.Empty;
                if (IsAlreadyMinifiedVersion(file.Name) || IfDebugVersionExists(file.Name))
                {
                    text = FileSystem.ReadAllText(file.FullName);
                }
                else
                {
                    var fileWithMin = FileNameForMinifiedVersion(file.FullName);
                    text = FileSystem.FileExists(fileWithMin) ? FileSystem.ReadAllText(fileWithMin) : Mini.MinifyJavaScriptFromPath(file.FullName);
                }

                sb.AppendLine(text + ";");
            }

            return sb.ToString();
        }

        private static readonly object RenderLockInstance = new object();
        protected override object RenderLock
        {
            get { return RenderLockInstance; }
        }
    }
}