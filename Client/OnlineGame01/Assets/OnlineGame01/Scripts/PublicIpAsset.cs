using UnityEngine;

namespace OutGame01
{
    [CreateAssetMenu(fileName = "PublicIp", menuName = "Scriptable Objects/PublicIpAsset")]
    public class PublicIpAsset : ScriptableObject
    {
        public string PublicIp;
    }
}