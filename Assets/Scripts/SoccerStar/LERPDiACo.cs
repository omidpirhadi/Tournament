using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LERPDiACo : MonoBehaviour
{
    public Transform t;
    public float maxdis = 00.2f;
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, t.position, maxdis);
            
            }
}
