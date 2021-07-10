using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : NetworkBehaviour {

    public GameObject playerPrefab;
    public GameObject enemy;
    public List<GameObject> enemySpawns = new List<GameObject> ();

    private bool isSolo = false;
    private int enemyId = 0;

    private void Start () {
        if (PlayerPrefs.GetString ("GameType") == "Solo") {
            isSolo = true;
            StartCoroutine (ShowingLevelCam ());
        }

        foreach (GameObject spawnPoint in enemySpawns) {
            if (isSolo) {
                Instantiate (enemy, spawnPoint.transform.position, Quaternion.identity);
            } else {
                GameObject enemyObject = Instantiate (enemy, spawnPoint.transform.position, Quaternion.identity);
                NetworkServer.Spawn (enemyObject);
                enemyObject.GetComponent<BaseEnemy> ().id = enemyId;
                enemyId++;
            }
        }
    }

    IEnumerator ShowingLevelCam () {
        yield return new WaitForSeconds (2f);
        //After showing cam spawn player
        GameObject.Find ("LevelCamera").SetActive (false);
        GameObject player = (GameObject)Instantiate (playerPrefab, new Vector3 (0f, 1.001f, 0f), Quaternion.identity);
    }

}