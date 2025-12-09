using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    GameObject target;
    public bool indoorMode = false;
    GameObject closeRangeCamera;

    private void Start()
    {

        target = transform.parent.gameObject;
        closeRangeCamera = GameObject.Find("cameraIndoor");
        closeRangeCamera.SetActive(false);

    }

    private void Update()
    {

        transform.LookAt(target.transform.position);
        if (Input.GetKeyDown(KeyCode.C))
        {
            indoorMode = !indoorMode;
        }

        if (indoorMode)
        {
            closeRangeCamera.SetActive(true);
        }
        else 
        {
            //changed from deprecated active
            if (closeRangeCamera.activeSelf) closeRangeCamera.SetActive(false);
        }
    }
}
