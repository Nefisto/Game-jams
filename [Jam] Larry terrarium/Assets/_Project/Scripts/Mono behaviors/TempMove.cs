using System;
using UnityEngine;

public class TempMove : MonoBehaviour
{
    private void Update()
    {
        var horizontalAxis = Input.GetKey(KeyCode.RightArrow) ? 1 : Input.GetKey(KeyCode.LeftArrow) ? -1 : 0;
        transform.Translate(new Vector3(horizontalAxis, 0f, 0f) * (10 * Time.deltaTime));
    }
}

