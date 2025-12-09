using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageButton : MonoBehaviour
{

    public void StartGame()
    {
        SceneManager.LoadScene("level1");
    }
}
