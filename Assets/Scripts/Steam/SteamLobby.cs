using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Steamworks;
using Mirror;

public class SteamLobby : MonoBehaviour {

    public GameObject playerPrefab;
    private NetworkManager networkManager;
    //Protected data type because Steam is a special snowflake
    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
    protected Callback<LobbyEnter_t> lobbyEntered;
    protected Callback<LobbyMatchList_t> lobbyMatchList_t;
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
        lobbyMatchList_t = Callback<LobbyMatchList_t>.Create (LobbySearch);
    }

    public void HostLobby () {
        //Create public lobby with max 2 people
        SteamMatchmaking.CreateLobby (ELobbyType.k_ELobbyTypePublic, networkManager.maxConnections);
    }

    public void JoinLobby () {
        //Runs callback
        SteamMatchmaking.RequestLobbyList ();
    }

    //Steam callback action
    private void LobbySearch (LobbyMatchList_t callback) {
        //Don't do anything if there are no open lobbies
        if (callback.m_nLobbiesMatching <= 0) {
            return;
        }
        //Only joins the first lobby (fine for testing)
        SteamMatchmaking.JoinLobby (SteamMatchmaking.GetLobbyByIndex (0));
    }
    private void OnLobbyCreated (LobbyCreated_t callback) {
        //What to do if the lobby wasnt created (failure on Steam's backend)
        if (callback.m_eResult != EResult.k_EResultOK) {
            SceneManager.LoadScene ("MainMenu", LoadSceneMode.Single);
            return;
        }
        lobbyId = new CSteamID (callback.m_ulSteamIDLobby);
        networkManager.StartHost ();
        //Create lobby data read the host steam id using the host address key constant
        SteamMatchmaking.SetLobbyData (lobbyId, hostAddressKey, SteamUser.GetSteamID ().ToString ());
        //Load into chosen level, make sure all players in that lobby go into the same level
        SceneManager.LoadScene ("TestScene", LoadSceneMode.Single);
    }
    
    private void OnGameLobbyJoinRequested (GameLobbyJoinRequested_t callback) {
        SteamMatchmaking.JoinLobby (callback.m_steamIDLobby);
    }

    private void OnLobbyEntered (LobbyEnter_t callback) {
        if (NetworkServer.active) {
            Debug.Log ("Is host doesnt need extra data on join");
            StartCoroutine (ShowingLevelCam ());
            return;
        }
        string hostAddress = SteamMatchmaking.GetLobbyData (new CSteamID (callback.m_ulSteamIDLobby), hostAddressKey);
        networkManager.networkAddress = hostAddress;
        NetworkClient.Connect (hostAddress);
        networkManager.StartClient ();
        //Load level, make sure its same as lobby level
        SceneManager.LoadScene ("TestScene", LoadSceneMode.Single);
        StartCoroutine (ShowingLevelCam ());
    }

    IEnumerator ShowingLevelCam () {
        yield return new WaitForSeconds (2f);
        Debug.Log ("yeet");
        //After showing cam spawn player
        GameObject.Find ("LevelCamera").SetActive (false);
        ClientScene.AddPlayer (NetworkClient.connection);
    }
}