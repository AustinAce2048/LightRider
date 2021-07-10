using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BaseEnemy : NetworkBehaviour {

    public GameObject shootingPoint;
    public float delayBetweenShots = 0.5f;
    public float damagePerShot;

    private GameObject target;
    private RaycastHit hit2;
    public float currentHealth;
    private bool cooldown = false;
    private bool isSolo = false;

    private void Start () {
        if (PlayerPrefs.GetString ("GameType") == "Solo") {
            isSolo = true;
        }
    }

    private void Update () {
        if (target != null) {
            transform.LookAt (target.transform, Vector3.up);
            if (Physics.Raycast (shootingPoint.transform.position, shootingPoint.transform.parent.forward, out hit2, 100f)) {
                if (hit2.collider.gameObject.tag == "Player") {
                    //Looking at player, shoot
                    if (!cooldown) {
                        if (isSolo) {
                            hit2.collider.gameObject.GetComponent<BasePlayer> ().TakeDamage (damagePerShot);
                        } else {
                            CmdDamage (hit2.collider.gameObject, 25f);
                        }
                        cooldown = true;
                        StartCoroutine (FireCooldown ());
                    }
                }
            }
        }
    }

    IEnumerator FireCooldown () {
        yield return new WaitForSeconds (delayBetweenShots);
        cooldown = false;
    }

    //Enemy takes damage
    public void TakeDamage (float damage) {
        if ((currentHealth - damage) <= 0) {
            //Die
            if (isSolo) {
                Destroy (gameObject);
            } else {
                //Multiplayer die
                NetworkServer.Destroy (gameObject);
            }
        } else {
            currentHealth = currentHealth - damage;
        }
    }

    [Command]
    void CmdDamage (GameObject target, float damage) {
        target.GetComponent<BasePlayer> ().TakeDamage (damage);
        TargetTakeDamage (target.GetComponent<NetworkIdentity> ().connectionToClient, damage);
    }

    [TargetRpc]
    public void TargetTakeDamage (NetworkConnection target, float damage) {
        currentHealth = currentHealth - damage;
        Debug.Log (currentHealth);
    }

    void OnTriggerStay (Collider other) {
        if (other.gameObject.tag == "Player") {
            target = other.gameObject;
        }
    }

    void OnTriggerExit (Collider other) {
        if (other.gameObject.tag == "Player") {
            target = null;
        }
    }

}