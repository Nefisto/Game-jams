using Sirenix.OdinInspector;
using UnityEngine;

[SelectionBase]
public class Ground : MonoBehaviour
{
    [field: TitleGroup("Settings")]
    [field: SerializeField]
    public GroundType GroundType { get; private set; }
}