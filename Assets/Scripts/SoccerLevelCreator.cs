
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SoccerLevelCreator : MonoBehaviour
{
    public int index = 0;

    public GameObject blue_marble_prefabs;
    public GameObject red_marble_prefabs;
    public GameObject ball_prefabs;

    public Button blue_marble_button;
    public Button red_marble_button;
    public Button ball_button;
    public Button reset_button;

    public TMPro.TMP_Text Position_Debug;

    private List<GameObject> objects= new List<GameObject>();
    private GameObject obj;
    private RaycastHit hit;
    void Start()
    {
        
        blue_marble_button.onClick.AddListener(() => { index = 0; });
        red_marble_button.onClick.AddListener(() => { index = 1; });
        ball_button.onClick.AddListener(() => { index = 2; });
        reset_button.onClick.AddListener(() => { Reset(); });
    }
    void Update()

    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100000000))
            {
                if(hit.collider)
                {
                    SpwanPrefab(new Vector3(hit.point.x, 2, hit.point.z));
                    Position_Debug.text = $"X = {hit.point.x}  Y = {2}  Z = {hit.point.z}";
                }
            }
        }
    }
    void SpwanPrefab(Vector3 pos)
    {
        
        if (index == 0)
          obj =  Instantiate(blue_marble_prefabs, pos, Quaternion.identity);
        else if (index == 1)
          obj =  Instantiate(red_marble_prefabs, pos, Quaternion.identity);
        else if (index == 2)
         obj =   Instantiate(ball_prefabs, pos, Quaternion.identity);

        objects.Add(obj);
    }
    void Reset()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            Destroy(objects[i]);
        }
    }
} 