using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropTable : MonoBehaviour
{ 
    [System.Serializable]
    public class Items
    {
        public ItemDrop item;
        public int weight; // æ∆¿Ã≈€ »Æ∑¸
    }

    public List<Items> items = new List<Items>();

    protected ItemDrop PickItem()
    {
        int sum = 0;
        foreach (var item in items)
        {
            sum += item.weight;
        }

        var random = Random.Range(0, sum);

        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            if (item.weight > random) return items[i].item;
            else random -= item.weight;
        }

        return null;
    }

    public void ItemDrop(Vector3 pos)
    {
        var item = PickItem();
        if (item == null) return;

        Instantiate(item.prefab, pos, Quaternion.identity);
    }
    
}
