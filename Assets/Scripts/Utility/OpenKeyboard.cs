using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenKeyboard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open()
    {
        TouchScreenKeyboard keyboard = new TouchScreenKeyboard("", TouchScreenKeyboardType.ASCIICapable, true, true, false, false, "sss", 255);
        keyboard.active = true;
        
    }
}
