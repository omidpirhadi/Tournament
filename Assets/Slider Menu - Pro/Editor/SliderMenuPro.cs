using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(SliderMenu))]
[System.Serializable]
public class SliderMenuPro : Editor{

	//Animation Variables------------------------------------------------------------------
	//	public enum AnimationType { Slide, itemtype1, itemtype2 }
	//	private AnimationType animation = AnimationType.Slide;
	//------------------------------------------------------------------
	public Texture2D Header;

	public GUIContent HorizontalSliderIcon;
	public GUIContent ShowButtonsIcon;
	public GUIContent ButtonSpriteIcon;
	public GUIContent BackgroundIcon;
	public GUIContent ScrollContentsIcon;
	public GUIContent ElementSizeIcon;
	public GUIContent ElementScaleIcon;
	public GUIContent ElementMarginIcon;
	public GUIContent TransitionInIcon;
	public GUIContent TransitionOutIcon;
	public GUIContent PreviousColorIcon;
	public GUIContent ActiveColorIcon;
	public GUIContent NextColorIcon;
	public GUIContent SlideAnimationIcon;


	void OnEnable()
	{
		SliderMenu MySliderMenu = (SliderMenu)target;

		Header =(Texture2D) AssetDatabase.LoadAssetAtPath ("Assets/Slider Menu - Pro/Editor/icons/Header.png", typeof(Texture2D));

		HorizontalSliderIcon = new GUIContent (" HScrollBar",(Texture2D) AssetDatabase.LoadAssetAtPath("Assets/Slider Menu - Pro/Editor/icons/Horizontal Slider.png",typeof(Texture2D)),"Horizontal Slider");
		ShowButtonsIcon = new GUIContent (" Show Buttons",(Texture2D) AssetDatabase.LoadAssetAtPath("Assets/SSlider Menu - Pro/Editor/icons/Show Buttons.png",typeof(Texture2D)),"Show Buttons");
		ButtonSpriteIcon = new GUIContent (" Button Sprite",(Texture2D) AssetDatabase.LoadAssetAtPath("Assets/Slider Menu - Pro/Editor/icons/Button Sprite.png",typeof(Texture2D)),"Button Sprite");
		BackgroundIcon = new GUIContent (" Background",(Texture2D) AssetDatabase.LoadAssetAtPath("Assets/Slider Menu - Pro/Editor/icons/background.png",typeof(Texture2D)),"Background");
		ScrollContentsIcon = new GUIContent (" Scroll Contents",(Texture2D) AssetDatabase.LoadAssetAtPath("Assets/Slider Menu - Pro/Editor/icons/Scroll Content.png",typeof(Texture2D)),"Scroll Content");
		ElementSizeIcon = new GUIContent (" Slides Size",(Texture2D) AssetDatabase.LoadAssetAtPath("Assets/Slider Menu - Pro/Editor/icons/Element Size.png",typeof(Texture2D)),"Slides Size");
		ElementScaleIcon = new GUIContent (" Slides Scale",(Texture2D) AssetDatabase.LoadAssetAtPath("Assets/Slider Menu - Pro/Editor/icons/Element Scale.png",typeof(Texture2D)),"Slides Scale");
		ElementMarginIcon = new GUIContent (" Slides Margin",(Texture2D) AssetDatabase.LoadAssetAtPath("Assets/Slider Menu - Pro/Editor/icons/Element Margin.png",typeof(Texture2D)),"Slides Margin");
		TransitionInIcon = new GUIContent (" Transition In",(Texture2D) AssetDatabase.LoadAssetAtPath("Assets/Slider Menu - Pro/Editor/icons/Transition In.png",typeof(Texture2D)),"Transition In");
		TransitionOutIcon = new GUIContent (" Transition Out",(Texture2D) AssetDatabase.LoadAssetAtPath("Assets/Slider Menu - Pro/Editor/icons/Transition Out.png",typeof(Texture2D)),"Transition Out");
		PreviousColorIcon = new GUIContent (" Previous Color",(Texture2D) AssetDatabase.LoadAssetAtPath("Assets/Slider Menu - Pro/Editor/icons/Previous Color.png",typeof(Texture2D)),"Previous Color");
		ActiveColorIcon = new GUIContent (" Active Color",(Texture2D) AssetDatabase.LoadAssetAtPath("Assets/Slider Menu - Pro/Editor/icons/Active Color.png",typeof(Texture2D)),"Active Color");
		NextColorIcon = new GUIContent (" Next Color",(Texture2D) AssetDatabase.LoadAssetAtPath("Assets/Slider Menu - Pro/Editor/icons/Next Color.png",typeof(Texture2D)),"Next Color");
		SlideAnimationIcon = new GUIContent (" Slide Animation",(Texture2D) AssetDatabase.LoadAssetAtPath("Assets/Slider Menu - Pro/Editor/icons/Slide Animation.png",typeof(Texture2D)),"Slide Animation");
	}

