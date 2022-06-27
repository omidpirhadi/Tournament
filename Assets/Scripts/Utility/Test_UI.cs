using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class Test_UI : MonoBehaviour
{
   

    private void Start()
    {
        Aa();
        Xx();
        Cc();
        
    }

    public IEnumerator A()
    {
        int a = 0;
        while (a<1000)
        {
            Debug.Log("A:"+a);
            a++;
        }
        yield return null;
    }
    public IEnumerator B()
    {
        int a = 0;
        while (a < 1000)
        {
            Debug.Log("B:" + a);
            a++;
        }
        yield return null;
    }
    public IEnumerator C()
    {
        int a = 0;
        while (a < 1000)
        {
            Debug.Log("C:" + a);
            a++;
        }
        yield return null;
    }
    public void Aa()
    {
        Task.Run(() =>
        {
            int a = 0;
            while (a < 1000)
            {
                Debug.Log("Aa:" + a);
                a++;
            }
        });
        /// yield return null;
    }
    public async void Bb()
    {
        await Task.Run(() =>
         {
             int a = 0;
             while (a < 1000)
             {
                 Debug.Log("Bb:" + a);
                 a++;
             }
         });
       // await Cc();
       //// yield return null;
    }
    public  Task Cc()
    {
        Task task = Task.Run(() =>
        {
            int a = 0;
            while (a < 1000)
            {
                Debug.Log("Cc:" + a);
                a++;
            }
        });
        return task;

        /// yield return null;
    }
    public Task Xx()
    {
        Task task = Task.Run(() =>
        {
            int a = 0;
            while (a < 1000)
            {
                Debug.Log("Xx:" + a);
                a++;
            }
        });
        return task;

        /// yield return null;
    }
}
