using HoloToolkit.Sharing;

namespace HoloGeek
{
    public interface IMessageReader
    {
        void readFrom(NetworkInMessage msg);
    }
}