using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using Mirror;

public class SteamLobby : MonoBehaviour {

    //Reference to lobby button
    public GameObject button;
    private NetworkManager networkManager;
    //Protected data type because Steam is a special snowflake
    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
    protected Callback<LobbyEnter_t> lobbyEntered;
    private const string hostAddressKey = "HostAddress";
    //Read this from anywhere but only able to set it in here
    public static CSteamID lobbyId {get; private set;}

    void Start () {
        networkManager = GetComponent<NetworkManager> ();
        //Kill the whole process if steam isnt on
        if (!SteamManager.Initialized) {
            return;
        }
        //Setup the lobby created callback
        lobbyCreated = Callback<LobbyCreated_t>.Create (OnLobbyCreated);
        gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create (OnGameLobbyJoinRequested);
        lobbyEntered = Callback<LobbyEnter_t>.Create (OnLobbyEntered);
    }

    public void HostLobby () {
        //Disable lobby UI
        button.SetActive (false);
        //Create public lobby with max 2 people
        SteamMatchmaking.CreateLobby (ELobbyType.k_ELobbyTypePublic, networkManager.maxConnections);
    }

    //Steam callback action
    private void OnLobbyCreated (LobbyCreated_t callback) {
        //What to do if the lobby wasnt created
        if (callback.m_eResult != EResult.k_EResultOK) {
            button.SetActive (true);
            return;
        }
        lobbyId = new CSteamID (callback.m_ulSteamIDLobby);
        networkManager.StartHost ();
        //Create lobby data read the host steam id using the host address key constant
        SteamMatchmaking.SetLobbyData (lobbyId, hostAddressKey, SteamUser.GetSteamID ().ToString ());
    }
    
    private void OnGameLobbyJoinRequested (GameLobbyJoinRequested_t callback) {
        SteamMatchmaking.JoinLobby (callback.m_steamIDLobby);
    }

    private void OnLobbyEntered (LobbyEnter_t callback) {
        if (NetworkServer.active) {
            return;
        }
        string hostAddress = SteamMatchmaking.GetLobbyData (new CSteamID (callback.m_ulSteamIDLobby), hostAddressKey);
        networkManager.networkAddress = hostAddress;
        networkManager.StartClient ();
        button.SetActive (false);
    }
}