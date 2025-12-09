using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSystem : MonoBehaviour
{
    public List<Item> shopItems; 
    public GameObject shopItemComponent; 
    GameObject[] shopItemComponents; 
    int totalPurchase = 0; 
    int initialMoney; 
    public int moneyLeft; 
    float topLeftX, topLeftY;

    public void Start()
    {

        //Init();

    }

    public void Init()
    {

        //initialMoney = 1000;
        initialMoney = GameObject.Find("Player").GetComponent<InventorySystem>().GetMoney();
        moneyLeft = initialMoney;
        topLeftX = 200;
        topLeftY = 350;

        shopItems = new List<Item>();
        shopItems.Add(new Item(Item.ItemType.YELLOW_DIAMOND));
        shopItems.Add(new Item(Item.ItemType.BLUE_DIAMOND));
        shopItems.Add(new Item(Item.ItemType.RED_DIAMOND));
        shopItems.Add(new Item(Item.ItemType.MEAT));
        shopItems.Add(new Item(Item.ItemType.APPLE));

        shopItemComponents = new GameObject[shopItems.Count];
        GameObject.Find("shopMoneyLeftValue").GetComponent<TextMeshProUGUI>().text = "" + initialMoney;
        for (int i = 0; i < shopItems.Count; i++)
        {

            SetupShopItemComponent(i);

        }

        void SetupShopItemComponent(int index)
        {

            shopItems[index].nb = 0;
            shopItemComponents[index] = Instantiate(shopItemComponent, transform.position, Quaternion.identity); shopItemComponents[index].GetComponent<ShopItem>().index = index;

            float width = shopItemComponents[index].transform.Find("itemBg").GetComponent<RectTransform>().sizeDelta.x;
            float borderAroundEachItem = 1.05f;
            shopItemComponents[index].name = "shopItem_" + index + shopItems[index].name;
            shopItemComponents[index].transform.Find("itemLabel").GetComponent<TextMeshProUGUI>().text = shopItems[index].name + "($" + shopItems[index].price + ")";
            shopItemComponents[index].transform.Find("itemQty").GetComponent<TextMeshProUGUI>().text = "" + shopItems[index].nb;

            shopItemComponents[index].transform.parent = GameObject.Find("shopUI").transform;
            shopItemComponents[index].transform.localPosition = new Vector3(topLeftX + (index % 3) * (width * borderAroundEachItem), topLeftY - (index / 3) * width * borderAroundEachItem, 0.0f);
            shopItemComponents[index].transform.Find("itemImage").GetComponent<RawImage>().texture = shopItems[index].GetTexture();

        }

    }

    public void UpdateTotal(int itemIndex, int itemAmount)
    {

        shopItems[itemIndex].nb = itemAmount;
        int tempTotal; tempTotal = CalculateTotal();
        GameObject.Find("shopTotalValue").GetComponent<TextMeshProUGUI>().text = "" + tempTotal;
        totalPurchase = tempTotal;
        moneyLeft = initialMoney - tempTotal;
        GameObject.Find("shopMoneyLeftValue").GetComponent<TextMeshProUGUI>().text = "" + moneyLeft;

    }

    public int CalculateTotal() 
    { 

        int temp = 0; 
        for (int i = 0; i < shopItems.Count; i++) 
        { 
            temp += shopItems[i].nb * shopItems[i].price; 
        } 

        return temp; 

    }

    public bool canAddItemsToCart(int index)
    {

        if (moneyLeft >= shopItems[index].price && shopItems[index].nb < shopItems[index].maxNb)
        {
            return true;
        }
        else
        { 
            return false;
        }
    }
}

