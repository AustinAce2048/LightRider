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
    [SyncVar]
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
                        hit2.collider.gameObject.GetComponent<BasePlayer> ().TakeDamage (damagePerShot);
                        cooldown = true;
                        StartCoroutine (FireCooldown ());
                    }
                }
            }
        }

        //Check if enemy is dead
        if (currentHealth <= 0) {
            Destroy (gameObject);
        }
    }

    IEnumerator FireCooldown () {
        yield return new WaitForSeconds (delayBetweenShots);
        cooldown = false;
    }

    //Enemy takes damage
    [ClientRpc]
    public void TakeDamage (float damage) {
        currentHealth = currentHealth - damage;
    }

    [Command]
    void CmdDamage (GameObject target, float damage) {
        target.GetComponent<BasePlayer> ().TakeDamage (damage);
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