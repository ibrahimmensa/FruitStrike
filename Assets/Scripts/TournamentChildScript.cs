using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TournamentChildScript : MonoBehaviour
{
    private int index;
    public Text indexText, name, info, fee, feeTxt, prize, time;
    public RawImage icon;
    public Button button;
    private Tournament thisTournament;
    private float lastUpdate;

    void Start()
    {
        index = FindObjectOfType<FruitSpearGameCode>().listItems.Count;
        thisTournament = NetworkScript.tournaments[index];

        name.text = thisTournament.name;
        info.text = thisTournament.info;
        feeTxt.text = "Entry Fee:";
        fee.text = ""+thisTournament.fee;
        prize.text = ""+thisTournament.prize;
        time.text = thisTournament.getTimeLeft();
        FindObjectOfType<NetworkScript>().retreivePlayers(thisTournament);

        string type = thisTournament.type;
        button.onClick.AddListener(() => joinGame());

        switch(type){
            case "bid":
               thisTournament.icon = (Texture2D)Resources.Load("t_luck");
               feeTxt.text = "Minimum:";
            break;
            case "top":
               thisTournament.icon = (Texture2D)Resources.Load("t_top");
               info.text = "Loading...";
            break;
            case "target":
                thisTournament.icon = (Texture2D)Resources.Load("t_target");
            break;
            case "time":
                thisTournament.icon = (Texture2D)Resources.Load("t_watch");
            break;
        }
               icon.texture = thisTournament.icon;
               
        int num = index + 1;
        indexText.text = ""+num;
                 
        FindObjectOfType<FruitSpearGameCode>().listItems.Add(this.gameObject);
    }

    private void joinGame(){

        FruitUI.selectedTournament = thisTournament;
		//Debug.Log(thisTournament.name + " Tournament Selected");
        FindObjectOfType<FruitUI>().openJoinPanel();
    }

    void Update(){

        if(Time.time - lastUpdate >= 1){
            lastUpdate = Time.time;
            if(thisTournament.type.Equals("top") && thisTournament.curPlayers!=null){
                info.text = "("+thisTournament.curPlayers.Count+"/"+thisTournament.max + " Players)";
                //Debug.Log("Setting Info "+info.text);
                if(thisTournament.curPlayers.Count >= thisTournament.max){
                    thisTournament.hasEnded = true;
                    thisTournament.used = true;
                }
            }
        }
    }
}
