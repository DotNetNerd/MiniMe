namespace MiniMe.Common
{
    public class Item
    {
        public Item(string path, int index)
        {
            Path = path;
            Index = index;
        }
        public string Path { get; set; }
        public int Index { get; set; }
    }
}