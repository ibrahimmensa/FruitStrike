using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tournament
{
   public string name;
   public string id;
   public string type;
   public string info;
   public string goal;
   public string winner1ID;
   public string winner2ID;
   public string winner3ID;
   public int fee;
   public float prize;
   public int max;
   public int day;
   public int hour;
   public int time;
   public int amount;
   public int shots;
   public int spears;
   public int plays;
   public float speed;
   public bool used;
   public bool hasEnded;
   public bool ready;
   public List<string> curPlayers = new List<string>();
   public static FruitUI ui;

   public Texture2D icon;
 
   public Tournament(string name, string id, string type, string info, string goal, int fee, float prize, int day, int hour){
       this.name = name;
       this.id = id;
	   this.type = type;
       this.info = info;
       this.goal = goal;
       this.fee = fee;
       this.prize = prize;
       this.day = day;
       this.hour = hour;
        
       getTimeLeft(); 
   }

    public string getTimeLeft()
    {
        string suffix = " Hrs Left";

        if(type.Equals("top")){
            return "";
        }
        else{
            int hrsLeft;

            if (DateTime.Now.Day.Equals(day)){
                hrsLeft = (hour - DateTime.Now.Hour);
                Debug.Log("Hour:" + DateTime.Now.Hour);    
            }
            else if (day > DateTime.Now.Day)
            { 
                if(hour >= DateTime.Now.Hour || day - DateTime.Now.Day > 1)
                    {
                        hrsLeft = day - DateTime.Now.Day;
                        suffix = " Days Left";
                    }
                    else
                        hrsLeft = 24 - (DateTime.Now.Hour - hour);
            }
            else{
                hrsLeft = -1;
            }

            if(hrsLeft < 0){
                hasEnded = true;
                NetworkScript.winnerReady = true;
                FruitUI.selectedTournament = this;
                return "Ended";
            }
            else
                return hrsLeft + suffix;
        }
    }

    public void showWinnerFirst(){
        if(PlayerPrefs.GetInt(id) != 1){
            Debug.Log("Showing Winner");
            showWinner();
            PlayerPrefs.SetInt(id,1);
        }else
            Debug.Log("Winner Already Shown");
    }

    public void showWinner(){
		int count = 0;
		
		if(winner1ID!=null && !winner1ID.Trim().Equals(""))
			count = 1;
		if(winner2ID!=null && !winner2ID.Trim().Equals(""))
			count = 2;
		if(winner3ID!=null && !winner3ID.Trim().Equals(""))
			count = 3;
		
		string[] ids = new string[count];
		float[] prizes = new float[count];
		
		if(count >0){
			ids[0] = winner1ID;
			prizes[0] = prize;
			
			if(count >1){
				ids[1] = winner2ID;
				prizes[0] = prize/2;
				prizes[1] = prize/2;
			}
			if(count >2){
				ids[2] = winner3ID;
				prizes[0] = prize/2;
				prizes[1] = prize/4;
				prizes[2] = prize/4;
			}
			
			
            ui.showRaffleWinner(name, ids, prizes);
        
		}else    
            ui.showMessage("Winner is yet to be chosen for "+name);
    }

    public string getTime()
    {
		string suffix = " Hrs";
		
        if(type.Equals("top")){
            return "";
        }
        else{
            int hrsLeft;

            if (DateTime.Now.Day.Equals(day)){
                hrsLeft = (hour - DateTime.Now.Hour);
                Debug.Log("Hour:" + DateTime.Now.Hour);    
            }
            else if (day > DateTime.Now.Day)
            { 
                if(hour >= DateTime.Now.Hour || day - DateTime.Now.Day > 1)
                    {
                        hrsLeft = day - DateTime.Now.Day;
                        suffix = " Days";
                    }
                    else
                        hrsLeft = 24 - (DateTime.Now.Hour - hour);
            }
            else{
                hrsLeft = -1;
            }

            if(hrsLeft < 0){
                hasEnded = true;
                NetworkScript.winnerReady = true;
                FruitUI.selectedTournament = this;
                return "Ended";
            }
            else
                return hrsLeft + suffix;
        }
    }
}
