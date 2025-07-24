using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrowDetector : MonoBehaviour
{
    public event Action OnEnableEnemyDetector;
    public event Action OnDisableEnemyDetector;
    
    [Header("Status")]
    public float maxRadius = 3f;
    public float damagePerSecondOnCrows = 10f;
    
    [Header("Ref")]
    public CircleCollider2D circleDetector;
    public PumpkinEquip equippedPumpkin;
    
    [Header("Debug")]
    [SerializeField, ReadOnly] private float currentScaryRadius = 3f;

    [Space]
    [SerializeField] private bool canScary = true;
    [SerializeField] private List<CrowLife> crowsInsideCircle;
    
    // Circle
    [Range(0,50)]
    public int segments = 50;
    private LineRenderer crowDetectorLine;

    #region Monobehaviour callbacks

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        // Handles.color = Color.yellow;
        // Handles.DrawWireDisc(transform.position, Vector3.forward, currentScaryRadius);
        //
        Handles.color = Color.red;
        foreach (var crow in crowsInsideCircle)
        {
            Handles.DrawAAPolyLine(Texture2D.whiteTexture, transform.position, crow.transform.position);
        }
    }

#endif
    
    private void OnValidate()
    {
        currentScaryRadius = maxRadius;
        circleDetector.radius = currentScaryRadius;
    }

    private void OnEnable()
    {
        GameEvents.OnStartGame += Setup;
    }

    private void OnDisable()
    {
        GameEvents.OnStartGame -= Setup;
    }

    private void Awake()
    {
        crowDetectorLine = GetComponent<LineRenderer>();
        
        equippedPumpkin.OnChangeEquippedPumpkin += (oldHead, newHead) =>
        {
            if (oldHead != null)
            {
                oldHead.OnUpdateLife -= UpdateScaryRadius;
                oldHead.OnDie -= DisableDetector;
            }

            if (newHead != null)
            {
                EnableDetector();
                newHead.OnUpdateLife += UpdateScaryRadius;
                newHead.OnDie += DisableDetector;
            }
        };
    }

    private void Start ()
    {
        crowDetectorLine.positionCount = segments + 1;
        crowDetectorLine.useWorldSpace = false;
    }

    private void Update()
    {
        if (canScary)
            ScaryCrows();
        
        // Draw Circle
        DrawCircle(currentScaryRadius);
        
        // CHEAT
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("UI/EndScreen");
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.TryGetComponent<CrowLife>(out var crow))
            crowsInsideCircle.Add(crow);
    }

    private void OnTriggerExit2D (Collider2D other)
    {
        if (other.TryGetComponent<CrowLife>(out var crow))
        {
            crowsInsideCircle.Remove(crow);
            
            if (crow.crowState != CrowState.Running)
            {
                crow.ChangeState(CrowState.Eating);
                // crow.TryEat();
            }
        }
    }

    #endregion
    
    #region Private Methods

    private void Setup()
    {
        canScary = true;
        DisableDetector();
    }
    
    private void ScaryCrows()
    {
        var damageOnThisFrame = damagePerSecondOnCrows * Time.deltaTime;
        // foreach (var crow in crowsInsideCircle)
        for (var i = crowsInsideCircle.Count - 1; i >= 0; i--)
        {
            crowsInsideCircle[i].GetScared(damageOnThisFrame);
        }
    }

    private void UpdateScaryRadius (float percentage)
    {
        var newRadius = maxRadius * percentage;

        currentScaryRadius = newRadius;  
        circleDetector.radius = newRadius;
    }

    private void EnableDetector()
    {
        circleDetector.enabled = true;
        
        OnEnableEnemyDetector?.Invoke();
    }
    
    private void DisableDetector()
    {
        circleDetector.enabled = false;
     
        OnDisableEnemyDetector?.Invoke();
    }
    
    private void DrawCircle (float radius)
    {
        float x, y;
        var angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin (Mathf.Deg2Rad * angle) * radius;
            y = Mathf.Cos (Mathf.Deg2Rad * angle) * radius;

            crowDetectorLine.SetPosition (i,new Vector3(x,y,0) );

            angle += (360f / segments);
        }
    }

    #endregion
}