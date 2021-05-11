using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;

public class CustomNetworkManager : NetworkManager {

    public override void OnServerAddPlayer (NetworkConnection conn) {
        base.OnServerAddPlayer (conn);
        //Steam id of person who just joined
        CSteamID steamId = SteamMatchmaking.GetLobbyMemberByIndex (SteamLobby.lobbyId, numPlayers - 1);
        //Get Component of that player
        var playerInfo = conn.identity.GetComponent<PlayerInfo> ();
        //Set the steam id ulong
        playerInfo.SetSteamId (steamId.m_SteamID);
    }

}