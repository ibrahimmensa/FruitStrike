using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearObject : MonoBehaviour
{
    void Start()
    {
        FruitSpearGameCode.spearObjects.Add(this.gameObject);
    }

    void Update()
    {
        
    }
}
