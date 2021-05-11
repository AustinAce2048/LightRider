using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    private Transform[] children;
    private List<GameObject> childObjects;
    private int side;
    public float rotateRate = 5, rotateTimer;

    // Start is called before the first frame update
    void Start()
    {
        GetChildren();
    }

    // Update is called once per frame
    void Update()
    {
        if(rotateTimer > 0)
        {
            rotateTimer -= Time.deltaTime;
        }
        if(rotateTimer < 0)
        {
            rotateTimer = 0;
        }
        if(rotateTimer == 0)
        {
            side = Random.Range(1, childObjects.Count);
            //print(childObjects[side].name);
            childObjects[side].GetComponent<MeshCollider>().enabled = false;
            childObjects[side].GetComponent<MeshCollider>().enabled = true;
            rotateTimer = rotateRate;
        }
    }

    // Put all children in a list
    private void GetChildren()
    {
        children = GetComponentsInChildren<Transform>();
        childObjects = new List<GameObject>();
        foreach (Transform child in children)
        {
            childObjects.Add(child.gameObject);
        }
    }
}
