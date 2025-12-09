using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItem : MonoBehaviour
{
    string name; 
    int price, quantity; 
    public int index;

    private void Start()
    {

        quantity = 0;
        UpdateQuantityLabel();

    }

    public void IncreaseQuantity() 
    { 

        if (!canClick()) return; 
        quantity++; 
        UpdateQuantityLabel(); 

    }
    public void DecreaseQuantity() 
    { 

        quantity--; 
        if (quantity < 0) quantity = 0; 
        UpdateQuantityLabel(); 

    }
    void UpdateQuantityLabel() 
    { 

        transform.Find("itemQty").GetComponent<TextMeshProUGUI>().text = "" + quantity;

        GameObject.Find("shopSystem").GetComponent<ShopSystem>().UpdateTotal(index, quantity);

    }
    bool canClick() 
    { 

        //return true;

        GameObject shopSystem = GameObject.Find("shopSystem"); 
        return shopSystem.GetComponent<ShopSystem>().canAddItemsToCart(this.index);

    }
}
