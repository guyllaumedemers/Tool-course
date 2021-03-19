using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{

    public CardAssetInfo cardAssetInfo;

    public TextMeshProUGUI nameText;
    public SpriteRenderer spriteImg;
    public TextMeshProUGUI desc;

    public void Start()
    {
        nameText.text = cardAssetInfo.name;
        spriteImg.sprite = cardAssetInfo.sprite;
        desc.text = cardAssetInfo.textDesc;
    }
}
