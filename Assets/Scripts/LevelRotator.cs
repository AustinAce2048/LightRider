using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRotator : MonoBehaviour {

    public float timeBetweenRotations;
    public float rotationTime;
    public GameObject detectors;
    public bool isRotating = false;

    private GameObject rotator;
    private RaycastHit[] hits;
    private string rotatingFace = "";
    private bool chosenDirection = false;
    private bool oppositeDirection = false;

    private void Start () {
        rotator = GameObject.Find ("Rotator");

        StartCoroutine (Rotate ());
    }

    IEnumerator Rotate () {
        yield return new WaitForSeconds (timeBetweenRotations);
        //Rotate
        switch (Random.Range (1, 7)) {
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
                rotatingFace = "F";
               StartCoroutine (PerformRotation ());
                break;
            case 2:
                //B
                hits = Physics.RaycastAll (detectors.transform.GetChild (4).transform.position, Vector3.down, 100f);
                for (int i = 0; i < hits.Length; i++) {
                    RaycastHit hit = hits[i];
                    if (hit.transform.gameObject.tag == "LevelPiece") {
                        hit.transform.parent = rotator.transform;
                    }
                }
                hits = Physics.RaycastAll (detectors.transform.GetChild (5).transform.position, Vector3.down, 100f);
                for (int i = 0; i < hits.Length; i++) {
                    RaycastHit hit = hits[i];
                    if (hit.transform.gameObject.tag == "LevelPiece") {
                        hit.transform.parent = rotator.transform;
                    }
                }
                rotatingFace = "B";
               StartCoroutine (PerformRotation ());
                break;
            case 3:
                //U
                hits = Physics.RaycastAll (detectors.transform.GetChild (0).transform.position, Vector3.forward, 100f);
                for (int i = 0; i < hits.Length; i++) {
                    RaycastHit hit = hits[i];
                    if (hit.transform.gameObject.tag == "LevelPiece") {
                        hit.transform.parent = rotator.transform;
                    }
                }
                hits = Physics.RaycastAll (detectors.transform.GetChild (1).transform.position, Vector3.forward, 100f);
                for (int i = 0; i < hits.Length; i++) {
                    RaycastHit hit = hits[i];
                    if (hit.transform.gameObject.tag == "LevelPiece") {
                        hit.transform.parent = rotator.transform;
                    }
                }
                rotatingFace = "U";
               StartCoroutine (PerformRotation ());
                break;
            case 4:
                //D
                hits = Physics.RaycastAll (detectors.transform.GetChild (2).transform.position, Vector3.forward, 100f);
                for (int i = 0; i < hits.Length; i++) {
                    RaycastHit hit = hits[i];
                    if (hit.transform.gameObject.tag == "LevelPiece") {
                        hit.transform.parent = rotator.transform;
                    }
                }
                hits = Physics.RaycastAll (detectors.transform.GetChild (3).transform.position, Vector3.forward, 100f);
                for (int i = 0; i < hits.Length; i++) {
                    RaycastHit hit = hits[i];
                    if (hit.transform.gameObject.tag == "LevelPiece") {
                        hit.transform.parent = rotator.transform;
                    }
                }
                rotatingFace = "D";
               StartCoroutine (PerformRotation ());
                break;
            case 5:
                //L
                hits = Physics.RaycastAll (detectors.transform.GetChild (1).transform.position, Vector3.forward, 100f);
                for (int i = 0; i < hits.Length; i++) {
                    RaycastHit hit = hits[i];
                    if (hit.transform.gameObject.tag == "LevelPiece") {
                        hit.transform.parent = rotator.transform;
                    }
                }
                hits = Physics.RaycastAll (detectors.transform.GetChild (3).transform.position, Vector3.forward, 100f);
                for (int i = 0; i < hits.Length; i++) {
                    RaycastHit hit = hits[i];
                    if (hit.transform.gameObject.tag == "LevelPiece") {
                        hit.transform.parent = rotator.transform;
                    }
                }
                rotatingFace = "L";
               StartCoroutine (PerformRotation ());
                break;
            case 6:
                //R
                hits = Physics.RaycastAll (detectors.transform.GetChild (0).transform.position, Vector3.forward, 100f);
                for (int i = 0; i < hits.Length; i++) {
                    RaycastHit hit = hits[i];
                    if (hit.transform.gameObject.tag == "LevelPiece") {
                        hit.transform.parent = rotator.transform;
                    }
                }
                hits = Physics.RaycastAll (detectors.transform.GetChild (2).transform.position, Vector3.forward, 100f);
                for (int i = 0; i < hits.Length; i++) {
                    RaycastHit hit = hits[i];
                    if (hit.transform.gameObject.tag == "LevelPiece") {
                        hit.transform.parent = rotator.transform;
                    }
                }
                rotatingFace = "R";
               StartCoroutine (PerformRotation ());
                break;
        }
        StartCoroutine (Rotate ());
    }

    int direction = 0;
    private void Update () {
        if (isRotating) {
            if (!chosenDirection) {
                direction = Random.Range (1, 3);
                chosenDirection = true;
            }
            switch (rotatingFace) {
                case "F":
                    if (direction == 1) {
                        rotator.transform.RotateAround (GameObject.Find ("F").transform.position, Vector3.forward, -90f * (Time.deltaTime / rotationTime));
                    } else {
                        oppositeDirection = true;
                        rotator.transform.RotateAround (GameObject.Find ("F").transform.position, Vector3.forward, 90f * (Time.deltaTime / rotationTime));
                    }
                    break;
                case "B":
                if (direction == 1) {
                        rotator.transform.RotateAround (GameObject.Find ("B").transform.position, Vector3.forward, 90f * (Time.deltaTime / rotationTime));
                    } else {
                        oppositeDirection = true;
                        rotator.transform.RotateAround (GameObject.Find ("B").transform.position, Vector3.forward, -90f * (Time.deltaTime / rotationTime));
                    }
                    break;
                case "U":
                if (direction == 1) {
                        rotator.transform.RotateAround (GameObject.Find ("U").transform.position, Vector3.up, 90f * (Time.deltaTime / rotationTime));
                    } else {
                        oppositeDirection = true;
                        rotator.transform.RotateAround (GameObject.Find ("U").transform.position, Vector3.up, -90f * (Time.deltaTime / rotationTime));
                    }
                    break;
                case "D":
                if (direction == 1) {
                        rotator.transform.RotateAround (GameObject.Find ("D").transform.position, Vector3.up, -90f * (Time.deltaTime / rotationTime));
                    } else {
                        oppositeDirection = true;
                        rotator.transform.RotateAround (GameObject.Find ("D").transform.position, Vector3.up, 90f * (Time.deltaTime / rotationTime));
                    }
                    break;
                case "L":
                if (direction == 1) {
                        rotator.transform.RotateAround (GameObject.Find ("L").transform.position, Vector3.left, 90f * (Time.deltaTime / rotationTime));
                    } else {
                        oppositeDirection = true;
                        rotator.transform.RotateAround (GameObject.Find ("L").transform.position, Vector3.left, -90f * (Time.deltaTime / rotationTime));
                    }
                    break;
                case "R":
                if (direction == 1) {
                        rotator.transform.RotateAround (GameObject.Find ("R").transform.position, Vector3.left, -90f * (Time.deltaTime / rotationTime));
                    } else {
                        oppositeDirection = true;
                        rotator.transform.RotateAround (GameObject.Find ("R").transform.position, Vector3.left, 90f * (Time.deltaTime / rotationTime));
                    }
                    break;
            }
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
        chosenDirection = false;
        isRotating = true;
        yield return new WaitForSecondsRealtime (rotationTime);
        isRotating = false;
        switch (rotatingFace) {
            case "F":
                if (!oppositeDirection) {
                    rotator.transform.eulerAngles = new Vector3 (rotator.transform.rotation.x, rotator.transform.rotation.y, -90f);
                } else {
                    rotator.transform.eulerAngles = new Vector3 (rotator.transform.rotation.x, rotator.transform.rotation.y, 90f);
                }
                rotator.transform.position = new Vector3 (rotator.transform.position.x, -4.5f, rotator.transform.position.z);
                break;
            case "B":
                if (!oppositeDirection) {
                    rotator.transform.eulerAngles = new Vector3 (rotator.transform.rotation.x, rotator.transform.rotation.y, 90f);
                } else {
                    rotator.transform.eulerAngles = new Vector3 (rotator.transform.rotation.x, rotator.transform.rotation.y, -90f);
                }
                rotator.transform.position = new Vector3 (rotator.transform.position.x, -4.5f, rotator.transform.position.z);
                break;
            case "U":
                if (!oppositeDirection) {
                    rotator.transform.eulerAngles = new Vector3 (rotator.transform.rotation.x, 90f, rotator.transform.rotation.z);
                } else {
                    rotator.transform.eulerAngles = new Vector3 (rotator.transform.rotation.x, -90f, rotator.transform.rotation.z);
                }
                rotator.transform.position = new Vector3 (rotator.transform.position.x, rotator.transform.position.y, 0f);
                break;
            case "D":
                if (!oppositeDirection) {
                    rotator.transform.eulerAngles = new Vector3 (rotator.transform.rotation.x, -90f, rotator.transform.rotation.z);
                } else {
                    rotator.transform.eulerAngles = new Vector3 (rotator.transform.rotation.x, 90f, rotator.transform.rotation.z);
                }
                rotator.transform.position = new Vector3 (rotator.transform.position.x, rotator.transform.position.y, 0f);
                break;
            case "L":
                if (!oppositeDirection) {
                    rotator.transform.eulerAngles = new Vector3 (-90f, rotator.transform.rotation.y, rotator.transform.rotation.z);
                } else {
                    rotator.transform.eulerAngles = new Vector3 (90f, rotator.transform.rotation.y, rotator.transform.rotation.z);
                }
                rotator.transform.position = new Vector3 (rotator.transform.position.x, -4.5f, rotator.transform.position.z);
                break;
            case "R":
                if (!oppositeDirection) {
                    rotator.transform.eulerAngles = new Vector3 (90f, rotator.transform.rotation.y, rotator.transform.rotation.z);
                } else {
                    rotator.transform.eulerAngles = new Vector3 (-90f, rotator.transform.rotation.y, rotator.transform.rotation.z);
                }
                rotator.transform.position = new Vector3 (rotator.transform.position.x, -4.5f, rotator.transform.position.z);
                break;
        }
        oppositeDirection = false;
        rotatingFace = "";
    }

}