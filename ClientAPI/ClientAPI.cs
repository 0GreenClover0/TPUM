using System.Text;

namespace ClientAPI
{
    public static class Utilities
    {
        public static ArraySegment<byte> StringToBytes(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            return new ArraySegment<byte>(buffer);
        }
    }

    internal class ClientAPI
    {
    }
}
