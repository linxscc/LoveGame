namespace Assets.Scripts.Module.Download
{
    public interface IDownloadItem
    {
        long Size { get; set; }
        string Path { get; set; }
        string Md5 { get; set; }
        FileType FileType { get; set; }
    }
}