using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Diaco.Social.TeamsInputField
{
    public class InputFieldSocial : MonoBehaviour
    {

        public bool Digit;
        public bool Context;
        public string PrimeryData;
        public List<string> ElementContexts;
        public int MinNum;
        public int MaxNum;

        public int CurrentValueDigit = 0;
        public int CurrentElementContext = 0;

        public InputField FieldOfViwe;
        public Button btn_Next;
        public Button btn_Previous;
        private void OnEnable()
        {
            FieldOfViwe.text = PrimeryData;
            btn_Next.onClick.AddListener(() => {  Next(); });
            btn_Previous.onClick.AddListener(() => {  Previous(); });
        }
        public void Next()
        {
            if(Digit)
            {
                var temp = CurrentValueDigit + 1;
               CurrentValueDigit =  Mathf.Clamp(temp, MinNum, MaxNum);
                FieldOfViwe.text = CurrentValueDigit.ToString();
                //Debug.Log("DigitPlus");
            }
            else
            {
                var temp = CurrentElementContext + 1;
                CurrentElementContext = Mathf.Clamp(temp, 0, ElementContexts.Count-1);
                var e = ElementContexts[CurrentElementContext];
                FieldOfViwe.text = e;
            }
        }
        public void Previous()
        {
            if (Digit)
            {
                var temp = CurrentValueDigit - 1;
                CurrentValueDigit = Mathf.Clamp(temp, MinNum, MaxNum);
                FieldOfViwe.text = CurrentValueDigit.ToString();
               
            }
            else
            {
                var temp = CurrentElementContext - 1;
                CurrentElementContext = Mathf.Clamp(temp, 0, ElementContexts.Count-1);
                var e = ElementContexts[CurrentElementContext];
                FieldOfViwe.text = e;
            }
        }
        public  void FillElementContexts(List<string> contexts)
        {
            for(int i = 0; i<contexts.Count;i++)
            {
               // ElementContexts[i] = contexts[i]
            }
        }
    }

}