using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public enum CrowState
{
    Reaching,
    Scared,
    Eating,
    Running
}

public class CrowLife : LazyBehavior
{
    public event Action OnReachPumpkin;
    public event Action OnScape;
    public event Action<float> OnUpdateLifePercentage;

    [Header("Status")]
    [ReadOnly] public CrowState crowState = CrowState.Reaching;
    [ReadOnly] public float currentHungry = 0f;
    [ReadOnly] public Pumpkin targetPumpkin;
        
    [Header("References")]
    [SerializeField] private CircleCollider2D crowCollider;
    [SerializeField] private CrowLifeHUD lifeHUD;

    private GameSettings gameSettings;

    private Coroutine eatingCoroutine;
    
    private Crow crow;
    private static readonly int Flying = Animator.StringToHash("Flying");
    private static readonly int Eating = Animator.StringToHash("Eating");
    private static readonly int Spooked = Animator.StringToHash("Spooked");

    public RandomAudioEvent runAwayAudio;
    public RandomAudioEvent spookedAudio;
    
    private AudioSource audioSource;
    
    #region Monobehaviour callbacks

    private void Awake()
    {
        gameSettings = GameSettings.Instance;
        crow = GetComponent<Crow>();

        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        Setup();
    }

    #endregion

    #region API

    [Button, DisableInEditorMode]
    public void GetScared(float scaryLevel)
    {
        if (crowState != CrowState.Scared)
            ChangeState(CrowState.Scared);
        
        currentHungry += scaryLevel;

        if (currentHungry >= gameSettings.crow.maxHungry)
        {
            RunAway();
        }
        else
        {
            var percentage = currentHungry / gameSettings.crow.maxHungry;
            OnUpdateLifePercentage?.Invoke(percentage);
        }
    }

    private Coroutine scareByClick;
    
    public void GetScaredByClick ()
    {
        if (scareByClick != null)
            StopCoroutine(scareByClick);
        
        scareByClick = StartCoroutine(_GetScaredByClick());

        IEnumerator _GetScaredByClick()
        {
            GetScared(gameSettings.crow.clickDamage);
            
            yield return new WaitForSeconds(.5f);

            ChangeState(CrowState.Eating);
        }
    }
    
    public IEnumerator Eat()
    {
        while (true)
        {
            currentHungry += gameSettings.crow.biteDamage;
            
            if (targetPumpkin == null)
                yield break;
            
            targetPumpkin.GetBit(gameSettings.crow.biteDamage);
                
            if (currentHungry >= gameSettings.crow.maxHungry)
            {
                RunAway();
            }
            else
            {
                var percentage = currentHungry / gameSettings.crow.maxHungry;
                OnUpdateLifePercentage?.Invoke(percentage);
            }

            yield return new WaitForSeconds(Random.Range(gameSettings.crow.timeBetweenBite.x, gameSettings.crow.timeBetweenBite.y));
        }
    }

    public void StopEat()
    {
        if (eatingCoroutine != null)
        {
            StopCoroutine(eatingCoroutine);
            eatingCoroutine = null;
        }
    }

    public void ReachPumpkin(Pumpkin target)
    {
        OnReachPumpkin?.Invoke();

        lifeHUD.ShowHUD();

        targetPumpkin = target;
        targetPumpkin.AddCrow(GetComponent<Crow>());
        
        target.OnDie += StopEat;
        
        crow.FixDirection(target.transform);

        ChangeState(CrowState.Eating);
    }
    
    public void RunAway()
    {
        ChangeState(CrowState.Running);
    }

    #endregion

    #region Private Methods

    private void Setup()
    {
        currentHungry = 0;
        crowCollider.enabled = true;
        
        var percentage = currentHungry / gameSettings.crow.maxHungry;
        OnUpdateLifePercentage?.Invoke(percentage);
        
        lifeHUD.HideHUD();
    }

    public void ChangeState (CrowState newState)
    {
        crowState = newState;
        
        switch (newState)
        {
            case CrowState.Reaching:
                break;

            case CrowState.Scared:
                if (eatingCoroutine != null)
                    StopCoroutine(eatingCoroutine);
                
                spookedAudio.PlayOneShot(audioSource);
                
                animator.SetTrigger(Spooked);
                break;

            case CrowState.Eating:
                if (eatingCoroutine != null)
                    StopCoroutine(eatingCoroutine);

                eatingCoroutine = StartCoroutine(Eat());
                
                animator.SetTrigger(Eating);
                break;

            case CrowState.Running:
                if (this == null)
                    return;
                
                if (crowCollider != null)
                    crowCollider.enabled = false;
                
                if (targetPumpkin != null)
                    targetPumpkin.RemoveCrow(GetComponent<Crow>());

                animator.SetTrigger(Flying);
                StartCoroutine(_RunAway());
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        IEnumerator _RunAway()
        {
            runAwayAudio.PlayOneShot(audioSource);
            
            var runPosition = RandomizeExitPosition();

            crow.FixDirection(runPosition);
            
            var initialPosition = transform.position;
            var percentage = 0f;
            while (Vector2.SqrMagnitude(runPosition - (Vector2)transform.position) > 2f)
            {
                var newPos = Vector2.Lerp(initialPosition, runPosition, percentage);

                transform.position = newPos;

                percentage += Time.deltaTime;

                yield return null;
            }
            
            OnScape?.Invoke();

            if (targetPumpkin)
                targetPumpkin.OnDie -= StopEat;
            
            StopAllCoroutines();
            crow.StopAllCoroutines();
            
            Destroy(gameObject, .3f);
        }
    }

    private Vector2 RandomizeExitPosition()
    {
        var xMultiplier = Random.value < .5f ? 1 : -1;
        var yMultiplier = Random.value < .5f ? 1 : -1;

        return (Vector2)transform.position + new Vector2(20 * xMultiplier, 20 * yMultiplier);
    }
    
    #endregion
}