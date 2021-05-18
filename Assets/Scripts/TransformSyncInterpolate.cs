using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TransformSyncInterpolate : NetworkBehaviour {

    [SerializeField] private float lerpRate = 5;
    [SyncVar] private Vector3 syncPos;
 
    private Vector3 lastPos;
    [SerializeField] private float threshold = 0.5f;
 
 
    void Start () {
        syncPos = transform.position;
    }
 
 
    void FixedUpdate () {
        TransmitPosition ();
        if (!hasAuthority) {
            transform.position = Vector3.Lerp (transform.position, syncPos, Time.deltaTime * lerpRate);
        }
    }
 
    [Command]
    void Cmd_ProvidePositionToServer (Vector3 pos) {
        syncPos = pos;
    }
 
    [ClientCallback]
    void TransmitPosition () {
        if (hasAuthority  && Vector3.Distance (transform.position, lastPos) > threshold) {
            Cmd_ProvidePositionToServer (transform.position);
            lastPos = transform.position;
        }
    }

}