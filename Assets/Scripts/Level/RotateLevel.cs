using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLevel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("cube"))
        {
            //Debug.Log(gameObject.name + " " + gameObject.transform.up);
            //Debug.Log(gameObject.name + " " + other.gameObject.name);
            other.gameObject.transform.SetParent(gameObject.transform);
        }
        //gameObject.GetComponent<MeshCollider>().enabled = false;
    }
}