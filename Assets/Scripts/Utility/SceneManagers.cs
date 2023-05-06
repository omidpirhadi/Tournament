using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneManagers : MonoBehaviour
{

    

    AsyncOperation asyncOperation;
    [Sirenix.OdinInspector.Button("ChangeScene")]
    public void loadlevel(string name)
    {
        StartCoroutine(SceneLoad(name));
    }

    private IEnumerator SceneLoad(string name)
    {
       
         asyncOperation =  SceneManager.LoadSceneAsync(name,LoadSceneMode.Single);
        asyncOperation.allowSceneActivation = false;

       // asyncOperation.completed += AsyncOperation_completed;
       // Debug.Log(asyncop.progress * 0.01f);
     /*   if(asyncop.isDone)
        {
           asyncop.allowSceneActivation = true;

        }*/
        while(asyncOperation.isDone == false)
        {
            Debug.Log(asyncOperation.progress );
            if (asyncOperation.progress >= 0.9f)
            {
               asyncOperation.allowSceneActivation = true;
            }

            
           
            yield return null;
        }
        
    }

   /* private void AsyncOperation_completed(AsyncOperation obj)
    {
        Debug.Log("aA");
        
    }

    private IEnumerator SceneLoad2(string name)
    {

        var asyncop = SceneManager.LoadSceneAsync(name, LoadSceneMode.Single);
       asyncop.allowSceneActivation = true;

        yield return asyncop;
        Debug.Log(asyncop.progress * 0.01f);
        if (asyncop.isDone)
        {
            asyncop.allowSceneActivation = true;

        }

    }*/

}
