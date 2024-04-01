using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropTable : MonoBehaviour
{ 
    [System.Serializable]
    public class Items
    {
        public ItemDrop item;
        public int weight;
    }

    public List<Items> items = new List<Items>();

    protected ItemDrop PickItem()
    {
        int sum = 0;
        foreach (var item in items)
        {
            sum += item.weight;
        }

        var rnd = Random.Range(0, sum);

        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            if (item.weight > rnd) return items[i].item;
            else rnd -= item.weight;
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
