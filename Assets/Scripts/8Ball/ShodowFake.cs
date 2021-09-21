using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShodowFake : MonoBehaviour
{
    public Transform table;
    public Transform shadow;
    public float OffsetX = 0.04f;
    public float OffsetZ = 0.04f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = (transform.position - table.transform.position);


        //Debug.Log(dir);
        //  Vector3 newPos = transform.position + dir;
        // newPos.x = transform.position.x + OffsetX;
        var newPos = new Vector3();
        newPos.x = dir.x * OffsetX;
        newPos.z = dir.z * OffsetX;
        shadow.transform.position = newPos;
        shadow.transform.rotation = Quaternion.Euler(90, 0, 0);
    }
    private void LateUpdate()
    {

       /* Vector3 dir = (transform.position - table.transform.position);


        //Debug.Log(dir);
        //  Vector3 newPos = transform.position + dir;
        // newPos.x = transform.position.x + OffsetX;
        var newPos = new Vector3();
        newPos.x = dir.x * OffsetX;
        newPos.z = dir.z * OffsetX;
        shadow.transform.position = newPos;
        shadow.transform.rotation = Quaternion.Euler(90, 0, 0);*/
    }
}
