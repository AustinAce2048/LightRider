using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TransformSyncInterpolate : NetworkBehaviour {

    [SerializeField] private float lerpRate = 5;
    [SyncVar] private Vector3 syncPos;
 
    private Vector3 lastPos;
    [SyncVar] private Quaternion syncRot;
 
    private Quaternion lastRot;
    [SerializeField] private float threshold = 0.5f;
 
 
    void Start () {
        syncPos = transform.position;
    }
 
 
    void FixedUpdate () {
        SendPosition ();
        SendRotation ();
        if (!hasAuthority) {
            transform.position = Vector3.Lerp (transform.position, syncPos, Time.deltaTime * lerpRate);
        }
        if (!hasAuthority) {
            transform.rotation = Quaternion.Lerp (transform.rotation, syncRot, Time.deltaTime * lerpRate);
        }
    }
 
    [Command]
    void Cmd_ProvidePositionToServer (Vector3 pos) {
        syncPos = pos;
    }

    [Command]
    void Cmd_ProvideRotationToServer (Quaternion rot) {
        syncRot = rot;
    }
 
    [ClientCallback]
    void SendPosition () {
        if (hasAuthority  && Vector3.Distance (transform.position, lastPos) > threshold) {
            Cmd_ProvidePositionToServer (transform.position);
            lastPos = transform.position;
        }
    }

    [ClientCallback]
    void SendRotation () {
        if (hasAuthority  && Quaternion.Angle (transform.rotation, lastRot) > threshold) {
            Cmd_ProvideRotationToServer (transform.rotation);
            lastRot = transform.rotation;
        }
    }

}