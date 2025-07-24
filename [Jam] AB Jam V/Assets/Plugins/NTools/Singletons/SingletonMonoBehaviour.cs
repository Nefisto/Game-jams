using UnityEngine;

public class SingletonMonoBehaviour<T> : LazyBehavior 
    where T: Component
{
    public static T Instance { get; protected set; }
 
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            throw new System.Exception("An instance of this singleton already exists.");
        }

        Instance = this as T;
    }
}