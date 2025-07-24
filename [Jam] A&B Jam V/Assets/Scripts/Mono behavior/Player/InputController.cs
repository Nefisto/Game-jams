using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [Header("Status")]
    public AnimationCurve accelerationCurve;
    public float maxSpeed = 5f;

    [Header("References")]
    [SerializeField] private PumpkinEquip pumpkinEquip;
    
    private float pressingTime = 0f;

    private GameSettings gameSettings;

    #region Monobehaviour callbacks

    private void Awake()
    {
        gameSettings = GameSettings.Instance;
    }

    private void Update()
    {
        MovementInput();
        PumpkinInput();
    }

    #endregion

    #region Private Methods

    private void MovementInput()
    {
        var horizontalSpeed = Input.GetAxisRaw("Horizontal");
        var verticalSpeed = Input.GetAxisRaw("Vertical");

        pressingTime = (horizontalSpeed != 0 || verticalSpeed != 0)
            ? Mathf.Clamp(pressingTime + Time.deltaTime, 0f, 1f)
            : 0f;

        var normalizedDirection = new Vector2(horizontalSpeed, verticalSpeed).normalized;
        var intensity = accelerationCurve.Evaluate(pressingTime) * maxSpeed;

        transform.Translate(normalizedDirection * (intensity * Time.deltaTime));
    }

    private void PumpkinInput()
    {
        if (Input.GetKeyDown(gameSettings.inputs.changeHead))
        {
            pumpkinEquip.WearPumpkin();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            pumpkinEquip.ReleaseCurrentPumpkin();
        }
    }
    
    #endregion
}