using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class StickerController : MonoBehaviour
{
    public Sticker sticker;
    public Image Renderer;
    public bool RepeatAnimation;


    public IEnumerator PlaySticker()
    {
       
        if (RepeatAnimation)
        {
            var repeated = 0;
            do
            {
                for (int i = 0; i < sticker.Sequences.Count; i++)
                {
                    Renderer.sprite = sticker.Sequences[i];
                    yield return new WaitForSecondsRealtime(sticker.DurationSequence);
                }
                repeated++;
            } while (repeated <= sticker.RepeateAnimation);
            Renderer.sprite = sticker.Sequences[0];
        }
        else
        {
            for (int i = 0; i < sticker.Sequences.Count; i++)
            {
                Renderer.sprite = sticker.Sequences[i];
                yield return new WaitForSecondsRealtime(sticker.DurationSequence);
            }
            Renderer.sprite = sticker.Sequences[0];
        }
    }
}
