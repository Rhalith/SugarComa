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
            GameObject f = Instantiate(friendObj, friendsContent);
            f.GetComponentInChildren<Text>().text = friend.Name;
            f.GetComponent<FriendObject>().steamid = friend.Id;
            AssingFriendImage(f, friend.Id);
        }
    }

    public async void AssingFriendImage(GameObject f, SteamId id)
    {
        var img = await SteamFriends.GetLargeAvatarAsync(id);
        f.GetComponentInChildren<RawImage>().texture = GetTextureFromImage(img.Value);
    }
    
    public static async System.Threading.Tasks.Task<Texture2D> GetTextureFromSteamIdAsync(SteamId id)
    {
        var img = await SteamFriends.GetLargeAvatarAsync(SteamClient.SteamId);
        Steamworks.Data.Image image = img.Value;
        Texture2D texture = new Texture2D((int)image.Width, (int)image.Height);

        for (int x = 0; x < image.Width; x++)
        {
            for (int y = 0; y < image.Height; y++)
            {
                var p = image.GetPixel(x, y);
                texture.SetPixel(x, (int)image.Height - y, new Color32(p.r, p.g, p.b, p.a));
            }
        }
        texture.Apply();
        return texture;
    }
}
