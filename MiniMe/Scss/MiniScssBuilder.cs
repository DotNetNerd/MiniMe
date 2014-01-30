using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using MiniMe.Common;

namespace MiniMe.Scss
{
    internal class MiniScssBuilder : BaseReferenceBuilder, IMiniScssBuilder
    {
        internal override string HttpKey { get { return "__scss__"; } }
        
        public MiniScssBuilder(){}

        public MiniScssBuilder(IMini mini): base(mini){}

        internal MiniScssBuilder(HttpContextBase httpContext, IMini mini, IFileSystem fileSystem)
            : base(httpContext, mini, fileSystem){}

        protected override string GenerateReferenceMarkup(string relativeFilePath)
        {
            return string.Format("<link href=\"{0}\" rel=\"stylesheet\" />", relativeFilePath);
        }

        protected override FileInfo[] MapToFileInfo(List<string> sourceFiles)
        {
            return FileSystem.GetExistingFiles(sourceFiles.Select(s => Context.Server.MapPath(Regex.Replace(s, "\\.css$", ".scss", RegexOptions.IgnoreCase))));
        }

        protected override string MapRenderedServerPath(string relativeFilePath)
        {
            return Regex.Replace(Context.Server.MapPath(relativeFilePath.Split('?')[0]), "\\.css$", ".scss", RegexOptions.IgnoreCase);
        }

        private static readonly object RenderLockInstance = new object();
        protected override object RenderLock
        {
            get { return RenderLockInstance; }
        }
    }
}