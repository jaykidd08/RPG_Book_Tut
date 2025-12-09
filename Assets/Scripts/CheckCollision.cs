using UnityEngine;

public class CheckCollision : MonoBehaviour
{

    int damage = 20;

    private void OnTriggerEnter(Collider other)
    {
        damage = GameObject.Find("GameManager").GetComponent<GameManager>().player.power / 2;

        if (other.GetComponent<Collider>().gameObject.tag == "target")
        {
            other.GetComponent<Collider>().gameObject.GetComponent<ManageTargetHealth>().DecreaseHealth(damage);
        }

    }
}
