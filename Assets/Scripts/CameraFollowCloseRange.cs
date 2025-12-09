using UnityEngine;

public class CameraFollowCloseRange : MonoBehaviour
{
    public GameObject target;

    private void Update()
    {

        transform.LookAt(target.transform.position + Vector3.up);

    }
}
