using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCasting : MonoBehaviour
{
    public static float distanceFromTarget;
    public float toTarget;
    public static GameObject hitTarget;

    // Update is called once per frame
    void Update()
    {

        RaycastHit hit;
        Debug.DrawRay(Camera.main.transform.position,
              Camera.main.transform.forward * 5f,
              Color.red);

        //if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))
        int mask = LayerMask.GetMask("UI");
        if (Physics.Raycast(Camera.main.transform.position,
                            Camera.main.transform.forward,
                            out hit, 1f, mask))
        {
            Debug.Log("Ray hit: " + hit.collider.name);
            //toTarget = hit.distance;
            distanceFromTarget = toTarget;
            hitTarget = hit.collider.gameObject;
        }
        else
        {
            distanceFromTarget = Mathf.Infinity;
            hitTarget = null;
        }
        
    }
}
