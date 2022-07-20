using Steamworks;
using UnityEngine;

namespace TempScripts
{
    public class PlayerInfo : MonoBehaviour
    {
        // Böyle kullanmada artık data kalma muhabbeti olur mu??
        
        // Mesajlar için enum açabilirsin..
    
        public struct Info
        {
            public SteamId id;
            public Vector3 direction;
            public string dirStr;
        }

        public static Info playerInfo = new Info();
    }
}
