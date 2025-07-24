using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using NDream;

public class BuilderSpace : LazyBehavior
{
    [SerializeField] private bool isEmpty = true;

    public Color possibleColor;
    public Color impossibleColor;

    public IntReference playerMoney;
    public IntReference currentTowerIndex;

    private bool isBuilding = false;

    private Color originalColor;

    // private int currentIndex;

    private void Start()
    {
        originalColor = spriteRenderer.color;
    }

    public void SetBuildingStatus(int value)
    {
        if (!isEmpty)
            return;

        spriteRenderer.enabled = isBuilding = value >= 0;
    }

    public void ChangeColorBasedOnEmptyness()
    {
        if (currentTowerIndex < 0)
            return;

        spriteRenderer.color = isEmpty ? possibleColor : impossibleColor;
    }

    public void ChangeColorToOriginal()
    {
        spriteRenderer.color = originalColor;
    }

    public void BuildTower(BaseEventData baseEvent)
    {
        var pointerEvent = (PointerEventData)baseEvent;

        var worldPos = Camera.main.ViewportToWorldPoint(pointerEvent.position);
        
        isEmpty = false;
    }

    public void TryInsertBuild()
    {
        if (!isEmpty)
            return;
        
        if (currentTowerIndex.Value < 0)
            return;
        
        var currentTower = transform.GetChild(currentTowerIndex.Value).GetComponent<BasicTower>();
        if (currentTower.CanBuy())
        {
            transform.GetChild(currentTowerIndex.Value).gameObject.SetActive(true);

            isEmpty = false;

            playerMoney.Value -= currentTower.cost;

            spriteRenderer.enabled = false;
        }
    }
}
