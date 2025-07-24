using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerChooser : MonoBehaviour
{
    public GameObject buildSpaceVinculed;

    public void Appear()
    {
        gameObject.SetActive(true);
    }

    public void Disappear()
    {
        gameObject.SetActive(false);
    }
    
}
