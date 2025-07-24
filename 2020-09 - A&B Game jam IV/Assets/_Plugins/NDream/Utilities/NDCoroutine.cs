using System.Collections;
using UnityEngine;

public static class NDCoroutine
{
    public static IEnumerator WaitForFrames(int framesCount)
    { 
        while (framesCount-- > 0)
            yield return null;
    }
}