	void OnGUI()
	{

	}
	public override void OnInspectorGUI(){
		serializedObject.Update ();
		SliderMenu MySliderMenu = (SliderMenu)target;

		EditorGUI.DrawPreviewTexture(new Rect(0,138,EditorGUIUtility.currentViewWidth,160),Header);
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();


		//Canvas Settings--------------------------------------------------------------------------------------------
		GUI.backgroundColor = Color.green;
		EditorGUILayout.HelpBox ("Canvas Settings",MessageType.Info);
		if (MySliderMenu.YourCanvas == null) {
			GUI.backgroundColor = Color.red;
			GUI.color=Color.white;
		} else {
			GUI.backgroundColor = Color.white;
		}
		MySliderMenu.YourCanvas = EditorGUILayout.ObjectField ("Canvas",MySliderMenu.YourCanvas,typeof(Canvas),true) as Canvas;
		GUI.backgroundColor = Color.white;
		MySliderMenu.SlidesInView = EditorGUILayout.IntField ("Slides In View",MySliderMenu.SlidesInView);
		//-----------------------------------------------------------------------------------------------------------



		//ScrollBar Settings-----------------------------------------------------------------------------------------
		GUI.backgroundColor = Color.green;
		EditorGUILayout.HelpBox ("ScrollBar Settings",MessageType.Info);
		GUI.backgroundColor = Color.white;
		MySliderMenu.Enable_Show_ScrollBar = EditorGUILayout.Toggle ("Show ScrollBar",MySliderMenu.Enable_Show_ScrollBar);
		if (MySliderMenu.HorizontalScrollBar == null) {
			GUI.backgroundColor = Color.red;
			GUI.color=Color.white;
		} else {
			GUI.backgroundColor = Color.white;
		}
		EditorGUILayout.PropertyField(serializedObject.FindProperty("HorizontalScrollBar"),HorizontalSliderIcon, true);
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		GUI.backgroundColor = Color.white;
		EditorGUILayout.PropertyField(serializedObject.FindProperty("ShowButtons"),ShowButtonsIcon, true);
		if (MySliderMenu.ShowButtons==true) {
			if (MySliderMenu.ButtonSprite == null) {
				GUI.backgroundColor = Color.red;
			} else {
				GUI.backgroundColor = Color.white;
			}
			EditorGUILayout.PropertyField(serializedObject.FindProperty("ButtonSprite"),ButtonSpriteIcon, true);
		}
		//-----------------------------------------------------------------------------------------------------------



		//Content Settings-------------------------------------------------------------------------------------------
		GUI.backgroundColor = Color.green;
		EditorGUILayout.HelpBox ("Content Settings",MessageType.Info);
		GUI.backgroundColor = Color.white;
		if (MySliderMenu.Background == null) {
			GUI.backgroundColor = Color.red;
			GUI.color=Color.white;
		} else {
			GUI.backgroundColor = Color.white;
		}
		EditorGUILayout.PropertyField(serializedObject.FindProperty("Background"),BackgroundIcon, true);
		if (MySliderMenu.ScrollContent == null) {
			GUI.backgroundColor = Color.red;
			GUI.color=Color.white;
		} else {
			GUI.backgroundColor = Color.white;
		}
		EditorGUILayout.PropertyField(serializedObject.FindProperty("ScrollContent"),ScrollContentsIcon, true);
		GUI.backgroundColor = Color.white;
		EditorGUILayout.PropertyField(serializedObject.FindProperty("LevelThumbnails"), true);
		//-----------------------------------------------------------------------------------------------------------



		//Slides Info Settings---------------------------------------------------------------------------------------
		GUI.backgroundColor = Color.green;
		EditorGUILayout.HelpBox ("Slides Settings",MessageType.Info);
		if (MySliderMenu.SlidesNamePrefix == null) {
			GUI.backgroundColor = Color.red;
			GUI.color=Color.white;
		} else {
			GUI.backgroundColor = Color.white;
		}
		MySliderMenu.SlidesNamePrefix = EditorGUILayout.TextField ("Prefix Name",MySliderMenu.SlidesNamePrefix);
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		GUI.backgroundColor = Color.white;
		EditorGUILayout.PropertyField(serializedObject.FindProperty("Element_Size"),ElementSizeIcon, true);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("Element_Scale"),ElementScaleIcon, true);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("Element_Margin"),ElementMarginIcon, true);
		//-----------------------------------------------------------------------------------------------------------



		//Transition Settings----------------------------------------------------------------------------------------
		GUI.backgroundColor = Color.green;
		EditorGUILayout.HelpBox ("Transition Settings",MessageType.Info);
		GUI.backgroundColor = Color.white;
		EditorGUILayout.PropertyField(serializedObject.FindProperty("Transition_In"),TransitionInIcon, true);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("Transition_Out"),TransitionOutIcon, true);
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.PropertyField(serializedObject.FindProperty("PreviousSlideColor"),PreviousColorIcon, true);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("PreviousSlideColorTransition"), true);
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.PropertyField(serializedObject.FindProperty("ActiveSlideColor"),ActiveColorIcon, true);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("ActiveSlideColorTransition"), true);
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.PropertyField(serializedObject.FindProperty("NextSlideColor"),NextColorIcon, true);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("NextSlideColorTransition"), true);
		//-----------------------------------------------------------------------------------------------------------


		//Animation Settings----------------------------------------------------------------------------------------
		GUI.backgroundColor = Color.green;
		EditorGUILayout.HelpBox ("Animation Settings",MessageType.Info);
		GUI.backgroundColor = Color.white;
		EditorGUILayout.PropertyField(serializedObject.FindProperty("AddAnimation"),SlideAnimationIcon, true);
		if (MySliderMenu.AddAnimation == true) {
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			MySliderMenu.Animation_Move = EditorGUILayout.Toggle ("[1] Move Animation", MySliderMenu.Animation_Move);
			if(MySliderMenu.Animation_Move==true){
				MySliderMenu.PreviousPosition_Y = EditorGUILayout.FloatField ("Previous Move Position",MySliderMenu.PreviousPosition_Y);
				EditorGUILayout.PropertyField(serializedObject.FindProperty("PreviousPositionTransition"), true);
				EditorGUILayout.Space();
				MySliderMenu.ActivePostion_Y = EditorGUILayout.FloatField ("Active Move Position",MySliderMenu.ActivePostion_Y);
				EditorGUILayout.PropertyField(serializedObject.FindProperty("ActivePositionTransition"), true);
				EditorGUILayout.Space();
				MySliderMenu.NextPosition_Y = EditorGUILayout.FloatField ("Next Move Position",MySliderMenu.NextPosition_Y);
				EditorGUILayout.PropertyField(serializedObject.FindProperty("NextPositionTransition"), true);
			}
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			MySliderMenu.Animation_Rotate = EditorGUILayout.Toggle ("[2] Rotate Animation", MySliderMenu.Animation_Rotate);
			if(MySliderMenu.Animation_Rotate==true){
				EditorGUILayout.PropertyField(serializedObject.FindProperty("PreviousRotation"), true);
				EditorGUILayout.PropertyField(serializedObject.FindProperty("PreviousRotationTransition"), true);
				EditorGUILayout.Space();
				EditorGUILayout.PropertyField(serializedObject.FindProperty("ActiveRotation"), true);
				EditorGUILayout.PropertyField(serializedObject.FindProperty("ActiveRotationTransition"), true);
				EditorGUILayout.Space();
				EditorGUILayout.PropertyField(serializedObject.FindProperty("NextRotation"), true);
				EditorGUILayout.PropertyField(serializedObject.FindProperty("NextRotationTransition"), true);
			}
		}
		//-----------------------------------------------------------------------------------------------------------



		//Desktop Platform Enable Or Disable-------------------------------------------------------------------------
		GUI.backgroundColor = Color.green;
		EditorGUILayout.HelpBox ("Platform Settings",MessageType.Info);
		GUI.backgroundColor = Color.white;
		MySliderMenu.DesktopPlatform = EditorGUILayout.Toggle ("Desktop Platform",MySliderMenu.DesktopPlatform);
		//-----------------------------------------------------------------------------------------------------------


		serializedObject.ApplyModifiedProperties();

	}


}
