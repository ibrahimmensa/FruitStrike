using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemScript : MonoBehaviour
{
    public RawImage image, back, peach;
    public Text priceText;
    private int index, price, picNum, has;
    public static bool hasItem;
    Texture2D blank;

    void Start()
    {
        index = FindObjectOfType<FruitSpearGameCode>().spearItems.Count;
		
		picNum = index + 1;
		price = FruitUI.spearPrices[index];

        image.texture = (Texture2D)Resources.Load("Spears/spear" + picNum);
        //Debug.Log("Picture file: " + "spear" + picNum);
        priceText.text = price + "";
        FindObjectOfType<FruitSpearGameCode>().spearItems.Add(this.gameObject);
        GetComponent<Button>().onClick.AddListener(() => selectItem());


        peach.texture = (Texture2D)Resources.Load("peach");
        blank = (Texture2D)Resources.Load("blank");
        
    }

    void selectItem()
    {

        FindObjectOfType<FruitSpearGameCode>().playSound(FruitSpearGameCode.Sound.Select);
        has = PlayerPrefs.GetInt("spear" + picNum);
        Debug.Log("Tier: " + FruitUI.Tier + " Selecting Item "+ picNum);
        Debug.Log("Item status is " + has);

        if (has == 1)
        {
            hasItem = true;
            FindObjectOfType<FruitUI>().selectedItemprice.text = "";
            FindObjectOfType<FruitUI>().buyText.text = "EQUIP";
            FindObjectOfType<FruitUI>().peachIcon.texture = (Texture2D)Resources.Load("blank");
        }
        else 
        {
            hasItem = false;
            FruitUI.currentPrice = price;
            FindObjectOfType<FruitUI>().selectedItemprice.text = price + "";
            FindObjectOfType<FruitUI>().buyText.text = "BUY";
            FindObjectOfType<FruitUI>().peachIcon.texture = (Texture2D)Resources.Load("peach");
        }

        if (FruitUI.equippedItem == picNum)
        {
            Debug.Log("Button Disabled");
            FindObjectOfType<FruitUI>().selectedItemprice.text = "EQUIPPED";
            FindObjectOfType<FruitUI>().peachIcon.texture = (Texture2D)Resources.Load("blank");
        }
        else
        {
            Debug.Log("Button Enabled");
        }

        FindObjectOfType<FruitUI>().selectedItemImage.texture = image.texture;
        FruitUI.selectedItem = picNum;

    }

    void Update()
    {
        if(FruitUI.selectedItem == picNum)
           back.texture =  (Texture2D)Resources.Load("highlight");
        else
            back.texture = (Texture2D)Resources.Load("blank");

        has = PlayerPrefs.GetInt("spear" + picNum);
        if (has == 1)
        {
            priceText.text = "";
            peach.texture = blank;
        }
    }
}
