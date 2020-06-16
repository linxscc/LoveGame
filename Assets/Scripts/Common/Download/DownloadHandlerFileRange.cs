using UnityEngine.Networking;
using System.IO;

/// <summary>
///         使用方式:
///         UnityWebRequest unityWebRequest = new UnityWebRequest("url");
///         unityWebRequest.downloadHandler = new DownloadHandlerFileRange("文件保存的路径", unityWebRequest);
///         unityWebRequest.SendWebRequest();
/// </summary>
public class DownloadHandlerFileRange : DownloadHandlerScript
{
    public event System.Action StartDownloadEvent;
    public event System.Action ErrorEvent;

    private string _path;//文件保存的路径
    private FileStream _fileStream;
    private UnityWebRequest _unityWebRequest;
    private long _localFileSize = 0;//本地已经下载的文件的大小
    private long _totalFileSize = 0;//文件的总大小
    private long _curFileSize = 0;//当前的文件大小
    private float _lastTime = 0;//用作下载速度的时间统计
    private float _lastDataSize = 0;//用来作为下载速度的大小统计
    private float _downloadSpeed = 0;//下载速度,单位:Byte/S
    private bool _useRange;
    private long _fileSize;

    /// <summary>
    /// 下载速度,单位:KB/S 保留两位小数
    /// </summary>
    public float Speed => ((int)(_downloadSpeed / 1024 * 100)) / 100.0f;

    /// <summary>
    /// 文件的总大小
    /// </summary>
    public long FileSize => _totalFileSize;

    /// <summary>
    /// 下载进度[0,1]
    /// </summary>
    public float DownloadProgress => GetProgress();

    /// <summary>
    /// 使用1MB的缓存,在补丁2017.2.1p1中对DownloadHandlerScript的优化中,目前最大传入数据量也仅仅是1024*1024,再多也没用
    /// </summary>
    /// <param name="path">文件保存的路径</param>
    /// <param name="request">UnityWebRequest对象,用来获文件大小,设置断点续传的请求头信息</param>
    /// <param name="useRange">断点续传</param>
    /// <param name="fileSize"></param>
    public DownloadHandlerFileRange(string path, UnityWebRequest request, bool useRange = false, long fileSize = -1) : base(new byte[1024 * 1024])
    {
        _fileSize = fileSize;
        _useRange = useRange;
        _path = path;
        FileInfo file = new FileInfo(_path);
        if (file.Directory != null && file.Directory.Exists == false)
        {
            file.Directory.Create();
        }

        _fileStream = new FileStream(_path, useRange ? FileMode.Append : FileMode.Create, FileAccess.Write);
        
        _unityWebRequest = request;
        
        _localFileSize = file.Length;
        _curFileSize = _localFileSize;
        if (useRange)
        {
            _unityWebRequest.SetRequestHeader("Range", "bytes=" + _localFileSize + "-");
        }
    }

    /// <summary>
    /// 下载完成后清理资源
    /// </summary>
    new public void Dispose()
    {
        _downloadSpeed = 0.0f;
        if (_fileStream != null)
        {
            _fileStream.Flush();
            _fileStream.Dispose();
            _fileStream = null;
        }
    }

    /// <summary>
    /// 下载完成后清理资源
    /// </summary>
    protected override void CompleteContent()
    {
        base.CompleteContent();
        _downloadSpeed = 0.0f;
        if (_fileStream != null)
        {
            _fileStream.Flush();
            _fileStream.Dispose();
            _fileStream = null;
        }
    }

    protected override byte[] GetData()
    {
        return null;
    }

    /// <summary>
    /// 得到下载进度
    /// </summary>
    /// <returns></returns>
    protected override float GetProgress()
    {
        return _totalFileSize == 0 ? 0 : ((float)_curFileSize) / _totalFileSize;
    }

    protected override string GetText()
    {
        return null;
    }

    //Note:当下载的文件数据大于2G时,该int类型的参数将会数据溢出,所以先自己通过响应头来获取长度,获取不到再使用参数的方式
    protected override void ReceiveContentLength(int contentLength)
    {
        string contentLengthStr = _unityWebRequest.GetResponseHeader("Content-Length");
        if (contentLengthStr != null && !"".Equals(contentLengthStr))
        {
            try
            {
                _totalFileSize = long.Parse(contentLengthStr);
            }
            catch (System.FormatException e)
            {
                UnityEngine.Debug.LogWarning("获取文件长度失败,contentLengthStr:" + contentLengthStr + "," + e.Message);
                _totalFileSize = contentLength;
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogWarning("获取文件长度失败,contentLengthStr:" + contentLengthStr + "," + e.Message);
                _totalFileSize = contentLength;
            }
        }
        else
        {
            _totalFileSize = contentLength;
        }
        //这里拿到的下载大小是待下载的文件大小,需要加上本地已下载文件的大小才等于总大小
        if (_useRange)
        {
            _totalFileSize += _localFileSize;
        }

        if (_useRange && _fileSize != -1 && _totalFileSize != _fileSize)
        {
            ErrorEvent?.Invoke();
        }
        else
        {
            _lastTime = UnityEngine.Time.time;
            _lastDataSize = _curFileSize;
            StartDownloadEvent?.Invoke();
        }
        
    }

    //在2017.3.0(包括该版本)以下的正式版本中存在一个性能上的问题
    //该回调方法有性能上的问题,每次传入的数据量最大不会超过65536(2^16)个字节,不论缓存区有多大
    //在下载速度中的体现,大约相当于每秒下载速度不会超过3.8MB/S
    //这个问题在 "补丁2017.2.1p1" 版本中被优化(2017.12.21发布)(https://unity3d.com/cn/unity/qa/patch-releases/2017.2.1p1)
    //(965165) - Web: UnityWebRequest: improve performance for DownloadHandlerScript.
    //优化后,每次传入数据量最大不会超过1048576(2^20)个字节(1MB),基本满足下载使用
    protected override bool ReceiveData(byte[] data, int dataLength)
    {
        if (data == null || dataLength == 0)
        {
            return false;
        }
        _fileStream.Write(data, 0, dataLength);
        _curFileSize += dataLength;
        //统计下载速度
        if (UnityEngine.Time.time - _lastTime >= 1.0f)
        {
            _downloadSpeed = (_curFileSize - _lastDataSize) / (UnityEngine.Time.time - _lastTime);
            _lastTime = UnityEngine.Time.time;
            _lastDataSize = _curFileSize;
        }
        return true;
    }

    ~DownloadHandlerFileRange()
    {
        if (_fileStream != null)
        {
            _fileStream.Flush();
            _fileStream.Dispose();
            _fileStream = null;
        }
    }

    public FileStream GetFileStream()
    {
        return _fileStream;
    }
}
