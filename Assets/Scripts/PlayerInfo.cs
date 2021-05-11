using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Steamworks;

public class PlayerInfo : NetworkBehaviour {

    [SyncVar (hook = nameof(HandeSteamIdUpdated))]
    private ulong steamId;
    public RawImage profileImage;
    public TMP_Text displayNameText;

    protected Callback<AvatarImageLoaded_t> avatarImageLoaded;

    public override void OnStartClient () {
        avatarImageLoaded = Callback<AvatarImageLoaded_t>.Create (OnAvatarImageLoaded); 
    }

    public void SetSteamId (ulong steamId) {
        this.steamId = steamId;
    }

    //Client side sync of just the steam id then the clients can pull the info
    private void HandeSteamIdUpdated (ulong oldSteamId, ulong newSteamId) {
        var CSteamID = new CSteamID (newSteamId);
        //You dont need to be steam friends this is just how Valve set it up
        displayNameText.text = SteamFriends.GetFriendPersonaName (CSteamID);
        
        int imageId = SteamFriends.GetLargeFriendAvatar (CSteamID);
        if (imageId == -1) {
            //Is not in cache
            return;
        } else {
            //Is in cache
            profileImage.texture = GetSteamImageAsTexture (imageId);
        }
    }

    private void OnAvatarImageLoaded (AvatarImageLoaded_t callback) {
        if (callback.m_steamID.m_SteamID != steamId) {
            return;
        } else {
            profileImage.texture = GetSteamImageAsTexture (callback.m_iImage);
        }
    }

    private Texture2D GetSteamImageAsTexture (int iImage) {
        Texture2D texture = null;
        //Steam says whether or not its the right size
        bool isValid = SteamUtils.GetImageSize (iImage, out uint width, out uint height);
        if (isValid) {
            //Create raw byte array to stream data
            byte[] image =  new byte[width * height * 4];
            //Grab steam image
            isValid = SteamUtils.GetImageRGBA (iImage, image, (int)(width * height * 4));
            if (isValid) {
                //Prep unity image texture and load it with the byte array
                //Also for some reason steam send the image upside down. Freaking aussies
                texture = new Texture2D ((int)width, (int)height, TextureFormat.RGBA32, false, true);
                texture.LoadRawTextureData (image);
                texture.Apply ();
            }
        }
        return texture;
    }

}