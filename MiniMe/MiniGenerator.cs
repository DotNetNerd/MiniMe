using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MiniMe
{
    public class MiniGenerator : IMiniGenerator
    {
        public void EnsureMiniVersions(string path)
        {
           EnsureMiniVersionsFromFullPath(HttpContext.Current.Server.MapPath(path));
        }

        public void EnsureMiniVersionsFromFullPath(string path)
        {
            var fullPath = path;
            var files = Directory.GetFiles(fullPath).Where(f => f.ToLower().EndsWith(".js") && !f.ToLower().EndsWith("-vsdoc.js"));
            var mini = new Mini();

            foreach (var file in files.Where(f => !Regex.IsMatch(f, "\\.min\\.js$", RegexOptions.IgnoreCase)))
            {
                var fileWithoutDebug = Regex.Replace(file, "\\.debug\\.js$", ".js", RegexOptions.IgnoreCase);
                var fileWithMin = Regex.Replace(file, "\\.js$", ".min.js", RegexOptions.IgnoreCase);
                var fileWithDebug = Regex.Replace(file, "\\.js$", ".debug.js", RegexOptions.IgnoreCase);

                if (Regex.IsMatch(file, "\\.debug.js$", RegexOptions.IgnoreCase) && !files.Contains(fileWithoutDebug))
                {
                    File.WriteAllText(fileWithoutDebug, mini.MinifyJavaScriptFromPath(file));
                }
                else if (!Regex.IsMatch(file, "\\.debug.js$", RegexOptions.IgnoreCase) && !files.Contains(fileWithMin) && !files.Contains(fileWithDebug))
                {
                    File.WriteAllText(fileWithMin, mini.MinifyJavaScriptFromPath(file));
                }
            }

            foreach (var dir in Directory.GetDirectories(fullPath)) EnsureMiniVersionsFromFullPath(dir);
        }
    }
}
