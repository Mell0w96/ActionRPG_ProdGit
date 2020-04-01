using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSens;
    public Transform Target;
    
    public float distanceFromPlayer = 2;

    public float rotationSmoothTime = 0.1f;
    Vector3 smoothVelocity;
    Vector3 currentCameraRotation;
  
    float mouseX, mouseY;
    float minYconstraint = -35f;
    float maxYconstraint = 60f;

    [HideInInspector] public bool IsActive = true;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        if (IsActive == true)
        {
            CameraControl();
        }
    }

    void CameraControl()    
    {
        mouseX += Input.GetAxis("Mouse X") * mouseSens;
        mouseY -= Input.GetAxis("Mouse Y") * mouseSens;
        mouseY = Mathf.Clamp(mouseY, minYconstraint, maxYconstraint);

        Vector3 targetRot = new Vector3(mouseY, mouseX);

        currentCameraRotation = Vector3.SmoothDamp(currentCameraRotation, targetRot, ref smoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentCameraRotation;
        transform.position = Target.position - transform.forward * distanceFromPlayer;

        



    }
}
