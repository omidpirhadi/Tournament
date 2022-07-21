using System;

using UnityEngine;
using UnityEngine.UI;
public class PopupHint : MonoBehaviour
{
    public Image hint_image;
    public Button close_button;
    public void SetHint(string Hint)
    {
        hint_image.sprite = ConvertImageToSprite(Hint);
        close_button.onClick.AddListener(() => { gameObject.SetActive(false); });
    }
    private Sprite ConvertImageToSprite(string image)
    {

        var image_byte = Convert.FromBase64String(image);
        Texture2D texture = new Texture2D(512, 512, TextureFormat.DXT5, false);
        texture.LoadImage(image_byte);
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
}
