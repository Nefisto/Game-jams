using UnityEngine;

public class PumpkinLifeHUD : MonoBehaviour
{
    [Header("Status")]

    public Gradient colorGradient;

    [Header("Reference")]
    public SpriteRenderer sprite;
    public PumpkinLife pumpkinLife;

    #region Monobehaviour callbacks

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        pumpkinLife.OnUpdateLife += UpdateLifeHUD;
    }

    private void OnDisable()
    {
        pumpkinLife.OnUpdateLife -= UpdateLifeHUD;
    }

    #endregion

    #region Private Methods

    private void UpdateLifeHUD(float percent)
    {
        var newColor = colorGradient.Evaluate(percent);
        
        sprite.color = newColor;
        sprite.size = new Vector2(percent, sprite.size.y);
    }

    #endregion
}