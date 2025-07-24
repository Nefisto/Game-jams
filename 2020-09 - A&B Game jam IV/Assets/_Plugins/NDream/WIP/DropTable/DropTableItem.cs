using UnityEngine;

public class DropItem
{
    public GameObject prefab;
    public int amount;
}

[System.Serializable]
public class DropTableItem
{
    public GameObject prefab;

    public int weight;

    public float percent;

    public bool isMultiple;

    public IntRange amount;

    public bool isGuaranted;

    // Cach
    public int lastWeight;

    public static implicit operator DropItem(DropTableItem dropTableItem)
        => new DropItem(){prefab = dropTableItem.prefab, 
                          amount = dropTableItem.isMultiple ? dropTableItem.amount.GetRandom() : 1};
}