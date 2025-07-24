using UnityEngine;

// Use somente se as informações compartilhadas aqui são referentes somente a cena atual
public class SingletonBehaviour<T> : MonoBehaviour 
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