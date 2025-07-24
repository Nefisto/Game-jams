using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LifeHUD : MonoBehaviour
{
    [Header("Status")]
    public Gradient lifeGradient;
    
    [Header("Reference")]
    [SerializeField] private PumpkinEquip pumpkinEquip;
    [SerializeField] private CrowDetector crowDetector;
    
    private Image lifeHUD;
    
    #region Monobehaviour callbacks

    private void Awake()
    {
        lifeHUD = GetComponent<Image>();

        pumpkinEquip.OnChangeEquippedPumpkin += (oldPumpkin, newPumpkin) =>
        {
            if (oldPumpkin != null)
                oldPumpkin.OnUpdateLife -= UpdateLifePercent;

            if (newPumpkin != null)
                newPumpkin.OnUpdateLife += UpdateLifePercent;
        };

        crowDetector.OnEnableEnemyDetector += EnableHUD;
        crowDetector.OnDisableEnemyDetector += DisableHUD;
    }
    
    #endregion

    #region Private Methods

    private void UpdateLifePercent (float newPercent)
    {
        var newColor = lifeGradient.Evaluate(newPercent);
        
        lifeHUD.fillAmount = newPercent;
        lifeHUD.color = newColor;
    }

    private void EnableHUD()
    {
        lifeHUD.enabled = true;
    }
    
    private void DisableHUD()
    {
        lifeHUD.enabled = false;
    }

    #endregion
}