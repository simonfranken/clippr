using System.Text;

namespace sway.Core.Clip;

public class ClipContent
{
    public ClipContent(string text)
    {
        Type = ClipContentType.Text;
        Data = Encoding.UTF8.GetBytes(text);
    }
    public ClipContent(byte[] data, ClipContentType type)
    {
        Data = data;
        Type = type;
    }

    public byte[] Data { get; set; }
    public ClipContentType Type { get; set; }
}