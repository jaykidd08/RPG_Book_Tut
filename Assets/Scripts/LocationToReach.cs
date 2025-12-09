using UnityEngine;

public class LocationToReach : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        if (other.name == "Player")
        {
            GameObject.Find("GameManager").GetComponent<QuestSystem>().Notify(QuestSystem.possibleActions.enter_place_called, gameObject.name);
        }

    }
}
