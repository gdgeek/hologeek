using HoloToolkit.Sharing;

namespace HoloGeek
{
    public interface IMessageWriter
    {
        void writeTo(NetworkOutMessage msg);
    }
}