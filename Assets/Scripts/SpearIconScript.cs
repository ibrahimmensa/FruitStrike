using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpearIconScript : MonoBehaviour
{
    private int index;
    public RawImage icon;
    void Start()
    {

        index = FindObjectOfType<FruitSpearGameCode>().spearIcons.Count;
        float offset = (Screen.height/32f) * index;
        float y = (Screen.height / 6.4f) + offset;
        transform.position = new Vector3((Screen.height / 16f), y, 0);
        //Debug.Log("Spear Icon "+index+" spawned at Y;"+y+" Offset: "+offset);
        FindObjectOfType<FruitSpearGameCode>().spearIcons.Add(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (FruitSpearGameCode.spearCount <= index)
            icon.texture = (Texture2D)Resources.Load("spear_shadow");
        else{
            icon.texture = FruitUI.currentSpear;
		}
    }
}
