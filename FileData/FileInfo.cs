namespace FileData
{
    public class FileInfo
    {
        public string Path { get; protected set; }
        public bool IsModified { get; set; }
        public FileInfo() { }
        public FileInfo(string path)
        {
            Path = path;
        }
        public void Clear()
        {
            Path = "";
        }
    }
}
