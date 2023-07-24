using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ListViewScript : MonoBehaviour
{
    private int index;
    public Text name, name2, score, score2, moneyText ,moneyText2, indexText;
    public RawImage avatar, trophy, scoreicon;
    public static int gold,silver,bronze,extra1,extra2,minimum;
    public GameObject moneyIco, cashIcon;

    public static bool shared;

    void Start()
    {
        index = FindObjectOfType<FruitSpearGameCode>().leaderboardItems.Count;

        if(NetworkScript.leaderboardEmpty){

            score.text = "";
			score2.text = "";
            indexText.text = "";
            name.text = NetworkScript.users[index].name;
            name2.text = "";
            moneyText.text = "";
            moneyText2.text = "";
            scoreicon.texture = (Texture2D)Resources.Load("blank"); 
            avatar.texture = (Texture2D)Resources.Load("blank"); 
            moneyIco.SetActive(false);
            FindObjectOfType<FruitSpearGameCode>().leaderboardItems.Add(this.gameObject);

        }else{

                score.text = NetworkScript.users[index].score + "";
            if(!NetworkScript.users[index].photoUrl.Trim().Equals(""))
                StartCoroutine(SetImage(NetworkScript.users[index].photoUrl, avatar));

            int num = index + 1;
            indexText.text = ""+num;

            if(FruitSpearGameCode.gameType == FruitSpearGameCode.GameType.Top){
                name.text = NetworkScript.users[index].name;
            }else{
                if(FruitUI.thisLeaderboard == FruitUI.Leaderboard.Todays_Apples || FruitUI.thisLeaderboard == FruitUI.Leaderboard.Yesterdays_Apples){
                    name2.text = NetworkScript.users[index].name;
                    name.text = "";
                    scoreicon.texture = (Texture2D)Resources.Load("green_apples");
                }
                else {
                    name.text = NetworkScript.users[index].name;
                    name2.text = "";
                    moneyText.text = "";
                    moneyText2.text = "";
					
					if(FruitUI.thisLeaderboard == FruitUI.Leaderboard.Top_Cashouts){
						name2.text = NetworkScript.users[index].name;
						name.text = "";
						score2.text = string.Format("{0:n0}", NetworkScript.users[index].score);
						score.text = "";
						scoreicon.texture = (Texture2D)Resources.Load("blank");  
						cashIcon.SetActive(true);
					}
					else{
						score2.text = "";
						scoreicon.texture = (Texture2D)Resources.Load("star");
						cashIcon.SetActive(false);
					}
                }

                moneyIco.SetActive(false);
            }
            
    /**
            if(num <=10)
                    if(FruitUI.userID.Equals(NetworkScript.users[index].id) && !shared){
                        FindObjectOfType<FruitUI>().openLeaderboardShareDialog();
                        FindObjectOfType<FruitUI>().shareText.text = 
                        "You are no "+num+" in the world!\n\nShare your acheivement";
                        shared = true;
                    }
    **/

            FindObjectOfType<FruitSpearGameCode>().leaderboardItems.Add(this.gameObject);

            if(FruitSpearGameCode.gameType == FruitSpearGameCode.GameType.Top || (FruitUI.thisLeaderboard != FruitUI.Leaderboard.Todays_Apples && FruitUI.thisLeaderboard != FruitUI.Leaderboard.Yesterdays_Apples)){
                if(num == 3)
                    trophy.texture = (Texture2D)Resources.Load("bronze");
                if(num == 2)
                    trophy.texture = (Texture2D)Resources.Load("silver");
                if(num == 1)
                    trophy.texture = (Texture2D)Resources.Load("gold");
            }
            else if(NetworkScript.users[index].score >= minimum){

                moneyIco.SetActive(true);
				
				if(num <= (silver + gold + bronze + extra1 + extra2)){
                    moneyText.text = ""+NetworkScript.extra2;
                    moneyText2.text = ""+NetworkScript.extra2;
                }
				
				if(num <= (silver + gold + bronze + extra1)){
                    moneyText.text = ""+NetworkScript.extra1;
                    moneyText2.text = ""+NetworkScript.extra1;
                }

                if(num <= (silver + gold +bronze)){
                    trophy.texture = (Texture2D)Resources.Load("bronze");
                    moneyText.text = ""+NetworkScript.bronze;
                    moneyText2.text = ""+NetworkScript.bronze;
                }
                if(num <= (silver + gold)){
                    trophy.texture = (Texture2D)Resources.Load("silver");
                    moneyText.text = ""+NetworkScript.silver;
                    moneyText2.text = ""+NetworkScript.silver;
                }
                if(num <= gold){
                    trophy.texture = (Texture2D)Resources.Load("gold");
                    moneyText.text = ""+NetworkScript.gold;
                    moneyText2.text = ""+NetworkScript.gold;
                }

                if(num > silver + gold + bronze + extra1 + extra2)
                    moneyIco.SetActive(false);
            }

        }
       
    }

     IEnumerator SetImage(string URLstring, RawImage image)
    {
		if(URLstring != null)
			if(!URLstring.Equals("")){
				System.Uri url = new System.Uri(URLstring);

				using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
				{
					yield return uwr.SendWebRequest();

					if (uwr.isNetworkError || uwr.isHttpError)
					{
						Debug.Log(uwr.error);
					}
					else
					{
						// Get downloaded asset bundle
						image.texture = DownloadHandlerTexture.GetContent(uwr);
					}
				}
		}
    }

}
