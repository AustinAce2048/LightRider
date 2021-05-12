using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamConsoleTest : MonoBehaviour {

    void Start () {
        //Always make sure SteamManager is running before calling steam functions
        if (SteamManager.Initialized) {
            Debug.Log (SteamFriends.GetPersonaName ());
        }
    }
}