using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BasePlayer : NetworkBehaviour {

    public float forwardSpeed;
    public float strafeSpeed;

    private CharacterController cController;
    private bool disabledMyself = false;

    private void Start () {
        //Get reference to controller
        cController = GetComponent<CharacterController> ();
    }

    private void FixedUpdate () {
        if (isLocalPlayer) {
            //Incredibly basic movement
            if (Input.GetKey (KeyCode.W)) {
                cController.Move (transform.forward * Time.deltaTime * forwardSpeed);
            } else if (Input.GetKey (KeyCode.S)) {
                cController.Move (-transform.forward * Time.deltaTime * (forwardSpeed / 2));
            }

            if (Input.GetKey (KeyCode.A)) {
                cController.Move (-transform.right * Time.deltaTime * strafeSpeed);
            } else if (Input.GetKey (KeyCode.D)) {
                cController.Move (transform.right * Time.deltaTime * strafeSpeed);
            }
        } else {
            //Turn off your own scripts if you're not owned
            //Prevents other players from controlling each other
            if (!disabledMyself) {
                GetComponent<MouseLook> ().enabled = false;
                transform.GetChild (2).gameObject.SetActive (false);
                disabledMyself = true;
            }
        }

        //Way to quit game
        if (Input.GetKey (KeyCode.Escape)) {
            Application.Quit ();
        }
    }

}