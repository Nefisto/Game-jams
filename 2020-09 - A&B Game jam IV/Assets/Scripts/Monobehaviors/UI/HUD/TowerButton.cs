using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NDream;
using UnityEngine.EventSystems;
using System;

public class TowerButton : LazyBehavior
{
    public Color disableColor;
    public int index;

    [Space]

    public Image towerSprite;
    private Color originalTowerSpriteColor;
    
    [Header("Proc events")]
    public GameEventInt changeCurrentTower;

    [Space]
    public IntReference playerMoney;

    private Color originalColor;
    public int cost;

    private void Start()
    {
        originalColor = image.color;
        originalTowerSpriteColor = towerSprite.color;

        cost = GameManager.instance.GetTower(index).cost;
    }

    private void Update()
    {
        VerifyBuyCondition();
    }

    private void VerifyBuyCondition()
    {
        if (button.interactable && playerMoney.Value < cost)
            DisableButton();
        else if (!button.interactable && playerMoney.Value >= cost)
            EnableButton();
    }

    private void EnableButton()
    {
        button.interactable = true;
        towerSprite.color = originalTowerSpriteColor;
    }

    private void DisableButton()
    {
        button.interactable = false;
        towerSprite.color = disableColor;
    }
}
