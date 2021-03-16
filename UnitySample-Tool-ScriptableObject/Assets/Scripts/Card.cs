using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour {

    public CardAssetInfo cardAssetInfo;

    public Text nameText;
    public Image spriteImg;
    public Text desc;

    public void Start()
    {
        nameText.text = cardAssetInfo.name;
        spriteImg.sprite = cardAssetInfo.sprite;
        desc.text = cardAssetInfo.textDesc;

        
    }
}
