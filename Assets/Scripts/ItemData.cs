using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    public string id;
    public string itemName;
    public Sprite icon;
    public string description;

    public abstract void ApplyEffects(Player player);
    public abstract void TriggerEffect(Player player, ItemEffectTrigger trigger);
}