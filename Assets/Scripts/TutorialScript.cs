using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Unity;

public class TutorialScript : MonoBehaviour
{
    public GameObject p0, p1, p2, p3, p4, p5, p6, p7, p8, p9;
	public Text welcomeText, nextText, skipText, tapText, tutorial0Text, tutorial1Text, tutorial2Text, tutorial3Text, tutorial4Text, tutorial5Text,
	tutorial5bText, tutorial6Text, tutorial6bText, tutorial7Text, tutorial7bText, tutorial8Text, tutorial9Text, tutorial9bText;
    private float lastUpdate;
	public static bool loaded;

    void Start(){
        p0.SetActive(true);
        lastUpdate  = Time.time;
        Debug.Log("tutorial commenced");
		loaded = true;
		
		setStrings();
		
    }
	
	public void setStrings(){
		
		try{
			FindObjectOfType<FruitUI>().curImage2.texture = (Texture2D)Resources.Load(FruitUI.currency);
		}catch(Exception e){
			
		}
		
		welcomeText.text = Strings.welcome;
		nextText.text = Strings.next;
		skipText.text = Strings.skip;
		tapText.text = Strings.tap;
		tutorial0Text.text = Strings.tutorial0;
		tutorial1Text.text = Strings.tutorial1;
		tutorial2Text.text = Strings.tutorial2;
		tutorial3Text.text = Strings.tutorial3;
		tutorial4Text.text = Strings.tutorial4;
		tutorial5Text.text = Strings.tutorial5;
		tutorial5bText.text = Strings.tutorial5b;
		tutorial6Text.text = Strings.tutorial6;
		tutorial6bText.text = Strings.tutorial6b;
		tutorial7Text.text = Strings.tutorial7;
		tutorial7bText.text = Strings.tutorial7b;
		tutorial8Text.text = Strings.tutorial8;
		tutorial9Text.text = Strings.tutorial9;
		tutorial9bText.text = Strings.tutorial9b;
		
	}

    void switchPanel(){

        if(!FindObjectOfType<FruitUI>().currencyPanel.activeSelf){

            if(p0.activeSelf){
                p0.SetActive(false);
                p1.SetActive(true);
                Debug.Log("Switching to Panel 1");
            }
            else if(p1.activeSelf){
                p1.SetActive(false);
                p2.SetActive(true);
                Debug.Log("Switching to Panel 2");
            }
            else if(p2.activeSelf){
                p2.SetActive(false);
                p3.SetActive(true);
                Debug.Log("Switching to Panel 3");
            }
            else if(p3.activeSelf){
                p3.SetActive(false);
                p4.SetActive(true);
                Debug.Log("Switching to Panel 4");
            }
            else if(p4.activeSelf){
                p4.SetActive(false);
				Debug.Log("Green Leaderboard: " + FruitSpearGameCode.isGreen);
                if(FruitSpearGameCode.isGreen){
                    p5.SetActive(true);
                    Debug.Log("Switching to Panel 5");
                }
                else{
                    p6.SetActive(true); 
                    Debug.Log("Jumping to Panel 6");
                }   
                
            }
            else if(p5.activeSelf){
                p5.SetActive(false);
                p6.SetActive(true);
                Debug.Log("Switching to Panel 6");
            }
             else if(p6.activeSelf){
                p6.SetActive(false);
                p7.SetActive(true);
                Debug.Log("Switching to Panel 7");
            }
             else if(p7.activeSelf){
                p7.SetActive(false);
                p8.SetActive(true);
                Debug.Log("Switching to Panel 8");
            }
             else if(p8.activeSelf){
                p8.SetActive(false);
                p9.SetActive(true);
                Debug.Log("Switching to Panel 9");
            }
            else if(p9.activeSelf){
                p9.SetActive(false);
                FindObjectOfType<FruitUI>().tutorialPanel.SetActive(false);
                p0.SetActive(true);
                FruitSpearGameCode.isNew = false;
                Debug.Log("Closing Tutorials");
				FindObjectOfType<FruitSpearGameCode>().showBannerAd();
				//FindObjectOfType<FruitUI>().showMessageDialog(NetworkScript.message);
            }
        }
    }
    void Update()
    {

#if UNITY_EDITOR
            if (Time.time - lastUpdate >=3f && Input.GetMouseButtonDown(0))
            {
                switchPanel();
            }
#elif UNITY_ANDROID
            if (Time.time - lastUpdate >=3f && Input.touchCount > 0 )
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    switchPanel();
                }
            }
#endif
    }
}
