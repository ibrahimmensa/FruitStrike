using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpinScripct : MonoBehaviour
{   
    public Text moneyText1, moneyText2, moneyText3, moneyText4;
    public GameObject videoIcon;
    public static int spins, maxSpins;
    private float angle, angleMax = 1000f, lastUpdate;
    private int counter, time;
    private float cash1=50f, cash2=50f, cash3=100f, cash4=200f;
    private bool pushed, ended;

    private bool turn, slow; 

    public GameObject wheel;

        void Start()
    {
        refreshCurrency();
        resetIcon();
    }

    public void refreshCurrency(){
        moneyText1.text = ""+cash1;
        moneyText2.text = ""+cash2;
        moneyText3.text = ""+cash3;
        moneyText4.text = ""+cash4;

        resetIcon();
    }

    public void spinWheel()
    {
        if(!turn){

            if(maxSpins > 0){

                FindObjectOfType<FruitUI>().playButtonSound();

                if(spins > 0){
                    spins--;
                    proceedSpin();
                }else{
                    
                    FruitSpearGameCode.rewardMode = FruitSpearGameCode.RewardMode.Spin;
                    FindObjectOfType<FruitSpearGameCode>().showRewardAd();
                }

            }else
                FindObjectOfType<FruitUI>().showMessage("You have reached your Spin limit for today, Come back tomorrow :)");
        }else
            Debug.Log("Still turning fam...");
    }

    private void resetIcon(){
        if(spins > 0)
            videoIcon.SetActive(false);
        else  
            videoIcon.SetActive(true);  
    }

    public void proceedSpin(){

            maxSpins--;
            PlayerPrefs.SetInt("max_spins", maxSpins);
            Debug.Log(maxSpins + " spins left today");

            turn = true;
                    slow = false;
                    ended = false;
                    pushed = false;
                    counter = 0;
                    angle = 350f;
                    time = UnityEngine.Random.Range(80,120);
                    Debug.Log("Spin for "+time);
                    resetIcon();
                    FindObjectOfType<FruitSpearGameCode>().playSound(FruitSpearGameCode.Sound.Spin);
        
    }

        void FixedUpdate()
    {
        
        if (Time.time - lastUpdate >= 0.018f)
            {
                counter++;
                if(turn){
                    wheel.transform.Rotate(new Vector3(0, 0, angle*Time.deltaTime));
                     float z = wheel.transform.localEulerAngles.z;
                    
                    if(angle < 260 && !ended){
                        FindObjectOfType<FruitSpearGameCode>().playSound(FruitSpearGameCode.Sound.SpinEnd);
                        ended = true;
                    }
                    if(angle < 100 && !pushed){

                        int x = 3;
                        int x2 = 2;

                        if(NetworkScript.myCash >= 1000f){
                            x = 4;
                            x2 = 2;
                        }
						
						if(NetworkScript.myCash >= 1500f){
                            x = 5;
                            x2 = 3;
                        }

						if(NetworkScript.myCash >= 2000f){
                            x = 0;
                            x2 = 0;
                        }
						
						
                        if(z>=313 && z<=336){
                            int rand = UnityEngine.Random.Range(0,x);
                            if(rand!=1){
                                angle += 35f;
                            }
                            Debug.Log("Cashed 1 Rand ="+rand);
                        }

                        if(z>= 133 && z<=156){
                            int rand = UnityEngine.Random.Range(0,x);
                            if(rand!=1){
                                angle += 35f;
                            }
                            Debug.Log("Cashed 2 Rand ="+rand);
                        }

                        if(z>= 245 && z<=268){
                            int rand = UnityEngine.Random.Range(0,x2);
                            if(rand!=1){
                                angle += 35f;

                            }
                            //Debug.Log("Cashed 3 Rand ="+rand);
                        }
						
						if(z>= 65 && z<=88){
                   
                           // Debug.Log("Cashed 4 Rand (Free)");
						   if(NetworkScript.myCash >= 3000f){
								int rand = UnityEngine.Random.Range(0,3);
								if(rand < 1){
									angle += 35f;
								}
								//Debug.Log(rand+"/1");
							}
                        }

                        if(z>= 112 && z<=135){
                             int rand = UnityEngine.Random.Range(0,3);
                            if(rand!=1)
                                angle += 35f;
                        }
                        
                        pushed = true;
                        
                    }
                    if(angle <= 0){
                        endSpin();
                    }
                    if(counter >= time)
                        slow = true;
                    if(slow)
                        angle -= 2f;    
                    else if(angle < angleMax)
                        angle += 2f;   
                }

                    lastUpdate = Time.time;
            }
    }

    private void endSpin(){
        turn = false;
        //FindObjectOfType<FruitSpearGameCode>().audio2.Pause();
        
        float z = wheel.transform.localEulerAngles.z;
        Debug.Log("End Spin @ Z="+z);
        
		if(z < 0){
            Debug.Log("Nothing");
        }
        else if(z <= 22.5f){
            Debug.Log("Awarded: "+cash4);
            FindObjectOfType<NetworkScript>().giveCash(cash4," from Fortune Wheel!");
        }
        else if(z <= 45f){
            Debug.Log("Awarded: Free Spin");
            FindObjectOfType<FruitUI>().showMessage("You have a Free Spin");
            spins++;
        }
        else if(z <= 67.5f){
            Debug.Log("Awarded: 20 Apples");
            FindObjectOfType<FruitUI>().openAwardPanel(20);
        }
        else if(z <= 90f){
            Debug.Log("Awarded: 10 Apples");
            FindObjectOfType<FruitUI>().openAwardPanel(10);
        }
        else if(z <= 112.5f){
            Debug.Log("Awarded: 50 Apples");
            FindObjectOfType<FruitUI>().openAwardPanel(50);
        }
        else if(z <= 135f){
            Debug.Log("Awarded: "+cash1);
            FindObjectOfType<NetworkScript>().giveCash(cash1," from Fortune Wheel!,");
        }
        else if(z <= 157.5f){
            Debug.Log("Awarded: 5 Green Apples");
            FindObjectOfType<FruitUI>().openGreenAwardPanel(5);
        }
        else if(z <= 180f){
            Debug.Log("Awarded: 100 Apples");
            FindObjectOfType<FruitUI>().openAwardPanel(100);
        }
        else if(z <= 202.5f){
            Debug.Log("Awarded: "+cash3);
            FindObjectOfType<NetworkScript>().giveCash(cash3," from Fortune Wheel!");
        }
        else if(z <= 225f){
            Debug.Log("Try Again");
            FindObjectOfType<FruitUI>().showMessage("Oops...Try Again");
        }
        else if(z <= 247.5f){
            Debug.Log("Awarded: 20 Apples");
            FindObjectOfType<FruitUI>().openAwardPanel(20);
        }
        else if(z <= 270f){
            Debug.Log("Awarded: 10 Apples");
            FindObjectOfType<FruitUI>().openAwardPanel(10);
        }
        else if(z <= 292.5f){
            Debug.Log("Awarded: 50 Apples");
            FindObjectOfType<FruitUI>().openAwardPanel(50);
        }
        else if(z <= 315f){
            Debug.Log("Awarded: "+cash2);
            FindObjectOfType<NetworkScript>().giveCash(cash2," from Fortune Wheel!");
        }
        else if(z <= 337.5f){
            Debug.Log("Awarded: 5 Green Apples");
            FindObjectOfType<FruitUI>().openGreenAwardPanel(5);
        }
         else if(z <= 360f){
            Debug.Log("Awarded: 100 Apples");
            FindObjectOfType<FruitUI>().openAwardPanel(100);
        }
       
         resetIcon();             
    }

     public static float Clamp0360(float eulerAngles)
      {
          float result = eulerAngles - Mathf.CeilToInt(eulerAngles / 360f) * 360f;
          if (result < 0)
          {
              result += 360f;
          }
          return result;
      }
}
