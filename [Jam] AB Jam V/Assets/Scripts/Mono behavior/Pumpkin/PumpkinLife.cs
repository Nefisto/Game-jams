using System;
using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public enum PumpkinState
{
    OnEarth,
    OnHead
}

public class PumpkinLife : MonoBehaviour
{
    public event Action<float> OnUpdateLife;
    public event Action OnDie;

    public static event Action OnLosePumpkin;
    
    [ReadOnly] public float currentLife = 0f;
    
    [Header("References")]
    private SpriteRenderer sprite; 

    [Header("Debug")]
    [ReadOnly] public PumpkinState currentState = PumpkinState.OnEarth;
    [SerializeField] private bool canUpdateLife = false;

    private Pumpkin pumpkin;

    public Transform pumpkinFolder;
    public Transform releasePosition;

    public RandomAudioEvent bitSound;

    private bool canPlayBitSound = true;
    
    private AudioSource audioSource;
    
    #region Monobehaviour callbacks

    private GameSettings gameSettings;
    
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();

        pumpkin = GetComponent<Pumpkin>();

        gameSettings = GameSettings.Instance;

        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        GameEvents.OnStartGame += Setup;
    }

    private void OnDisable()
    {
        GameEvents.OnStartGame -= Setup;
    }

    private void Update()
    {
        if (canUpdateLife)
        {
            if (currentState == PumpkinState.OnEarth)
            {
                if (gameSettings.pumpkin.healWhenHaveCrows)
                    GainLife();
                else
                {
                    var numberOfCrowsEating = pumpkin
                        .crowsEatingMe
                        .Count(crow => crow.GetCrowState == CrowState.Eating);
                        
                    if (numberOfCrowsEating == 0)
                        GainLife();
                }
            }
            else
                LoseLife();
        }
    }

    #endregion

    #region API

    public void ChangeState (PumpkinState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case PumpkinState.OnEarth:
                sprite.sortingOrder = 0;
                transform.position = releasePosition.position;
                transform.parent = pumpkinFolder;
                pumpkin.EnableGraphics();
                break;

            case PumpkinState.OnHead:
                sprite.sortingOrder = 2;
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #endregion

    #region Private Methods

    private void Setup()
    {
        SetLife(Random.Range(gameSettings.pumpkin.initialLife.x, gameSettings.pumpkin.initialLife.y));
        canUpdateLife = true;
    }
    
    private void GainLife()
    {
        var gainedThisFrame = gameSettings.pumpkin.lifeGainedPerSecond * Time.deltaTime;
        var newAmount = Mathf.Clamp(currentLife + gainedThisFrame, 0f, gameSettings.pumpkin.maxLife);

        SetLife(newAmount);
    }

    private void LoseLife()
    {
        var lostThisFrame = gameSettings.pumpkin.lifeLostPerSecond * Time.deltaTime;
        var newAmount = Mathf.Clamp(currentLife - lostThisFrame, 0f, gameSettings.pumpkin.maxLife);

        SetLife(newAmount);
    }

    public void GetBit(float bitDamage)
    {
        var newAmount = Mathf.Clamp(currentLife - bitDamage, 0f, gameSettings.pumpkin.maxLife);

        if (canPlayBitSound)
        {
            bitSound.PlayOneShot(audioSource);
            StartCoroutine(Delay());
        }
        
        SetLife(newAmount);

        IEnumerator Delay()
        {
            canPlayBitSound = false;
            yield return new WaitForSeconds(3f);
            canPlayBitSound = true;
        }
    }

    private void SetLife(float newLife)
    {
        if (newLife.IsNearlyEnoughTo(0f))
        {
            OnDie?.Invoke();
            Die();
        }
        else
        {
            currentLife = newLife;

            var newPercent = newLife / gameSettings.pumpkin.maxLife;

            OnUpdateLife?.Invoke(newPercent);
        }
    }

    private void Die()
    {
        canUpdateLife = false;
        
        gameObject.SetActive(false);
        PumpkinManager.AlivePumpkins.Remove(pumpkin);
        OnLosePumpkin?.Invoke();

        Destroy(gameObject, .3f);
    }

    #endregion

    #region Debug

    [Button, DisableInEditorMode]
    private void ResetLife()
    {
        SetLife(1f);
    }

    #endregion
}