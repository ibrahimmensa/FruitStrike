using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransactionChild : MonoBehaviour
{
    private int index;
    public Text info;


    void Start()
    {
        index = FindObjectOfType<FruitSpearGameCode>().transactionItems.Count;
        info.text = NetworkScript.transactions[index];
      
                 
        FindObjectOfType<FruitSpearGameCode>().transactionItems.Add(this.gameObject);

    }
}
