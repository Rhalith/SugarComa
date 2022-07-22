using Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class SteamFriendsManager : MonoBehaviour
{
    public RawImage pp;
    public Text playername;

    public Transform friendsContent;
    public GameObject friendObj;

    async void Start()
    {
        if (!SteamClient.IsValid) return;

        playername.text = SteamClient.Name;
        InitFriendsAsync();
        var img = await SteamFriends.GetLargeAvatarAsync(SteamClient.SteamId);
        pp.texture = GetTextureFromImage(img.Value);
    }

    public static Texture2D GetTextureFromImage(in Steamworks.Data.Image image)
    {
        Texture2D texture = new Texture2D((int)image.Width, (int)image.Height);

        Color32[] pixels = new Color32[image.Width * image.Height];
        for (int y = 0; y < image.Height; y++)
        {
            for (int x = 0; x < image.Width; x++)
            {
                var p = image.GetPixel(x, y);
                pixels[((int)image.Height - y - 1) * image.Width + x] = new Color32(p.r, p.g, p.b, p.a);
            }
        }
        texture.SetPixels32(pixels);
        texture.Apply();
        return texture;
    }

    public void InitFriendsAsync()
    {
        foreach (var friend in SteamFriends.GetFriends())
        {
            if (friend.IsOnline)
            {
                GameObject f = Instantiate(friendObj, friendsContent);
                f.GetComponentInChildren<Text>().text = friend.Name;
                f.GetComponent<FriendObject>().steamid = friend.Id;
                AssingFriendImage(f, friend.Id);
            }
        }
    }

    public async void AssingFriendImage(GameObject f, SteamId id)
    {
        var img = await SteamFriends.GetLargeAvatarAsync(id);
        f.GetComponentInChildren<RawImage>().texture = GetTextureFromImage(img.Value);
    }
    
    public static async System.Threading.Tasks.Task<Texture2D> GetTextureFromSteamIdAsync(SteamId id)
    {
        // TODO 
        var img = await SteamFriends.GetLargeAvatarAsync(id);
        if (!img.HasValue) return Texture2D.redTexture;

        Texture2D texture = new Texture2D((int)img.Value.Width, (int)img.Value.Height);

        Color32[] pixels = new Color32[img.Value.Width * img.Value.Height];
        for (int y = 0; y < img.Value.Height; y++)
        {
            for (int x = 0; x < img.Value.Width; x++)
            {
                var p = img.Value.GetPixel(x, y);
                pixels[((int)img.Value.Height - y - 1) * img.Value.Width + x] = new Color32(p.r, p.g, p.b, p.a);
            }
        }
        texture.SetPixels32(pixels);
        texture.Apply();
        return texture;
    }
}
