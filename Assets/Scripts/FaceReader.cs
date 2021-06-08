using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceReader : MonoBehaviour {

    public string faceId;

    void OnTriggerStay (Collider other) {
        if (other.gameObject.tag == "Player") {
            other.gameObject.GetComponent<BasePlayer> ().face = faceId;
        }
    }

}