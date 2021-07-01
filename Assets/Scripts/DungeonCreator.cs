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
    public List<GameObject> currentEdge = new List<GameObject> ();
    private List<GameObject> newEdge = new List<GameObject> ();

    private string newPieceDirection;
    private GameObject dungeonHead;
    private List<string> nextPieceDirections = new List<string> ();
    public List<string> pieceDirections = new List<string> ();

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

        int j = 0;
        for (int i = 0; i < iterations; i++) {
            Debug.Log ("Iteration " + (i + 1));

            foreach (GameObject edge in currentEdge) {
                List<string> noWallHere = new List<string> ();
                if (edge.name.Contains ("Room")) {
                    //Ignore the direction it came from
                    dungeonHead.transform.position = new Vector3 (edge.transform.position.x, edge.transform.position.y + 1f, edge.transform.position.z);
                    RaycastHit hit;
                    //Positive z
                    if (Physics.Raycast (dungeonHead.transform.position, dungeonHead.transform.forward, out hit, 10f)) {
                        //There's a wall here
                    } else {
                        noWallHere.Add ("+z");
                    }

                    //Negative z
                    if (Physics.Raycast (dungeonHead.transform.position, -dungeonHead.transform.forward, out hit, 10f)) {
                        //There's a wall here
                    } else {
                       noWallHere.Add ("-z");
                    }

                    //Positve x
                    if (Physics.Raycast (dungeonHead.transform.position, dungeonHead.transform.right, out hit, 10f)) {
                        //There's a wall here
                    } else {
                        noWallHere.Add ("+x");
                    }

                    //Negative x
                    if (Physics.Raycast (dungeonHead.transform.position, -dungeonHead.transform.right, out hit, 10f)) {
                        //There's a wall here
                    } else {
                        noWallHere.Add ("-x");
                    }

                    if (noWallHere.Count > 0) {
                        foreach (string direction in noWallHere) {
                            SpawnPiece (edge, direction);
                        }
                    }
                } else {
                    SpawnPiece (edge, pieceDirections[j]);
                }
                j++;
            }
            if (i !> 0) {
                currentEdge.Clear ();
                foreach (GameObject thing in newEdge) {
                    currentEdge.Add (thing);
                }
                pieceDirections.Clear ();
                foreach (string thing in nextPieceDirections) {
                    pieceDirections.Add (thing);
                }
                nextPieceDirections.Clear ();
                newEdge.Clear ();
            }
            j = 0;
        }
    }

    private void SpawnPiece (GameObject edge, string direction) {
        switch (direction) {
            case "+z":
                switch (Random.Range (1, 6)) {
                    case 1:
                        //Room
                        GameObject _room = Instantiate (room, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (0).transform.GetChild (1).position, room.transform.rotation);
                        Destroy (_room.transform.Find ("Z-Wall").gameObject);
                        newEdge.Add (_room);
                        //Direction is added but ignored, need it for a index spacer
                        nextPieceDirections.Add ("+z");
                        break;
                    case 2:
                        //Short hallway
                        GameObject hallway = Instantiate (zShortHallway, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (0).transform.GetChild (0).position, zShortHallway.transform.rotation);
                        newEdge.Add (hallway);
                        nextPieceDirections.Add ("+z");
                        break;
                    case 3:
                        //Long hallway
                        GameObject lHallway = Instantiate (zLongHallway, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (0).transform.GetChild (1).position, zLongHallway.transform.rotation);
                        newEdge.Add (lHallway);
                        nextPieceDirections.Add ("+z");
                        break;
                    case 4:
                        //Turn left
                        GameObject turn = Instantiate (zPosLeftTurn, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (0).transform.GetChild (1).position, zPosLeftTurn.transform.rotation);
                        newEdge.Add (turn);
                        nextPieceDirections.Add ("-x");
                        break;
                    case 5:
                        //Turn right
                        GameObject turn2 = Instantiate (zPosRightTurn, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (0).transform.GetChild (1).position, zPosRightTurn.transform.rotation);
                        newEdge.Add (turn2);
                        nextPieceDirections.Add ("+x");
                        break;
                }
                break;
            case "-z":
                switch (Random.Range (1, 6)) {
                    case 1:
                        //Room
                        GameObject _room = Instantiate (room, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (1).transform.GetChild (1).position, room.transform.rotation);
                        Destroy (_room.transform.Find ("Z+Wall").gameObject);
                        newEdge.Add (_room);
                        nextPieceDirections.Add ("-z");
                        break;
                    case 2:
                        //Short hallway
                        GameObject hallway = Instantiate (zShortHallway, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (1).transform.GetChild (0).position, zShortHallway.transform.rotation);
                        newEdge.Add (hallway);
                        nextPieceDirections.Add ("-z");
                        break;
                    case 3:
                        //Long hallway
                        GameObject lHallway = Instantiate (zLongHallway, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (1).transform.GetChild (1).position, zLongHallway.transform.rotation);
                        newEdge.Add (lHallway);
                        nextPieceDirections.Add ("-z");
                        break;
                    case 4:
                        //Turn left
                        GameObject turn = Instantiate (zNegLeftTurn, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (1).transform.GetChild (1).position, zNegLeftTurn.transform.rotation);
                        newEdge.Add (turn);
                        nextPieceDirections.Add ("+x");
                        break;
                    case 5:
                        //Turn right
                        GameObject turn2 = Instantiate (zNegRightTurn, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (1).transform.GetChild (1).position, zNegRightTurn.transform.rotation);
                        newEdge.Add (turn2);
                        nextPieceDirections.Add ("-x");
                        break;
                }
                break;
            case "+x":
                switch (Random.Range (1, 6)) {
                    case 1:
                        //Room
                        GameObject _room = Instantiate (room, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (2).transform.GetChild (1).position, room.transform.rotation);
                        Destroy (_room.transform.Find ("X-Wall").gameObject);
                        newEdge.Add (_room);
                        nextPieceDirections.Add ("+x");
                        break;
                    case 2:
                        //Short hallway
                        GameObject hallway = Instantiate (xShortHallway, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (2).transform.GetChild (0).position, xShortHallway.transform.rotation);
                        newEdge.Add (hallway);
                        nextPieceDirections.Add ("+x");
                        break;
                    case 3:
                        //Long hallway
                        GameObject lHallway = Instantiate (xLongHallway, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (2).transform.GetChild (1).position, xLongHallway.transform.rotation);
                        newEdge.Add (lHallway);
                        nextPieceDirections.Add ("+x");
                        break;
                    case 4:
                        //Turn left
                        GameObject turn = Instantiate (xPosLeftTurn, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (2).transform.GetChild (1).position, xPosLeftTurn.transform.rotation);
                        newEdge.Add (turn);
                        nextPieceDirections.Add ("+z");
                        break;
                    case 5:
                        //Turn right
                        GameObject turn2 = Instantiate (xPosRightTurn, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (2).transform.GetChild (1).position, xPosRightTurn.transform.rotation);
                        newEdge.Add (turn2);
                        nextPieceDirections.Add ("-z");
                        break;
                }
                break;
            case "-x":
                switch (Random.Range (1, 6)) {
                    case 1:
                        //Room
                        GameObject _room = Instantiate (room, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (3).transform.GetChild (1).position, room.transform.rotation);
                        Destroy (_room.transform.Find ("X+Wall").gameObject);
                        newEdge.Add (_room);
                        nextPieceDirections.Add ("-x");
                        break;
                    case 2:
                        //Short hallway
                        GameObject hallway = Instantiate (xShortHallway, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (3).transform.GetChild (0).position, xShortHallway.transform.rotation);
                        newEdge.Add (hallway);
                        nextPieceDirections.Add ("-x");
                        break;
                    case 3:
                        //Long hallway
                        GameObject lHallway = Instantiate (xLongHallway, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (3).transform.GetChild (1).position, xLongHallway.transform.rotation);
                        newEdge.Add (lHallway);
                        nextPieceDirections.Add ("-x");
                        break;
                    case 4:
                        //Turn left
                        GameObject turn = Instantiate (xNegLeftTurn, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (3).transform.GetChild (1).position, xNegLeftTurn.transform.rotation);
                        newEdge.Add (turn);
                        nextPieceDirections.Add ("-z");
                        break;
                    case 5:
                        //Turn right
                        GameObject turn2 = Instantiate (xNegRightTurn, edge.GetComponent<DungeonPieceData> ().snaps.transform.GetChild (3).transform.GetChild (1).position, xNegRightTurn.transform.rotation);
                        newEdge.Add (turn2);
                        nextPieceDirections.Add ("+z");
                        break;
                }
            break;
        }
    }

}