namespace MiniMe.Common
{
    public class CdnItem
    {
        public CdnItem(string path, string cdnPath, int index)
        {
            Path = path;
            CdnPath = cdnPath;
            Index = index;
        }
        public string Path { get; set; }
        public string CdnPath { get; set; }
        public int Index { get; set; }
    }
}