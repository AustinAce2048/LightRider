using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;

public class CustomNetworkManager : NetworkManager {

    public override void OnServerAddPlayer (NetworkConnection conn) {
        Debug.Log ("trying to add player");
        GameObject player = (GameObject)Instantiate (playerPrefab, new Vector3 (0f, 5.508f, 0f), Quaternion.identity);
        NetworkServer.AddPlayerForConnection (conn, player);
        Debug.Log ("player spawned");
        //Steam id of person who just joined
        CSteamID steamId = SteamMatchmaking.GetLobbyMemberByIndex (SteamLobby.lobbyId, numPlayers - 1);
        //Get Component of that player
        var playerInfo = conn.identity.GetComponent<PlayerInfo> ();
        //Set the steam id ulong
        playerInfo.SetSteamId (steamId.m_SteamID);
    }

}