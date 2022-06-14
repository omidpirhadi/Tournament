
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BilliardLevelCreator : MonoBehaviour
{
    public int index = 0;

    public GameObject ballwhite_prefabs;
    public GameObject colorball_prefabs;
    public GameObject sibl_prefabs;

    public Button ballwhite_button;
    public Button colorball_button;
    public Button sibl_button;
    public Button reset_button;

    public TMPro.TMP_Text Position_Debug;

    private List<GameObject> objects = new List<GameObject>();
    private GameObject obj;
    private RaycastHit hit;
    void Start()
    {

        ballwhite_button.onClick.AddListener(() => { index = 0; });
        colorball_button.onClick.AddListener(() => { index = 1; });
        sibl_button.onClick.AddListener(() => { index = 2; });
        reset_button.onClick.AddListener(() => { Reset(); });
    }
    void Update()

    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100000000))
            {
                if (hit.collider)
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
            obj = Instantiate(ballwhite_prefabs, pos, Quaternion.identity);
        else if (index == 1)
            obj = Instantiate(colorball_prefabs, pos, Quaternion.identity);
        else if (index == 2)
            obj = Instantiate(sibl_prefabs, pos, Quaternion.identity);

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
