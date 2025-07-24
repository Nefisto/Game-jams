using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSelectionButton : LazyBehavior
{
    public Color selectedColor = Color.green;
    private Color originalColor;

    private void Start()
    {
        originalColor = spriteRenderer.color;
    }

    public void ChangeSpriteColor()
    {
        spriteRenderer.color = selectedColor;
    }

    public void ChangeBackToOriginalColor()
    {
        spriteRenderer.color = originalColor;
    }
}
