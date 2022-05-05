using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StickerShareViwer : MonoBehaviour
{
    public List<Sticker> Stickers;

    public Image Renderer;
    public bool RepeatAnimation;
    public int StickerSelected = 0;
    private void OnEnable()
    {
        StartCoroutine(PlayAnimation());
    }



    private void OnDisable()
    {

    }
    private IEnumerator PlayAnimation()
    {
       
       int index = StickerSelected;
        Debug.Log("StickerPlayed");
        if (RepeatAnimation)
        {
            var repeated = 0;
            do
            {
                for (int i = 0; i < Stickers[index].Sequences.Count; i++)
                {
                    Renderer.sprite = Stickers[index].Sequences[i];
                    yield return new WaitForSecondsRealtime(Stickers[index].DurationSequence);
                }
                repeated++;
            } while (repeated <= Stickers[index].RepeateAnimation);
            Renderer.sprite = Stickers[index].Sequences[0];
        }
        else
        {
            for (int i = 0; i < Stickers[index].Sequences.Count; i++)
            {
                Renderer.sprite = Stickers[index].Sequences[i];
                yield return new WaitForSecondsRealtime(Stickers[index].DurationSequence);
            }
            Renderer.sprite = Stickers[index].Sequences[0];
        }

        gameObject.SetActive(false);
    }
    public void SelectSticker(string  name)
    {
       
        for (int i = 0; i < Stickers.Count;i++)
        {
            if(Stickers[i].stickerName == name)
            {
                StickerSelected = i;
            }
        }
        
    }
}
