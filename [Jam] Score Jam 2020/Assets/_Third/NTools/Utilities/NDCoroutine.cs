using System.Collections;

public static class NDCoroutine
{
    public static IEnumerator WaitForFrames(int framesCount)
    { 
        while (framesCount-- > 0)
            yield return null;
    }
}