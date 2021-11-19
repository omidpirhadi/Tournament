using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Func1());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public  IEnumerator Func1()
    {
      /*  for (int x = 0; x < 10; x++)
        {
            yield return new WaitForSecondsRealtime(2f);
                Debug.Log($"Func1{x}");
            
        }
        Debug.Log("Finish1");*/
        StartCoroutine(Func2());
        StartCoroutine(Func3());
        yield return null;
        Debug.Log("FinishMain");
    }
    public IEnumerator Func2()
    {
        for (int x = 0; x < 10; x++)
        {
            yield return new WaitForSecondsRealtime(2f);
            Debug.Log($"Func2{x}");

        }
        Debug.Log("Finish2");
       // yield return null;
    }
    public IEnumerator Func3()
    {
        for (int x = 0; x < 10; x++)
        {
            yield return new WaitForSecondsRealtime(2f);
            Debug.Log($"Func3{x}");

        }
        Debug.Log("Finish3");
       // yield return null;
    }
}
