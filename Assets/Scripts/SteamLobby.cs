using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using Mirror;

public class SteamLobby : MonoBehaviour {

    //Reference to lobby button
    public GameObject button;
    private CustomNetworkManager customNetworkManager;
    //Protected data type because Steam is a special snowflake
    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
    protected Callback<LobbyEnter_t> lobbyEntered;
    private const string hostAddressKey = "HostAddress";

    void Start() {
        //Kill the whole process if steam isnt on
        if (!SteamManager.Initialized) {
            return;
        }
        customNetworkManager = GameObject.Find ("NetworkManager").GetComponent<CustomNetworkManager> ();
        //Setup the lobby created callback
        lobbyCreated = Callback<LobbyCreated_t>.Create (OnLobbyCreated);
        gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create (OnGameLobbyJoinRequested);
        lobbyEntered = Callback<LobbyEnter_t>.Create (OnLobbyEntered);
    }

    public void HostLobby () {
        //Disable lobby UI
        button.SetActive (false);
        //Create public lobby with max 2 people
        SteamMatchmaking.CreateLobby (ELobbyType.k_ELobbyTypePublic, 2);
    }

    //Steam callback action
    private void OnLobbyCreated (LobbyCreated_t callback) {
        //What to do if the lobby wasnt created
        if (callback.m_eResult != EResult.k_EResultOK) {
            button.SetActive (true);
            return;
        }
        customNetworkManager.StartHost ();
        //Create lobby data read the host steam id using the host address key constant
        SteamMatchmaking.SetLobbyData (new CSteamID (callback.m_ulSteamIDLobby), hostAddressKey, SteamUser.GetSteamID ().ToString ());
    }
    
    private void OnGameLobbyJoinRequested (GameLobbyJoinRequested_t callback) {
        SteamMatchmaking.JoinLobby (callback.m_steamIDLobby);
    }

    private void OnLobbyEntered (LobbyEnter_t callback) {
        string hostAddress = SteamMatchmaking.GetLobbyData (new CSteamID (callback.m_ulSteamIDLobby), hostAddressKey);
        customNetworkManager.networkAddress = hostAddress;
        customNetworkManager.StartClient ();
        button.SetActive (false);
    }
}