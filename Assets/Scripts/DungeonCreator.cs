using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCreator : MonoBehaviour {

    public int iterations;
    public GameObject room;
    public GameObject zShortHallway;
    public GameObject xShortHallway;
    public GameObject zLongHallway;
    public GameObject xLongHallway;
    public GameObject zPosLeftTurn;
    public GameObject zPosRightTurn;
    public GameObject zNegLeftTurn;
    public GameObject zNegRightTurn;
    public GameObject xPosLeftTurn;
    public GameObject xPosRightTurn;
    public GameObject xNegLeftTurn;
    public GameObject xNegRightTurn;
    public GameObject zPosT;
    public GameObject zNegT;
    public GameObject xPosT;
    public GameObject xNegT;
    public List<GameObject> currentEdge = new List<GameObject> ();
    public List<GameObject> newEdge = new List<GameObject> ();

    private string newPieceDirection;
    private GameObject dungeonHead;
    private List<string> edgeDirections = new List<string> ();

    private void Start () {
        dungeonHead = new GameObject ();
        dungeonHead.name = "DungeonHead";
        GenerateDungeon ();
    }

    private void GenerateDungeon () {
        GameObject piece = Instantiate (room, Vector3.zero, room.transform.rotation);
        currentEdge.Add (piece);

        //Break walls for spawn room
        int amountOfRemovedSpawnWalls = 0;
        for (int i = 0; i < 4; i++) {
            if (Random.Range (0, 2) == 1) {
                piece.transform.GetChild (i).gameObject.SetActive (false);
                Destroy (piece.transform.GetChild (i).gameObject);
                amountOfRemovedSpawnWalls++;
            }
        }
        //If it randomly chose none, break the Z+ wall
        if (amountOfRemovedSpawnWalls == 0) {
            piece.transform.GetChild (0).gameObject.SetActive (false);
            Destroy (piece.transform.GetChild (0).gameObject);
        }

        for (int i = 0; i < iterations; i++) {
            Debug.Log ("Iteration " + i);

            foreach (GameObject edge in currentEdge) {
                switch (Random.Range(1, 5)) {
                    case 1:
                        newPieceDirection = "Z+";
                        break;
                    case 2:
                        newPieceDirection = "Z-";
                        break;
                    case 3:
                        newPieceDirection = "X+";
                        break;
                    case 4:
                        newPieceDirection = "X-";
                        break;
                }

                List<string> directionItCameFrom = new List<string> ();
                if (edge.name.Contains ("Room")) {
                    //Ignore the direction it came from
                    dungeonHead.transform.position = new Vector3 (edge.transform.position.x, edge.transform.position.y + 1f, edge.transform.position.z);
                    RaycastHit hit;
                    //Positive z
                    if (Physics.Raycast (dungeonHead.transform.position, dungeonHead.transform.forward, out hit, 10f)) {
                        //There's a wall here
                    } else {
                        directionItCameFrom.Add ("Z+");
                    }

                    //Negative z
                    if (Physics.Raycast (dungeonHead.transform.position, -dungeonHead.transform.forward, out hit, 10f)) {
                        //There's a wall here
                    } else {
                       directionItCameFrom.Add ("Z-");
                    }

                    //Positve x
                    if (Physics.Raycast (dungeonHead.transform.position, dungeonHead.transform.right, out hit, 10f)) {
                        //There's a wall here
                    } else {
                        directionItCameFrom.Add ("X+");
                    }

                    //Negative x
                    if (Physics.Raycast (dungeonHead.transform.position, -dungeonHead.transform.right, out hit, 10f)) {
                        //There's a wall here
                    } else {
                        directionItCameFrom.Add ("X-");
                    }
                }

                if (directionItCameFrom.Count > 0) {
                    foreach (string direction in directionItCameFrom) {
                        switch (direction) {
                            case "Z+":
                                switch (Random.Range (1, 7)) {
                                    case 1:
                                        //Room
                                        GameObject _room = Instantiate (room, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (0).transform.GetChild (1).position, room.transform.rotation);
                                        Destroy (_room.transform.Find ("Z-Wall").gameObject);
                                        newEdge.Add (_room);
                                        //Direction is added but ignored, need it for a index spacer
                                        edgeDirections.Add ("+z");
                                        break;
                                    case 2:
                                        //Short hallway
                                        GameObject hallway = Instantiate (zShortHallway, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (0).transform.GetChild (0).position, zShortHallway.transform.rotation);
                                        newEdge.Add (hallway);
                                        edgeDirections.Add ("+z");
                                        break;
                                    case 3:
                                        //Long hallway
                                        GameObject lHallway = Instantiate (zLongHallway, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (0).transform.GetChild (1).position, zLongHallway.transform.rotation);
                                        newEdge.Add (lHallway);
                                        edgeDirections.Add ("+z");
                                        break;
                                    case 4:
                                        //Turn left
                                        GameObject turn = Instantiate (zPosLeftTurn, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (0).transform.GetChild (0).position, zPosLeftTurn.transform.rotation);
                                        newEdge.Add (turn);
                                        edgeDirections.Add ("-x");
                                        break;
                                    case 5:
                                        //Turn right
                                        GameObject turn2 = Instantiate (zPosRightTurn, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (0).transform.GetChild (0).position, zPosRightTurn.transform.rotation);
                                        newEdge.Add (turn2);
                                        edgeDirections.Add ("+x");
                                        break;
                                    case 6:
                                        //T-section
                                        GameObject t = Instantiate (zPosT, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (0).transform.GetChild (0).position, zPosT.transform.rotation);
                                        newEdge.Add (t);
                                        edgeDirections.Add ("-x");
                                    break;
                                }
                                break;
                            case "Z-":
                                switch (Random.Range (1, 7)) {
                                    case 1:
                                        //Room
                                        GameObject _room = Instantiate (room, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (1).transform.GetChild (1).position, room.transform.rotation);
                                        Destroy (_room.transform.Find ("Z+Wall").gameObject);
                                        newEdge.Add (_room);
                                        edgeDirections.Add ("-z");
                                        break;
                                    case 2:
                                        //Short hallway
                                        GameObject hallway = Instantiate (zShortHallway, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (1).transform.GetChild (0).position, zShortHallway.transform.rotation);
                                        newEdge.Add (hallway);
                                        edgeDirections.Add ("-z");
                                        break;
                                    case 3:
                                        //Long hallway
                                        GameObject lHallway = Instantiate (zLongHallway, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (1).transform.GetChild (1).position, zLongHallway.transform.rotation);
                                        newEdge.Add (lHallway);
                                        edgeDirections.Add ("-z");
                                        break;
                                    case 4:
                                        //Turn left
                                        GameObject turn = Instantiate (zNegLeftTurn, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (1).transform.GetChild (0).position, zNegLeftTurn.transform.rotation);
                                        newEdge.Add (turn);
                                        edgeDirections.Add ("+x");
                                        break;
                                    case 5:
                                        //Turn right
                                        GameObject turn2 = Instantiate (zNegRightTurn, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (1).transform.GetChild (0).position, zNegRightTurn.transform.rotation);
                                        newEdge.Add (turn2);
                                        edgeDirections.Add ("-x");
                                        break;
                                    case 6:
                                        //T-section
                                        GameObject t = Instantiate (zNegT, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (1).transform.GetChild (0).position, zNegT.transform.rotation);
                                        newEdge.Add (t);
                                        edgeDirections.Add ("+x");
                                        break;
                                }
                                break;
                            case "X+":
                                switch (Random.Range (1, 7)) {
                                    case 1:
                                        //Room
                                        GameObject _room = Instantiate (room, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (2).transform.GetChild (1).position, room.transform.rotation);
                                        Destroy (_room.transform.Find ("X-Wall").gameObject);
                                        newEdge.Add (_room);
                                        edgeDirections.Add ("+x");
                                        break;
                                    case 2:
                                        //Short hallway
                                        GameObject hallway = Instantiate (xShortHallway, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (2).transform.GetChild (0).position, xShortHallway.transform.rotation);
                                        newEdge.Add (hallway);
                                        edgeDirections.Add ("+x");
                                        break;
                                    case 3:
                                        //Long hallway
                                        GameObject lHallway = Instantiate (xLongHallway, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (2).transform.GetChild (1).position, xLongHallway.transform.rotation);
                                        newEdge.Add (lHallway);
                                        edgeDirections.Add ("+x");
                                        break;
                                    case 4:
                                        //Turn left
                                        GameObject turn = Instantiate (xPosLeftTurn, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (2).transform.GetChild (0).position, xPosLeftTurn.transform.rotation);
                                        newEdge.Add (turn);
                                        edgeDirections.Add ("+z");
                                        break;
                                    case 5:
                                        //Turn right
                                        GameObject turn2 = Instantiate (xPosRightTurn, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (2).transform.GetChild (0).position, xPosRightTurn.transform.rotation);
                                        newEdge.Add (turn2);
                                        edgeDirections.Add ("-z");
                                        break;
                                    case 6:
                                        //T-section
                                        GameObject t = Instantiate (xPosT, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (2).transform.GetChild (0).position, xPosT.transform.rotation);
                                        newEdge.Add (t);
                                        edgeDirections.Add ("-z");
                                        break;
                                }
                                break;
                            case "X-":
                                switch (Random.Range (1, 7)) {
                                    case 1:
                                        //Room
                                        GameObject _room = Instantiate (room, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (3).transform.GetChild (1).position, room.transform.rotation);
                                        Destroy (_room.transform.Find ("X+Wall").gameObject);
                                        newEdge.Add (_room);
                                        edgeDirections.Add ("-x");
                                        break;
                                    case 2:
                                        //Short hallway
                                        GameObject hallway = Instantiate (xShortHallway, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (3).transform.GetChild (0).position, xShortHallway.transform.rotation);
                                        newEdge.Add (hallway);
                                        edgeDirections.Add ("-x");
                                        break;
                                    case 3:
                                        //Long hallway
                                        GameObject lHallway = Instantiate (xLongHallway, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (3).transform.GetChild (1).position, xLongHallway.transform.rotation);
                                        newEdge.Add (lHallway);
                                        edgeDirections.Add ("-x");
                                        break;
                                    case 4:
                                        //Turn left
                                        GameObject turn = Instantiate (xNegLeftTurn, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (3).transform.GetChild (0).position, xNegLeftTurn.transform.rotation);
                                        newEdge.Add (turn);
                                        edgeDirections.Add ("-z");
                                        break;
                                    case 5:
                                        //Turn right
                                        GameObject turn2 = Instantiate (xNegRightTurn, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (3).transform.GetChild (0).position, xNegRightTurn.transform.rotation);
                                        newEdge.Add (turn2);
                                        edgeDirections.Add ("+z");
                                        break;
                                    case 6:
                                        //T-section
                                        GameObject t = Instantiate (xNegT, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (3).transform.GetChild (0).position, xNegT.transform.rotation);
                                        newEdge.Add (t);
                                        edgeDirections.Add ("+z");
                                        break;
                                }
                                break;
                        }
                    }
                } else {
                    switch (newPieceDirection) {
                        case "Z+":
                            switch (Random.Range (1, 7)) {
                                case 1:
                                    //Room
                                    GameObject _room = Instantiate (room, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (0).transform.GetChild (1).position, room.transform.rotation);
                                    Destroy (_room.transform.Find ("Z-Wall").gameObject);
                                    newEdge.Add (_room);
                                    //Direction is added but ignored, need it for a index spacer
                                    edgeDirections.Add ("+z");
                                    break;
                                case 2:
                                    //Short hallway
                                    GameObject hallway = Instantiate (zShortHallway, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (0).transform.GetChild (0).position, zShortHallway.transform.rotation);
                                    newEdge.Add (hallway);
                                    edgeDirections.Add ("+z");
                                    break;
                                case 3:
                                    //Long hallway
                                    GameObject lHallway = Instantiate (zLongHallway, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (0).transform.GetChild (1).position, zLongHallway.transform.rotation);
                                    newEdge.Add (lHallway);
                                    edgeDirections.Add ("+z");
                                    break;
                                case 4:
                                    //Turn left
                                    GameObject turn = Instantiate (zPosLeftTurn, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (0).transform.GetChild (0).position, zPosLeftTurn.transform.rotation);
                                    newEdge.Add (turn);
                                    edgeDirections.Add ("-x");
                                    break;
                                case 5:
                                    //Turn right
                                    GameObject turn2 = Instantiate (zPosRightTurn, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (0).transform.GetChild (0).position, zPosRightTurn.transform.rotation);
                                    newEdge.Add (turn2);
                                    edgeDirections.Add ("+x");
                                    break;
                                case 6:
                                    //T-section
                                    GameObject t = Instantiate (zPosT, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (0).transform.GetChild (0).position, zPosT.transform.rotation);
                                    newEdge.Add (t);
                                    edgeDirections.Add ("-x");
                                break;
                            }
                            break;
                        case "Z-":
                            switch (Random.Range (1, 7)) {
                                case 1:
                                    //Room
                                    GameObject _room = Instantiate (room, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (0).transform.GetChild (1).position, room.transform.rotation);
                                    Destroy (_room.transform.Find ("Z+Wall").gameObject);
                                    newEdge.Add (_room);
                                    edgeDirections.Add ("-z");
                                    break;
                                case 2:
                                    //Short hallway
                                    GameObject hallway = Instantiate (zShortHallway, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (0).transform.GetChild (0).position, zShortHallway.transform.rotation);
                                    newEdge.Add (hallway);
                                    edgeDirections.Add ("-z");
                                    break;
                                case 3:
                                    //Long hallway
                                    GameObject lHallway = Instantiate (zLongHallway, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (0).transform.GetChild (1).position, zLongHallway.transform.rotation);
                                    newEdge.Add (lHallway);
                                    edgeDirections.Add ("-z");
                                    break;
                                case 4:
                                    //Turn left
                                    GameObject turn = Instantiate (zNegLeftTurn, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (0).transform.GetChild (0).position, zNegLeftTurn.transform.rotation);
                                    newEdge.Add (turn);
                                    edgeDirections.Add ("+x");
                                    break;
                                case 5:
                                    //Turn right
                                    GameObject turn2 = Instantiate (zNegRightTurn, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (0).transform.GetChild (0).position, zNegRightTurn.transform.rotation);
                                    newEdge.Add (turn2);
                                    edgeDirections.Add ("-x");
                                    break;
                                case 6:
                                    //T-section
                                    GameObject t = Instantiate (zNegT, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (0).transform.GetChild (0).position, zNegT.transform.rotation);
                                    newEdge.Add (t);
                                    edgeDirections.Add ("+x");
                                    break;
                            }
                            break;
                        case "X+":
                            switch (Random.Range (1, 7)) {
                                case 1:
                                    //Room
                                    GameObject _room = Instantiate (room, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (0).transform.GetChild (1).position, room.transform.rotation);
                                    Destroy (_room.transform.Find ("X-Wall").gameObject);
                                    newEdge.Add (_room);
                                    edgeDirections.Add ("+x");
                                    break;
                                case 2:
                                    //Short hallway
                                    GameObject hallway = Instantiate (xShortHallway, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (0).transform.GetChild (0).position, xShortHallway.transform.rotation);
                                    newEdge.Add (hallway);
                                    edgeDirections.Add ("+x");
                                    break;
                                case 3:
                                    //Long hallway
                                    GameObject lHallway = Instantiate (xLongHallway, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (0).transform.GetChild (1).position, xLongHallway.transform.rotation);
                                    newEdge.Add (lHallway);
                                    edgeDirections.Add ("+x");
                                    break;
                                case 4:
                                    //Turn left
                                    GameObject turn = Instantiate (xPosLeftTurn, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (0).transform.GetChild (0).position, xPosLeftTurn.transform.rotation);
                                    newEdge.Add (turn);
                                    edgeDirections.Add ("+z");
                                    break;
                                case 5:
                                    //Turn right
                                    GameObject turn2 = Instantiate (xPosRightTurn, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (0).transform.GetChild (0).position, xPosRightTurn.transform.rotation);
                                    newEdge.Add (turn2);
                                    edgeDirections.Add ("-z");
                                    break;
                                case 6:
                                    //T-section
                                    GameObject t = Instantiate (xPosT, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (0).transform.GetChild (0).position, xPosT.transform.rotation);
                                    newEdge.Add (t);
                                    edgeDirections.Add ("-z");
                                    break;
                            }
                            break;
                        case "X-":
                            switch (Random.Range (1, 7)) {
                                case 1:
                                    //Room
                                    GameObject _room = Instantiate (room, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (0).transform.GetChild (1).position, room.transform.rotation);
                                    Destroy (_room.transform.Find ("X+Wall").gameObject);
                                    newEdge.Add (_room);
                                    edgeDirections.Add ("-x");
                                    break;
                                case 2:
                                    //Short hallway
                                    GameObject hallway = Instantiate (xShortHallway, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (0).transform.GetChild (0).position, xShortHallway.transform.rotation);
                                    newEdge.Add (hallway);
                                    edgeDirections.Add ("-x");
                                    break;
                                case 3:
                                    //Long hallway
                                    GameObject lHallway = Instantiate (xLongHallway, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (0).transform.GetChild (1).position, xLongHallway.transform.rotation);
                                    newEdge.Add (lHallway);
                                    edgeDirections.Add ("-x");
                                    break;
                                case 4:
                                    //Turn left
                                    GameObject turn = Instantiate (xNegLeftTurn, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (0).transform.GetChild (0).position, xNegLeftTurn.transform.rotation);
                                    newEdge.Add (turn);
                                    edgeDirections.Add ("-z");
                                    break;
                                case 5:
                                    //Turn right
                                    GameObject turn2 = Instantiate (xNegRightTurn, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (0).transform.GetChild (0).position, xNegRightTurn.transform.rotation);
                                    newEdge.Add (turn2);
                                    edgeDirections.Add ("+z");
                                    break;
                                case 6:
                                    //T-section
                                    GameObject t = Instantiate (xNegT, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (0).transform.GetChild (0).position, xNegT.transform.rotation);
                                    newEdge.Add (t);
                                    edgeDirections.Add ("+z");
                                    break;
                            }
                            break;
                    }
                }
            }
            currentEdge.Clear ();
            foreach (GameObject thing in newEdge) {
                currentEdge.Add (thing);
            }
            edgeDirections.Clear ();
            newEdge.Clear ();
        }
    }

}