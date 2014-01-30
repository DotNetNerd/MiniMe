using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using MiniMe.Common;

namespace MiniMe.CoffeeScript
{
    internal class MiniCoffeeScriptBuilder : BaseReferenceBuilder, IMiniCoffeeScriptBuilder
    {
        internal override string HttpKey { get { return "__coffeescript__"; } }
        
        public MiniCoffeeScriptBuilder(){}

        public MiniCoffeeScriptBuilder(IMini mini): base(mini){}

        internal MiniCoffeeScriptBuilder(HttpContextBase httpContext, IMini mini, IFileSystem fileSystem)
            : base(httpContext, mini, fileSystem){}

        protected override string GenerateReferenceMarkup(string relativeFilePath)
        {
            return string.Format("<script src=\"{0}\"></script>", RunInDebug() ? relativeFilePath : Regex.Replace(relativeFilePath, "(\\.min)?\\.js$", ".min.js", RegexOptions.IgnoreCase));
        }

        protected override FileInfo[] MapToFileInfo(List<string> sourceFiles)
        {
            return FileSystem.GetExistingFiles(sourceFiles.Select(s => Context.Server.MapPath(Regex.Replace(s, "\\.js$", ".coffee", RegexOptions.IgnoreCase))));
        }

        protected override string MapRenderedServerPath(string relativeFilePath)
        {
            return Regex.Replace(Context.Server.MapPath(relativeFilePath.Split('?')[0]), "(\\.min)?\\.js$", ".coffee", RegexOptions.IgnoreCase);
        }

        private static readonly object RenderLockInstance = new object();
        protected override object RenderLock
        {
            get { return RenderLockInstance; }
        }
    }
}