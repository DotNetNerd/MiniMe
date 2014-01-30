using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using MiniMe.Common;

namespace MiniMe.Less
{
    internal class MiniLessBuilder : BaseReferenceBuilder, IMiniLessBuilder
    {
        internal override string HttpKey { get { return "__less__"; } }
        
        public MiniLessBuilder(){}

        public MiniLessBuilder(IMini mini): base(mini){}

		internal MiniLessBuilder(HttpContextBase httpContext, IMini mini, IFileSystem fileSystem)
            : base(httpContext, mini, fileSystem){}

        protected override string GenerateReferenceMarkup(string relativeFilePath)
        {

			return string.Format("<link href=\"{0}\" rel=\"stylesheet\" />", Regex.Replace(relativeFilePath, "\\.css$", ".less", RegexOptions.IgnoreCase));
        }

        protected override FileInfo[] MapToFileInfo(List<string> sourceFiles)
        {
            return FileSystem.GetExistingFiles(sourceFiles.Select(s => Context.Server.MapPath(Regex.Replace(s, "\\.css$", ".less", RegexOptions.IgnoreCase))));
        }

        protected override string MapRenderedServerPath(string relativeFilePath)
        {
            return Regex.Replace(Context.Server.MapPath(relativeFilePath.Split('?')[0]), "\\.css$", ".less", RegexOptions.IgnoreCase);
        }

        private static readonly object RenderLockInstance = new object();
        protected override object RenderLock
        {
            get { return RenderLockInstance; }
        }
    }
}