using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{


    public Transform treeTop;
    Vector3 offset = new Vector3(0, -6, -10);
    

    // Update is called once per frame
    void Update()
    {
        transform.position -= (transform.position - (treeTop.position + offset))/0.1f * Time.deltaTime;
    }
}
