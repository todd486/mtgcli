﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battlefield : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateCard(GameObject card) {
        GameObject newCard = Instantiate(card);
        newCard.transform.parent = gameObject.transform;
    }
}
