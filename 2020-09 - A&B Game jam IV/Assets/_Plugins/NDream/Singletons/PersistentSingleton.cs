using UnityEngine;

public class PersistentSingleton<T> : MonoBehaviour
    where T: Component
{
    public static T instance { get; protected set; }
 
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            throw new System.Exception("An instance of this singleton already exists.");
        }
        else
        {
            instance = this as T;
        }
        
    }
}