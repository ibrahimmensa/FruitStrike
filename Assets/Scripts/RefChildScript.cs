using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class RefChildScript : MonoBehaviour
{
    private int index;
    public Text name, indexText, cash;
    public RawImage avatar;
    public GameObject star;


    void Start()
    {
        index = FindObjectOfType<FruitSpearGameCode>().inviteItems.Count;
		Debug.Log("Index "+index);
        name.text = NetworkScript.invites[index].name;
		cash.text = NetworkScript.invites[index].score+"";
		if(NetworkScript.invites[index].score >= 1000){
			cash.color = Color.green;
			cash.text = "1000+";
		}
        if(!NetworkScript.invites[index].photoUrl.Trim().Equals(""))
			StartCoroutine(SetImage(NetworkScript.invites[index].photoUrl, avatar));

        int num = index + 1;
        indexText.text = ""+num;
        if(NetworkScript.invites[index].email.Equals("no"))
            star.SetActive(true); 
        else
            star.SetActive(false);
                 
        FindObjectOfType<FruitSpearGameCode>().inviteItems.Add(this.gameObject);

    }

     IEnumerator SetImage(string URLstring, RawImage image)
    {
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

