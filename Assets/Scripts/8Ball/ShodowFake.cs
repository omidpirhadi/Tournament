using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShodowFake : MonoBehaviour
{
    public Transform table;
    public Transform shadow;
    public Transform Parent;
    public float Offset = 0.04f;
  
    private void Start()
    {
        
        table = GameObject.Find("TablePos").transform;
        if (GameObject.Find("BilliardGame(Clone)"))
            Parent = GameObject.Find("BilliardGame(Clone)").transform;
        if (GameObject.Find("BilliardGame"))
            Parent = GameObject.Find("BilliardGame").transform;
        shadow.parent = Parent;
    }
    private void Update()
    {
        Vector3 dir = (transform.position - table.transform.position);
//shadow.transform.rotation = Quaternion.Euler(90, 0, 0);

       // Debug.Log(dir);
        //  Vector3 newPos = transform.position + dir;
        // newPos.x = transform.position.x + OffsetX;
        var newPos = new Vector3(0.0f,0.0f,0.0f);
        newPos.x = dir.x * Offset;
        newPos.y = dir.y;
        newPos.z = dir.z * Offset;
        if (shadow)
            shadow.transform.position = newPos;
        
    }

    private void OnDestroy()
    {
        Destroy(shadow.gameObject);
    }
}
