using System.Collections.Generic;
using UnityEngine;

public class HeldItems : MonoBehaviour
{
    public List<ItemData> items = new List<ItemData>();

    public delegate void OnItemChanged(ItemData item, bool wasAdded);
    public event OnItemChanged onItemChangedCallback;

    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    public void AddItem(ItemData item)
    {
        if (item == null) return;

        items.Add(item);
        item.ApplyEffects(player);
        onItemChangedCallback?.Invoke(item, true);
    }

    public bool HasItem(string itemId)
    {
        return items.Exists(item => item.id == itemId);
    }

    public void TriggerItemEffects(ItemEffectTrigger trigger)
    {
        foreach (var item in items)
        {
            item.TriggerEffect(player, trigger);
        }
    }
}

public enum ItemEffectTrigger
{
    OnDamageTaken,
    OnEnemyDefeated,
    OnRoomEntered,
    OnItemPickup
    // Add more triggers as needed
}