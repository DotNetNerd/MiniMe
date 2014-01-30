namespace MiniMe
{
    public interface IMini
    {
        string MinifyJavaScriptFromPath(string path);
        string MinifyJavaScript(string file);
        string MinifyStyleSheetFromPath(string path);
        string MinifyStyleSheet(string file);
    }
}