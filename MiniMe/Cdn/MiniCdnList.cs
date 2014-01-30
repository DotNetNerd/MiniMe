using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using MiniMe.Common;

namespace MiniMe.Cdn
{
    public class MiniCdnList : IMiniCdnList, ICanRenderCdnBuilder 
    {
        private static string HttpKey { get { return "__cdn__"; } }
        private readonly HttpContextBase _httpContext;

        public MiniCdnList()
            : this(new HttpContextWrapper(HttpContext.Current))
        {
        }

        internal MiniCdnList(HttpContextBase httpContext)
        {
            _httpContext = httpContext;
        }

        public ICanRenderCdnBuilder Add(string filePath, string cdnFilePath)
        {
            return Add(filePath, cdnFilePath, 0);
        }

        public ICanRenderCdnBuilder Add(string filePath, string cdnFilePath, int groupIndex)
        {
            if (string.IsNullOrEmpty(filePath) && string.IsNullOrEmpty(cdnFilePath)) throw new ArgumentException("Both paths cannot be empty");
            var list = GetRequestList();
            list.Add(new CdnItem(filePath, cdnFilePath, groupIndex));
            return this;
        }

        public string Render()
        {
            var result = Render(GetRequestList().OrderBy(i => i.Index));
            GetRequestList().Clear();
            return result;
        }

        private IList<CdnItem> GetRequestList()
        {

            if (_httpContext.Items[HttpKey] == null) _httpContext.Items[HttpKey] = new List<CdnItem>();
            return (List<CdnItem>)_httpContext.Items[HttpKey];
        }

        private string Render(IEnumerable<CdnItem> files)
        {
            if (files == null || !files.Any()) return string.Empty;
            var sb = new StringBuilder();
            if (_httpContext.Request.IsLocal || _httpContext.IsDebuggingEnabled)
            {
                foreach (var file in files) sb.Append(string.Format("<script src=\"" + (string.IsNullOrEmpty(file.Path) ? file.CdnPath : file.Path) + "\"></script>\r\n    "));
                return sb.ToString();
            }

            foreach (var file in files) sb.Append(string.Format("<script src=\"" + (string.IsNullOrEmpty(file.CdnPath) ? file.Path : file.CdnPath) + "\"></script>\r\n    "));
            return sb.ToString();
        }
    }
}