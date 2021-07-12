using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BasePlayer : NetworkBehaviour {

    public float forwardSpeed;
    public float strafeSpeed;
    public float jumpPower = 20f;
    public float damagePerShot = 25f;
    public GameObject shootingPoint;
    public int id;

    private Rigidbody rb;
    private bool disabledMyself = false;
    private RaycastHit hit2;
    [SyncVar]
    public float currentHealth = 100f;
    private bool isSolo = false;

    private void Start () {
        //Check if solo
        if (PlayerPrefs.GetString ("GameType") == "Solo") {
            isSolo = true;
            Destroy (GetComponent<TransformSyncInterpolate> ());
            Destroy (GetComponent<PlayerInfo> ());
        }
        //Get reference to controller
        rb = GetComponent<Rigidbody> ();
        //Hide and lock cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate () {
        if (isLocalPlayer || isSolo) {
            //Shooting
            if (Input.GetMouseButtonDown (0)) {
                Debug.DrawRay (shootingPoint.transform.position, shootingPoint.transform.parent.forward * 100f, Color.green, 10f);
                if (Physics.Raycast (shootingPoint.transform.position, shootingPoint.transform.parent.forward, out hit2, 100f)) {
                    if (hit2.collider.gameObject.tag == "Enemy") {
                        if (isSolo) {
                            hit2.collider.gameObject.GetComponent<BaseEnemy> ().SoloTakeDamage (damagePerShot);
                        } else {
                            CmdDamage (hit2.collider.gameObject.GetComponent<BaseEnemy> ().id, damagePerShot, id);
                        }
                    }
                }
            }

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
            if (Input.GetKey (KeyCode.Space) && Physics.Raycast (transform.position, -transform.up, out hit, 1.1f)) {
                rb.AddForce (transform.up * jumpPower);
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

    [Command]
    void CmdDamage (int id, float damage, int playerId) {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag ("Player")) {
            if (player.GetComponent<BasePlayer> ().id == playerId) {
                player.GetComponent<BasePlayer> ().RpcDamageEnemy (id, damage);
            }
        }
    }

    [ClientRpc]
    void RpcDamageEnemy (int id, float damage) {
        Debug.Log ("Damage enemy");
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag ("Enemy")) {
            if (enemy.GetComponent<BaseEnemy> ().id == id) {
                enemy.GetComponent<BaseEnemy> ().TakeDamage (damage);
            }
        }
    }

    public void TakeDamage (float damage) {
        currentHealth = currentHealth - damage;
    }

}