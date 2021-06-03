using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRotator : MonoBehaviour {

    public float timeBetweenRotations;
    public float rotationTime;
    public GameObject detectors;

    private GameObject rotator;
    private RaycastHit[] hits;
    private bool isRotating = false;

    private void Start () {
        rotator = GameObject.Find ("Rotator");

        StartCoroutine (Rotate ());
    }

    IEnumerator Rotate () {
        yield return new WaitForSeconds (timeBetweenRotations);
        //Rotate
        switch (Random.Range (1, 2)) {
            case 1:
                //F
                hits = Physics.RaycastAll (detectors.transform.GetChild (6).transform.position, Vector3.down, 100f);
                for (int i = 0; i < hits.Length; i++) {
                    RaycastHit hit = hits[i];
                    if (hit.transform.gameObject.tag == "LevelPiece") {
                        hit.transform.parent = rotator.transform;
                    }
                }
                hits = Physics.RaycastAll (detectors.transform.GetChild (7).transform.position, Vector3.down, 100f);
                for (int i = 0; i < hits.Length; i++) {
                    RaycastHit hit = hits[i];
                    if (hit.transform.gameObject.tag == "LevelPiece") {
                        hit.transform.parent = rotator.transform;
                    }
                }
               StartCoroutine (PerformRotation ());
                break;
            case 2:
                //B
                break;
            case 3:
                //U
                break;
            case 4:
                //D
                break;
            case 5:
                //L
                break;
            case 6:
                //R
                break;
        }
        StartCoroutine (Rotate ());
    }

    private void Update () {
        if (isRotating) {
            rotator.transform.RotateAround (GameObject.Find ("F").transform.position, Vector3.forward, 90f * (Time.deltaTime / rotationTime));
        } else {
            foreach (Transform piece in rotator.transform) {
                piece.transform.parent = gameObject.transform;
            }
            if (rotator.transform.childCount == 0) {
                rotator.transform.eulerAngles = Vector3.zero;
                rotator.transform.position = Vector3.zero;
            }
        }
    }

    IEnumerator PerformRotation () {
        isRotating = true;
        yield return new WaitForSecondsRealtime (rotationTime);
        isRotating = false;
        rotator.transform.eulerAngles = new Vector3 (rotator.transform.rotation.x, rotator.transform.rotation.y, 90f);
        rotator.transform.position = new Vector3 (rotator.transform.position.x, -4.5f, rotator.transform.position.z);
    }

}