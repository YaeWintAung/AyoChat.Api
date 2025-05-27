using System.Collections.Concurrent;

namespace AyoChat.Api.Hubs
{
    public static class ConnectedUsers
    {
        public static readonly ConcurrentDictionary<string, string> OnlineUsers = new();
    }
}
