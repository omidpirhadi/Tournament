using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenKeyboard : MonoBehaviour
{
    TouchScreenKeyboard keyboard;

    
    public void Open()
    {
         keyboard = TouchScreenKeyboard.Open("hI", TouchScreenKeyboardType.ASCIICapable, true, true, false, false, "sss", 255);
       
        
    }
}
