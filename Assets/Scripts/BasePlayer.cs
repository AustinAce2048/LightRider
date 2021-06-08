using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BasePlayer : NetworkBehaviour {

    public float forwardSpeed;
    public float strafeSpeed;
    public float jumpPower = 20f;
    public string face;

    private Rigidbody rb;
    private bool disabledMyself = false;
    private bool switchedParent = false;
    private LevelRotator levelRotator;
    private bool unparent = false;
    private Vector3 previousDirection;
    private GameObject cachedFace;

    private void Start () {
        //Get reference to controller
        rb = GetComponent<Rigidbody> ();
        //Hide and lock cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        levelRotator = GameObject.Find ("LevelGeo").GetComponent<LevelRotator> ();
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

            //Gravity, its working against me
            RaycastHit hit;
            RaycastHit hit2;
            if (Input.GetKey (KeyCode.Space) && Physics.Raycast (transform.position, -transform.up, out hit, 1.1f)) {
                rb.AddForce (transform.up * jumpPower);
            }
            if (Physics.Raycast (transform.position, -transform.up, out hit2, 5f)) {
                if (hit2.collider.gameObject.tag == "Face") {
                    if (levelRotator.isRotating) {
                        if (!switchedParent) {
                            transform.parent = hit2.collider.gameObject.transform;
                            transform.localScale = new Vector3 (0.11111f, 111.11111f, 0.11111f);
                            cachedFace = hit2.collider.gameObject;
                            switchedParent = true;
                        } else {
                            if (hit2.collider.gameObject != cachedFace) {
                                switchedParent = false;
                            }
                        }
                    }
                }
            }
            rb.AddForce (-transform.up * 23f);
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