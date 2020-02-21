using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrounCheck : MonoBehaviour
{
    private CharacterController player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        #region GroundCheck
        RaycastHit hit;
        // draw raycast going down       
        Debug.DrawRay(this.transform.position, -Vector3.up, Color.red, player.distanceToGround);

        if (Physics.Raycast(transform.position, -Vector3.up, out hit, player.distanceToGround, player.walkable))
        {
            //print("HIT SOMETHING");
            player.playerGrounded = true;
        }
        else
        {
            player.playerGrounded = false;
        }
        #endregion
    }
}
