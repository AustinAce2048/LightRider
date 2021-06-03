using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BasePlayer : NetworkBehaviour {

    public float forwardSpeed;
    public float strafeSpeed;
    public float jumpPower = 20f;

    private Rigidbody rb;
    private bool disabledMyself = false;

    private void Start () {
        //Get reference to controller
        rb = GetComponent<Rigidbody> ();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate () {
        if (isLocalPlayer) {
            //Incredibly basic movement
            if (Input.GetKey (KeyCode.W)) {
                rb.AddForce (transform.forward * forwardSpeed);
            } else if (Input.GetKey (KeyCode.S)) {
                rb.AddForce (-transform.forward * forwardSpeed / 2f);
            }

            if (Input.GetKey (KeyCode.A)) {
                rb.AddForce (-transform.right * strafeSpeed);
            } else if (Input.GetKey (KeyCode.D)) {
                rb.AddForce (transform.right * strafeSpeed);
            }

            //Gravity, working against me
            RaycastHit hit;
            if (Input.GetKey (KeyCode.Space) && Physics.Raycast (transform.position, -transform.up, out hit, 1.1f)) {
                rb.AddForce (transform.up * jumpPower);
            }
            rb.AddForce (new Vector3 (0f, -18.81f, 0f));
            rb.velocity = rb.velocity * 0.85f;
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