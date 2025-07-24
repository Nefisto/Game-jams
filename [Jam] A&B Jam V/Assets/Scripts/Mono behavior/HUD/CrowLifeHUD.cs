using UnityEngine;

public class CrowLifeHUD : MonoBehaviour
{
    [Header("Status")]
    public Gradient gradient;

    [Header("Reference")]
    [SerializeField] private CrowLife crowLife;
    [SerializeField] private SpriteRenderer spriteRenderer;

    #region Monobehaviour callbacks

    private void OnEnable()
    {
        crowLife.OnReachPumpkin += ShowHUD;
        crowLife.OnScape += HideHUD;
            
        crowLife.OnUpdateLifePercentage += SetLifePercentage;
    }

    #endregion

    #region Private Methods

    private void SetLifePercentage(float percentage)
    {
        spriteRenderer.color = gradient.Evaluate(percentage);
        spriteRenderer.size = new Vector2(percentage, spriteRenderer.size.y);
    }

    public void ShowHUD()
    {
        spriteRenderer.enabled = true;
    }

    public void HideHUD()
    {
        spriteRenderer.enabled = false;
    }

    #endregion
}