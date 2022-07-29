using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
public class PopupHint : MonoBehaviour
{
    public Image hint_image;
    public Button close_button;
    public RectTransform content;

    private List<Image> listimages = new List<Image>(3);
    public void SetHint(string Hint)
    {
        hint_image.sprite = ConvertImageToSprite(Hint);
      //  close_button.onClick.AddListener(() => { this.gameObject.SetActive(false); });
    }
    [Button("test", ButtonSizes.Medium)]
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
            
            hint_image.sprite = ConvertImageToSprite(data[0]);
          

        }
        else if (count == 2)
        {
            hint_image.sprite = ConvertImageToSprite(data[0]);
          
            var image1 = Instantiate(hint_image, content);
          
            image1.sprite = ConvertImageToSprite(data[1]);
          
            listimages.Add(image1);
          

        }
        else if (count == 3)
        {
            hint_image.sprite = ConvertImageToSprite(data[0]);
            
            var image1 = Instantiate(hint_image, content);
            var image2 = Instantiate(hint_image, content);
           
            image1.sprite = ConvertImageToSprite(data[1]);
            image2.sprite = ConvertImageToSprite(data[2]);
            
            listimages.Add(image1);
            listimages.Add(image2);
        }
        
        close_button.onClick.AddListener(() => { this.gameObject.SetActive(false); });
    }
    private Sprite ConvertImageToSprite(string image)
    {

        var image_byte = Convert.FromBase64String(image);
        Texture2D texture = new Texture2D(512, 512, TextureFormat.DXT5, false);
        texture.LoadImage(image_byte);
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

}
