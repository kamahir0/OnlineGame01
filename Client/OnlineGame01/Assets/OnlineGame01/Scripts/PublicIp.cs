using OutGame01;
using UnityEngine;

namespace OnlineGame01
{
    public static class PublicIp
    {
        public static string Current => _current ?? Resources.Load<PublicIpAsset>("PublicIp").PublicIp;
        private static string _current;
    }
}