using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManageBar : MonoBehaviour
{
    int value = 100;
    string label;

    private void Start()
    {
        UpdateValue();
        label = "Health";
        transform.Find("label").GetComponent<TextMeshProUGUI>().text = label;
    }

    public void IncreaseValue(int amount) 
    { 
        value += amount; 
        if (value > 100) value = 100; 
        UpdateValue(); 
    }

    void UpdateValue() 
    { 
        transform.Find("fill").transform.localScale = new Vector3(value / 100.0f, transform.localScale.y, transform.localScale.z); 
        transform.Find("text").GetComponent<TextMeshProUGUI>().text = "" + value; 
    }

    void Update() 
    { 
        //if (Input.GetKeyDown(KeyCode.B)) IncreaseValue(10); 
    }

    public void SetValue(int amount) 
    { 
        value = amount; 
        UpdateValue(); 
    }
}
