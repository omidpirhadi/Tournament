using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
public class PopupHintStatic : MonoBehaviour
{

    public Image hint_image;
    public Button close_button;
    public RectTransform content;
    [SerializeField] private List<ImageHints> ImageHints = new List<ImageHints>();
    private List<Image> listimages = new List<Image>(3);
    

    public void Init_Hint(int count, params string[] data)
    {


        if (listimages.Count == 1)
        {
            hint_image.sprite = null;
            Destroy(listimages[0].gameObject);
        }
        else if (listimages.Count == 2)
        {
            hint_image.sprite = null;
            Destroy(listimages[0].gameObject);
            Destroy(listimages[1].gameObject);
            ///  Destroy(listimages[2].gameObject);
        }
        else
        {
            hint_image.sprite = null;
        }


        listimages.Clear();



        if (count == 1)
        {

            hint_image.sprite = LoadImageFromList(data[0]);


        }
        else if (count == 2)
        {
            hint_image.sprite = LoadImageFromList(data[0]);

            var image1 = Instantiate(hint_image, content);

            image1.sprite = LoadImageFromList(data[1]);

            listimages.Add(image1);


        }
        else if (count == 3)
        {
            hint_image.sprite = LoadImageFromList(data[0]);

            var image1 = Instantiate(hint_image, content);
            var image2 = Instantiate(hint_image, content);

            image1.sprite = LoadImageFromList(data[1]);
            image2.sprite = LoadImageFromList(data[2]);

            listimages.Add(image1);
            listimages.Add(image2);
        }

        close_button.onClick.AddListener(() => { this.gameObject.SetActive(false); });
    }

    private  Sprite LoadImageFromList(string name)
    {
        Sprite s = null;
        for (int i = 0; i < ImageHints.Count; i++)
        {
            if(ImageHints[i].name == name)
            {
                s = ImageHints[i].sprite;
            }
        }
        return s;
    }

    private List<Texture2D> temp_texture = new List<Texture2D>();
    private List<Sprite> temp_sprite = new List<Sprite>();
    private void ClearMemoryTextures()
    {
        if (temp_texture.Count > 0)
        {
            for (int i = 0; i < temp_texture.Count; i++)
            {
                Destroy(temp_texture[i]);
                Destroy(temp_sprite[i]);
            }
            temp_texture.Clear();
            temp_sprite.Clear();
        }
    }

    private void OnDisable()
    {
        ClearMemoryTextures();
    }
    private void OnDestroy()
    {
        ClearMemoryTextures();
    }

}
[Serializable]
public struct ImageHints
{
    public string name;
    public Sprite sprite;
}