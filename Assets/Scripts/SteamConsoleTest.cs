using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamConsoleTest : MonoBehaviour {

    void Start () {
        if (SteamManager.Initialized) {
            Debug.Log (SteamFriends.GetPersonaName ());
        }
    }

}