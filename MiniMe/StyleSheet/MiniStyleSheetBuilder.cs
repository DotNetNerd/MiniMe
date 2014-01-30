using System;
using System.IO;
using System.Text;
using System.Web;
using MiniMe.Common;

namespace MiniMe.StyleSheet
{
    internal class MiniStyleSheetBuilder : BaseReferenceBuilder, IMiniStyleSheetBuilder 
    {
        internal override string HttpKey { get { return "__stylesheet__"; } }

        public MiniStyleSheetBuilder(){}

        public MiniStyleSheetBuilder(IMini mini): base(mini){}

        internal MiniStyleSheetBuilder(HttpContextBase httpContext, IMini mini, IFileSystem fileSystem): base(httpContext, mini, fileSystem){}

        protected override string Minify(FileInfo[] files)
        {
            var sb = new StringBuilder();

            foreach (FileInfo file in files)
                sb.Append(FileSystem.ReadAllText(file.FullName));

            return Mini.MinifyStyleSheet(sb.ToString());
        }

        protected override string GenerateReferenceMarkup(string relativeFilePath)
        {
            return string.Format("<link href=\"{0}\" rel=\"stylesheet\" />", relativeFilePath);
        }

        private static readonly object RenderLockInstance = new object();
        protected override object RenderLock
        {
            get { return RenderLockInstance; }
        }
    }
}