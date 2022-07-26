using System;

using UnityEngine;
using UnityEngine.UI;
public class PopupHint : MonoBehaviour
{
    public Image hint_image;
    public Button close_button;
    public RectTransform content;
    public void SetHint(string Hint)
    {
        hint_image.sprite = ConvertImageToSprite(Hint);
        close_button.onClick.AddListener(() => { gameObject.SetActive(false); });
    }
    public void Init_Hint(int count, params string[] data)
    {
        if (count == 1)
        {
            
            hint_image.sprite = ConvertImageToSprite(data[0]);
        }
        else if (count == 2)
        {
            hint_image.sprite = ConvertImageToSprite(data[0]);
          //  var image = Instantiate(hint_image, content);
            var image1 = Instantiate(hint_image, content);
          //  image.sprite = ConvertImageToSprite(data[0]);
            image1.sprite = ConvertImageToSprite(data[1]);
        }
        else if (count == 3)
        {
            hint_image.sprite = ConvertImageToSprite(data[0]);
            //var image = Instantiate(hint_image, content);
            var image1 = Instantiate(hint_image, content);
            var image2 = Instantiate(hint_image, content);
            //image.sprite = ConvertImageToSprite(data[0]);
            image1.sprite = ConvertImageToSprite(data[1]);
            image2.sprite = ConvertImageToSprite(data[2]);
        }
    }
    private Sprite ConvertImageToSprite(string image)
    {

        var image_byte = Convert.FromBase64String(image);
        Texture2D texture = new Texture2D(512, 512, TextureFormat.DXT5, false);
        texture.LoadImage(image_byte);
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
}
