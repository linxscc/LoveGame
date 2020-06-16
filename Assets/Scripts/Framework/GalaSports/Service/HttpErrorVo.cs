public class HttpErrorVo
{
    public string ErrorString;

    public string Cmd;

    public object CustomData;

    public int ErrorCode;

    public override string ToString()
    {
        return
            $"{nameof(ErrorString)}: {ErrorString}, {nameof(Cmd)}: {Cmd}, {nameof(CustomData)}: {CustomData}, {nameof(ErrorCode)}: {ErrorCode}";
    }
}