using Steamworks;
using UnityEngine;

namespace TempScripts
{
    public class TempStructScript : MonoBehaviour
    {
        // Böyle kullanmada artık data kalma muhabbeti olur mu??
    
        public struct TempStruct
        {
            public SteamId id;
            public Vector3 direction;
            public string dirStr;
        }

        public static TempStruct temp = new TempStruct();
    }
}
