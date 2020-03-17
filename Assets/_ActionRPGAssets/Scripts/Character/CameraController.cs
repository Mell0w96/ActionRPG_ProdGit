using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSens;
    public Transform Target;
    public Rigidbody Player;    
  
    float mouseX, mouseY;
    float minYconstraint = -35f;
    float maxYconstraint = 60f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        CameraControl();
    }

    void CameraControl()    
    {
        mouseX += Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        mouseY -= Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;
        mouseY = Mathf.Clamp(mouseY, minYconstraint, maxYconstraint);

        transform.LookAt(Target);

        Target.rotation = Quaternion.Euler(mouseY, mouseX, 0);
        Player.rotation = Quaternion.Euler(Player.velocity.x, mouseX, Player.velocity.y);
        



    }
}
