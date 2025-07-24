#pragma warning disable 0414

using UnityEngine;

[CreateAssetMenu(fileName = "BoolVariable", menuName = "Framework/Variables/Bool")]
public class BoolVariable : BaseVariable<bool> 
{ 
    public void Toggle()
        => Value = !Value;
}