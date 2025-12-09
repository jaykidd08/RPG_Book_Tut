using UnityEngine;

public class ManageShopItem : MonoBehaviour
{
    public void IncreaseQuantity() 
    { 

        print("Just Clicked"); 
        transform.parent.GetComponent<ShopItem>().IncreaseQuantity(); 

    }
    public void DecreaseQuantity() 
    { 

        transform.parent.GetComponent<ShopItem>().DecreaseQuantity(); 

    }
}
