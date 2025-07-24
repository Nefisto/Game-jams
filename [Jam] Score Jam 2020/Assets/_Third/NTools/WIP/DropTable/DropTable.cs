using System.Linq;
using System.Collections.Generic;
using UnityEngine;

// FRAME-TODO: Custom editor para aparecer a quantia somente quando o bool for true
// FRAME-TODO: Criar IntRange
// FRAME-TODO: Vincular droptable com pool manager
// FRAME-TODO: mudar o label do item para "prefab" ou "poolName" no custom editor

public enum DropTableKind
{
    WithReposition,
    WithoutReposition
}

[CreateAssetMenu(fileName = "Drop Table", menuName = "Framework/Drop Table/Default")]
public class DropTable : ScriptableObject
{
    public List<DropTableItem> loot = new List<DropTableItem>();

    public DropTableKind sampleKind;

    private int TotalWeight
    {
        get => loot.Sum(item => item.weight);
    }

    public bool isEmpty
    {
        get => loot.Count > 0 ? true : false;
    }

    public List<DropItem> Drop(int nDrops = 1)
    {
        List<DropItem> droppedLoot = new List<DropItem>();

        AddUniqueItem(droppedLoot, nDrops);
        AddGuarantedItems(droppedLoot);

        return droppedLoot;
    }

    private void AddUniqueItem(List<DropItem> droppedLoot, int nDrop = 1)
    {
        List<DropTableItem> possibleDrops = new List<DropTableItem>(loot);
        possibleDrops.RemoveAll((item) => item.isGuaranted || item.weight == 0 || !item.prefab);

        while (nDrop-- > 0)
        {
            int x = Random.Range(1, TotalWeight + 1);

            foreach (var item in possibleDrops)
            {
                x -= item.weight;

                if (x <= 0)
                {
                    droppedLoot.Add(item);
                    break;
                }
            }
        }
    }

    private void AddGuarantedItems(List<DropItem> droppedLoot)
    {
        // Get all guaranted items
        foreach (var item in loot)
        {
            // Ever drop this item
            if (item.isGuaranted)
                droppedLoot.Add(item);
        }
    }

    public void CalculatePercent()
    {
        var totalWeight = 0;

        if (loot.Count == 0)
            return;

        foreach (var item in loot)
            totalWeight += item.weight;

        foreach (var item in loot)
        {
            if (item.weight == 0)
            {
                item.percent = -1f;
                continue;
            }

            item.percent = (float)item.weight / totalWeight;
        }
    }
}