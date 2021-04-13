using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamOverlay : MonoBehaviour {

    protected Callback<GameOverlayActivated_t> m_GameOverlayActivated;

    private void OnEnable () {
        if (SteamManager.Initialized) {
            m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create (OnGameOverlayActivated);
        }
    }

    private void OnGameOverlayActivated (GameOverlayActivated_t pCallback) {
        if (pCallback.m_bActive != 0) {
            Debug.Log ("Steam overlay is open");
        } else {
            Debug.Log ("Steam overlay is closed");
        }
    }

}