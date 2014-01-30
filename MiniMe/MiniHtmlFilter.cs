using System;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using MiniMe.Common;

namespace MiniMe
{
    internal class MiniHtmlFilter : Stream
    {
        private readonly Stream _responseStream;
        private static readonly Regex CloseBodyRegex = new Regex("</body>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex CloseTitleRegex = new Regex("</title>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex CloseHeadRegex = new Regex("</head>", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public MiniHtmlFilter(Stream responseStream)
        {
            _responseStream = responseStream;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _responseStream.Read(buffer, offset, count);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            string html = HttpContext.Current.Response.ContentEncoding.GetString(buffer, offset, count);

            if(CloseTitleRegex.IsMatch(html) || CloseHeadRegex.IsMatch(html) || CloseBodyRegex.IsMatch(html))
            {
                string jsPath = "/Scripts/Site_#.min.js";
                string cssPath = "/Content/Site_#.css";
                var configSection = ConfigurationManager.GetSection("miniMeAppSettingsGroup/miniMeAppSettings") as Configuration.MiniMeConfigurationSection;

                if (configSection != null)
                {
                    if (!string.IsNullOrEmpty(configSection.MiniJsRelativePath)) jsPath = configSection.MiniJsRelativePath;
                    if (!string.IsNullOrEmpty(configSection.MiniCssRelativePath)) cssPath = configSection.MiniCssRelativePath;
                }

                html = CloseBodyRegex.Replace(html, x => "    " + ((ICanRenderCdnBuilder)Reference.Cdn).Render() + ((ICanRenderBuilder)Reference.JavaScript).Render(jsPath) + ((ICanRenderBuilder)Reference.CoffeeScript).Render(jsPath) + Environment.NewLine + "</body>", 1);

                bool closeTitleFound = false;

				string cssMarkup = ((ICanRenderBuilder)Reference.StyleSheet).Render(cssPath) + ((ICanRenderBuilder)Reference.Scss).Render(cssPath) + ((ICanRenderBuilder)Reference.Less).Render(cssPath);

                html = CloseTitleRegex.Replace(html, x =>
                {
                    closeTitleFound = true;
                    return "</title>" + Environment.NewLine + "    " + cssMarkup;
                }, 1);

                if (!closeTitleFound)
                {
                    html = CloseHeadRegex.Replace(html, x => "    " + cssMarkup + Environment.NewLine + "</head>", 1);
                }

                buffer = HttpContext.Current.Response.ContentEncoding.GetBytes(html);
                _responseStream.Write(buffer, 0, buffer.Length);
            }
            else
            {
                _responseStream.Write(buffer, 0, buffer.Length);
            }
        }

        #region "Not Implemented members"
        public override bool CanRead
        {
            get { return HttpContext.Current.Response.ContentType.Equals("text/html", StringComparison.OrdinalIgnoreCase); }
        }

        public override bool CanSeek
        {
            get { return HttpContext.Current.Response.ContentType.Equals("text/html", StringComparison.OrdinalIgnoreCase); }
        }

        public override bool CanWrite
        {
            get { return HttpContext.Current.Response.ContentType.Equals("text/html", StringComparison.OrdinalIgnoreCase); }
        }

        public override long Length
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public override long Position
        {
            get
            {
                return 0; //throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                //throw new Exception("The method or operation is not implemented.");
            }
        }
        public override long Seek(long offset, SeekOrigin origin)
        {
            return 0;  // throw new Exception("The method or operation is not implemented.");
        }

        public override void SetLength(long value)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void Flush()
        {

        }
        #endregion
    }
}