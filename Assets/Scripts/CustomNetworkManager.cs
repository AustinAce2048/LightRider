using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using UnityEngine.SceneManagement;

public class CustomNetworkManager : NetworkManager {

    public override void OnServerAddPlayer (NetworkConnection conn) {
        if (PlayerPrefs.GetString ("GameType") == "Private") {
            //Private game
            if (NetworkServer.connections.Count == GetComponent<NetworkManager> ().maxConnections) {
                //Start game
                SpawnPlayer (conn);
            }
        } else {
            //Public/Random game
            SpawnPlayer (conn);
        }
    }

    void SpawnPlayer (NetworkConnection conn) {
        Debug.Log ("trying to add player");
        GameObject player = (GameObject)Instantiate (playerPrefab, new Vector3 (0f, 1.001f, 0f), Quaternion.identity);
        NetworkServer.AddPlayerForConnection (conn, player);
        Debug.Log ("player spawned");
        //Steam id of person who just joined
        CSteamID steamId = SteamMatchmaking.GetLobbyMemberByIndex (SteamLobby.lobbyId, numPlayers - 1);
        //Get Component of that player
        var playerInfo = conn.identity.GetComponent<PlayerInfo> ();
        //Set the steam id ulong
        playerInfo.SetSteamId (steamId.m_SteamID);
    }

    public override void OnClientDisconnect (NetworkConnection conn) {
        base.OnClientDisconnect (conn);
        SceneManager.LoadScene ("MainMenu", LoadSceneMode.Single);
    }

}