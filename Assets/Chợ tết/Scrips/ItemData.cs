using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public ItemId itemId;
    public string itemName;
    public Sprite icon;
    [TextArea] public string description;

    [Space(10)]
    public bool isUsableAtKitchen = false;
    public ItemData resultItemAfterUsed;
}
public enum ItemId
{
    //Color == 1x
    Color_Red = 11,
    Color_Green = 2,
    Color_Blue = 3,
    Color_Yellow = 4,
    Color_White = 5,

    //Ingredient = 2x
    Ingredient_Gac = 21,
    Ingredient_GaoNep = 22,
    Ingredient_LaTiaTo = 23,
    Ingredient_LaNhoNoi = 24,
    Ingredient_Nghe = 25,

    //Kitchen Item = 3x
    KitchenItem_Pot = 31,
    KitchenItem_Knife = 32,
    KitchenItem_OngDua = 33,
    KitchenItem_Wasp = 34,
    KitchenItem_Luoc = 35
}