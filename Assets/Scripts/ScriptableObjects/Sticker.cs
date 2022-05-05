using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "Sticker", menuName = "DiacoTools/CreateSticker", order = 1)]
public class Sticker : ScriptableObject
{
    public string stickerName;
    public Texture2D Titel;
    public int RepeateAnimation;
    public float DurationSequence;
    public List<Sprite> Sequences;
}
