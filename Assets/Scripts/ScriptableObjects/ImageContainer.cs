using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Diaco.ImageContainerTool
{
    [CreateAssetMenu(fileName = "ImageContainer", menuName = "DiacoTools/ImageContainer", order = 0)]
    public class ImageContainer : ScriptableObject
    {
        public List<ImageContainerInfo> imageContainers;
        public static Texture2D texture;
        public static void InitializeTexture()
        {
            texture = new Texture2D(512, 512);
           // Debug.Log("*************TextureInitialize");
        }
        public Sprite LoadImage(string name)
        {
    
            imageContainers.ForEach((e) => {
                if(e.name == name)
                {
                    texture = e.Texture;
                    
                }
                
            });
         
            if(name == "")
            {
                texture = imageContainers[0].Texture;
            }
            var s = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
           // temp_sprite.Add(s);
            return s;
        }
       /* private static List<Sprite> temp_sprite = new List<Sprite>();*/
        public static void ClearMemoryTextures()
        {
            /*if (temp_sprite.Count > 0)
            {
                for (int i = 0; i < temp_sprite.Count; i++)
                {
                    
                    Destroy(temp_sprite[i]);
                }
             
                temp_sprite.Clear();
            }*/
        }




        /*public Texture2D LoadSkinMarble(string name)
        {
            texture = new Texture2D(512, 512);
            imageContainers.ForEach((e) => {
                if (e.name == name)
                {
                    texture = e.Texture;
                }
            });

            return texture;
        }*/
    }
    [Serializable]
    public struct ImageContainerInfo
    {
        public string name;
        public Texture2D Texture;
    }
}