using UnityEngine;
using System.Collections;

public class CamRot : MonoBehaviour {

    public Transform player;
    Transform cam;
    public float distance = 10;

    bool selectionMode = false;

    float x;
    float y;

    // Use this for initialization
    void Start () {
        cam = GetComponentInChildren<Camera>().transform;
        selectionMode = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
	
	// Update is called once per frame
	void Update () {


        if (Input.GetKeyDown(KeyCode.Escape)) {
            SwitchCursor();
        }

        if (!selectionMode)
        {

            //Check for things under the camera and rotate cam
            Ray ray1 = new Ray(cam.position, -cam.transform.up);
            RaycastHit hit1;
            if (Physics.Raycast(ray1, out hit1, 1.5f))
            {
                if (Input.GetAxis("Mouse Y") < 0)
                {
                    x -= Input.GetAxis("Mouse Y");
                }
            }
            else
                x -= Input.GetAxis("Mouse Y");
            y += Input.GetAxis("Mouse X");
            transform.rotation = Quaternion.Euler(x, y, 0);

            //Move player
            if (Input.GetMouseButton(0))
            {
                float currentY = transform.eulerAngles.y;
                player.eulerAngles = new Vector3(0, currentY, 0);
            }
        }
    }

    void SwitchCursor() {
        selectionMode = !selectionMode;
        if (!selectionMode)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
