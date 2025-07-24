using UnityEngine;

public static class MyDebug
{
    public static string Log(Object obj)
    {
        Debug.Log(obj.ToString());
        return obj.ToString();
    }
    
    public static string Log(string message)
    {
        Debug.Log(message);
        return message;
    }
}