using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SliderMenu : MonoBehaviour {
	//Canvas Settings--------------------------------------------------------------------------------------------
	public Canvas YourCanvas;
	public int SlidesInView;
	//-----------------------------------------------------------------------------------------------------------



	//ScrollBar Settings-----------------------------------------------------------------------------------------
	public bool Enable_Show_ScrollBar = false;
	public Scrollbar HorizontalScrollBar;
	public bool ShowButtons=false;
	public Sprite ButtonSprite;
	private float k=0;
	private bool ButtonClicked=false;
	//-----------------------------------------------------------------------------------------------------------


	//Content Settings-------------------------------------------------------------------------------------------
	public Sprite Background;
	public RectTransform ScrollContent;
	public List<GameObject> LevelThumbnails;
	//-----------------------------------------------------------------------------------------------------------


	//Slides Settings--------------------------------------------------------------------------------------------
	public string SlidesNamePrefix="Button 0";
	public Vector2 Element_Size;
	public float Element_Margin;
	public Vector2 Element_Scale;
	//-----------------------------------------------------------------------------------------------------------



	//Transition Settings----------------------------------------------------------------------------------------
	public float Transition_In;
	public float Transition_Out;
	public Color PreviousSlideColor=new Color(1,1,1,255);
	public Color ActiveSlideColor=new Color(1,1,1,255);
	public Color NextSlideColor=new Color(1,1,1,255);
	public float PreviousSlideColorTransition;
	public float ActiveSlideColorTransition;
	public float NextSlideColorTransition;
	//-----------------------------------------------------------------------------------------------------------



	//Animation Settings-----------------------------------------------------------------------------------------
	public bool AddAnimation=false;

	public bool Animation_Move = false;
	public float PreviousPosition_Y;
	public float ActivePostion_Y;
	public float NextPosition_Y;
	public float PreviousPositionTransition;
	public float ActivePositionTransition;
	public float NextPositionTransition;

	public bool Animation_Rotate = false;
	public Vector3 PreviousRotation;
	public Vector3 ActiveRotation;
	public Vector3 NextRotation;
	public float PreviousRotationTransition;
	public float ActiveRotationTransition;
	public float NextRotationTransition;

	public bool Animation_Scale=false;
	public Vector3 PreviousScale;
	public Vector3 ActiveScale;
	public Vector3 NextScale;
	//-----------------------------------------------------------------------------------------------------------



	//Slides Settings--------------------------------------------------------------------------------------------
	public bool DesktopPlatform;
	//-----------------------------------------------------------------------------------------------------------




	//Other Variables--------------------------------------------------------------------------------------------
	private float n;
	private float ScrollSteps;
	//-----------------------------------------------------------------------------------------------------------










	/*void Start () {

		//Canvas Settings----------------------------------------------------------------------------
		YourCanvas.GetComponent<CanvasScaler> ().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
		YourCanvas.GetComponent<CanvasScaler> ().referenceResolution = new Vector2 ((Element_Size.x+2*Mathf.Clamp(Element_Margin,0,1000))*SlidesInView,YourCanvas.GetComponent<CanvasScaler> ().referenceResolution.y);
		//-------------------------------------------------------------------------------------------



		//Set Background Of Content------------------------------------------------------------------
		GameObject.Find ("BackgroundOfContent").GetComponent<Image>().sprite=Background;
		//-------------------------------------------------------------------------------------------



		//Next And Previous Button-------------------------------------------------------------------
		if (ShowButtons == true) {
			GameObject.Find ("Buttons").SetActive (true);
			GameObject.Find ("Buttons/Next").GetComponent<Image> ().sprite = ButtonSprite;
			GameObject.Find ("Buttons/Previous").GetComponent<Image> ().sprite = ButtonSprite;
		} else {
			GameObject.Find ("Buttons").SetActive (false);
		}
		//-------------------------------------------------------------------------------------------



		//Auto Find Slides And Auto Set Size And Position Of Slides
		for (int b=0; b<ScrollContent.childCount; b++) {
			LevelThumbnails.Add(GameObject.Find(ScrollContent.name+"/"+SlidesNamePrefix+b));
			LevelThumbnails[b].GetComponent<RectTransform>().sizeDelta=new Vector2(Element_Size.x,Element_Size.y);
			LevelThumbnails[b].GetComponent<RectTransform>().localPosition=new Vector3((2*b+3)*Element_Size.x/2+(2*b+3)*Mathf.Clamp(Element_Margin,0,1000),200,10);
		}
		//-------------------------------------------------------------------------------------------



		//Set Size Of ScrollContent (Auto Set)
		ScrollContent.GetComponent<RectTransform>().sizeDelta=new Vector2((LevelThumbnails.Count+2)*(Element_Size.x+2*Mathf.Clamp(Element_Margin,0,1000)),Element_Size.y);
		//-------------------------------------------------------------------------------------------



		//Calculate ScrollSteps Value----------------------------------------------------------------
		n = LevelThumbnails.Count - 1;
		ScrollSteps = 1 / n;
		//-------------------------------------------------------------------------------------------
	}

    */






    public void InitializeStart()
    {
        //Canvas Settings----------------------------------------------------------------------------
        YourCanvas.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        YourCanvas.GetComponent<CanvasScaler>().referenceResolution = new Vector2((Element_Size.x + 2 * Mathf.Clamp(Element_Margin, 0, 1000)) * SlidesInView, YourCanvas.GetComponent<CanvasScaler>().referenceResolution.y);
        //-------------------------------------------------------------------------------------------



        //Set Background Of Content------------------------------------------------------------------
        GameObject.Find("BackgroundOfContent").GetComponent<Image>().sprite = Background;
        //-------------------------------------------------------------------------------------------



        //Next And Previous Button-------------------------------------------------------------------
        if (ShowButtons == true)
        {
            GameObject.Find("Buttons").SetActive(true);
            GameObject.Find("Buttons/Next").GetComponent<Image>().sprite = ButtonSprite;
            GameObject.Find("Buttons/Previous").GetComponent<Image>().sprite = ButtonSprite;
        }
        else
        {

            GameObject.Find("Buttons").SetActive(false);
        }
        //-------------------------------------------------------------------------------------------



        //Auto Find Slides And Auto Set Size And Position Of Slides
        for (int b = 0; b < ScrollContent.childCount; b++)
        {
            LevelThumbnails.Add(GameObject.Find(ScrollContent.name + "/" + SlidesNamePrefix + b));
            LevelThumbnails[b].GetComponent<RectTransform>().sizeDelta = new Vector2(Element_Size.x, Element_Size.y);
            LevelThumbnails[b].GetComponent<RectTransform>().localPosition = new Vector3((2 * b + 3) * Element_Size.x / 2 + (2 * b + 3) * Mathf.Clamp(Element_Margin, 0, 1000), 200, 10);
        }
        //-------------------------------------------------------------------------------------------



        //Set Size Of ScrollContent (Auto Set)
        ScrollContent.GetComponent<RectTransform>().sizeDelta = new Vector2((LevelThumbnails.Count + 2) * (Element_Size.x + 2 * Mathf.Clamp(Element_Margin, 0, 1000)), Element_Size.y);
        //-------------------------------------------------------------------------------------------



        //Calculate ScrollSteps Value----------------------------------------------------------------
        n = LevelThumbnails.Count - 1;
        ScrollSteps = 1 / n;
    }

	void Update () {
		//Show Or Hide ScrollBar---------------------------------------------------------------------
		if (Enable_Show_ScrollBar == true) {
			HorizontalScrollBar.gameObject.SetActive(true);
		}else{
			HorizontalScrollBar.gameObject.SetActive(false);
		}
		//-------------------------------------------------------------------------------------------



		//Slides Magnet------------------------------------------------------------------------------
		if (DesktopPlatform == false) {
			if (Input.touchCount == 0) {
				for (int s=0; s<LevelThumbnails.Count; s++) {
					if (HorizontalScrollBar.GetComponent<Scrollbar> ().value > (ScrollSteps / 2) + (s - 1) * (ScrollSteps) && HorizontalScrollBar.GetComponent<Scrollbar> ().value <= Mathf.Clamp (ScrollSteps / 2 + s * ScrollSteps, 0, 1)) {
						HorizontalScrollBar.GetComponent<Scrollbar> ().value = Mathf.Lerp (HorizontalScrollBar.GetComponent<Scrollbar> ().value, s * ScrollSteps, 0.1f);
					}
				}
			}
		} 
								//When Use Next And Previous Buttons
		for (int s=0; s<LevelThumbnails.Count; s++) {
			if (k > (ScrollSteps / 2) + (s - 1) * (ScrollSteps) && k <= Mathf.Clamp (ScrollSteps / 2 + s * ScrollSteps, 0, 1)) {
				k = Mathf.Lerp (k, s * ScrollSteps, 0.1f);
			}
		}
		//-------------------------------------------------------------------------------------------



		//Slides Scale, Slides Transition And Slides Color Transition-------------------------------
		for (int s=0; s<LevelThumbnails.Count; s++) {
			for (int t=0; t<LevelThumbnails.Count; t++) {
				if (HorizontalScrollBar.GetComponent<Scrollbar> ().value > (ScrollSteps / 2) + (s - 1) * (ScrollSteps) && HorizontalScrollBar.GetComponent<Scrollbar> ().value <= Mathf.Clamp (ScrollSteps / 2 + s * ScrollSteps, 0, 1)) {
					if(t!=s){
						LevelThumbnails [t].transform.localScale = Vector2.Lerp (LevelThumbnails [t].transform.localScale, new Vector2 (1, 1), Transition_Out);
					}
					if(t==s){
						LevelThumbnails [s].GetComponent<Image>().color=Vector4.Lerp(LevelThumbnails [s].GetComponent<Image>().color,ActiveSlideColor,ActiveSlideColorTransition) ;
						LevelThumbnails [s].transform.localScale = Vector2.Lerp (LevelThumbnails [s].transform.localScale, new Vector2 (Element_Scale.x, Element_Scale.y), Transition_In);
						LevelThumbnails [s].gameObject.transform.SetAsLastSibling();


						//Slide Animation
						if(Animation_Move==true){
							Vector3 PositionVector=LevelThumbnails[s].GetComponent<RectTransform>().localPosition;
							LevelThumbnails[s].GetComponent<RectTransform>().localPosition=Vector3.Lerp(PositionVector,new Vector3(PositionVector.x,ActivePostion_Y,10),ActivePositionTransition);
						}
						//Rotate Animation
						if(Animation_Rotate==true){
							Vector3 RotationVector=LevelThumbnails[s].GetComponent<RectTransform>().localEulerAngles;
							LevelThumbnails[s].GetComponent<RectTransform>().localEulerAngles=Vector3.Lerp(RotationVector,ActiveRotation,ActiveRotationTransition);
						}
					}
					else if(t<s){
						LevelThumbnails [t].GetComponent<Image>().color=Vector4.Lerp(LevelThumbnails [t].GetComponent<Image>().color,PreviousSlideColor,PreviousSlideColorTransition) ;

						//Slide Animation
						if(Animation_Move==true){
							Vector3 PositionVector=LevelThumbnails[t].GetComponent<RectTransform>().localPosition;
							LevelThumbnails[t].GetComponent<RectTransform>().localPosition=Vector3.Lerp(PositionVector,new Vector3(PositionVector.x,PreviousPosition_Y,10),PreviousPositionTransition);
						}
						//Rotate Animation
						if(Animation_Rotate==true){
							Vector3 RotationVector=LevelThumbnails[t].GetComponent<RectTransform>().localEulerAngles;
							LevelThumbnails[t].GetComponent<RectTransform>().localEulerAngles=Vector3.Lerp(RotationVector,PreviousRotation,PreviousRotationTransition);
						}
					}
					else if(t>s){
						LevelThumbnails [t].GetComponent<Image>().color=Vector4.Lerp(LevelThumbnails [t].GetComponent<Image>().color,NextSlideColor,NextSlideColorTransition) ;

						//Slide Animation---------------------
						if(Animation_Move==true){
							Vector3 PositionVector=LevelThumbnails[t].GetComponent<RectTransform>().localPosition;
							LevelThumbnails[t].GetComponent<RectTransform>().localPosition=Vector3.Lerp(PositionVector,new Vector3(PositionVector.x,NextPosition_Y,10),NextPositionTransition);
						}
						//Rotate Animation
						if(Animation_Rotate==true){
							Vector3 RotationVector=LevelThumbnails[t].GetComponent<RectTransform>().localEulerAngles;
							LevelThumbnails[t].GetComponent<RectTransform>().localEulerAngles=Vector3.Lerp(RotationVector,NextRotation,PreviousRotationTransition);
						}
					}

				}
			}


		}
		//-------------------------------------------------------------------------------------------



		//Next Or Previous Button Is Clicked---------------------------------------------------------
		if (ButtonClicked == true) {
			HorizontalScrollBar.GetComponent<Scrollbar> ().value=Mathf.Lerp(HorizontalScrollBar.GetComponent<Scrollbar> ().value,k,0.1f);
		}
		//-------------------------------------------------------------------------------------------





	}





	public void NextButton(){
		k = Mathf.Clamp (k+ScrollSteps,0,1);
		ButtonClicked = true;
	}

	public void PreviousButton(){
		k = Mathf.Clamp (k-ScrollSteps,0,1);
		ButtonClicked = true;
	}

	public void ContentDrag(){
		ButtonClicked = false;
		k=Mathf.Clamp (HorizontalScrollBar.GetComponent<Scrollbar> ().value,0,1);

	}
}
