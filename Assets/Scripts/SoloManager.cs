using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoloManager : MonoBehaviour {

    private void Start () {
        foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject> ()) {
            if (obj.name == "GameManager") {
                obj.SetActive (true);
            }
        }
    }

}