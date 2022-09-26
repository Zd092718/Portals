using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float grabDistance = 3f;
    public float throwingForce = 300f;

    public Action onCollectOrb;

    private GrabbableObject grabbableObject;
    private Camera playerCamera;


    // Start is called before the first frame update
    void Start()
    {
        playerCamera = transform.GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        // Clicking logic for grabbing objects
        // and shooting portals.
        if (Input.GetMouseButtonDown(0))
        {
            // If holding grabbable object, release it
            if (grabbableObject != null)
            {
                Release();
            }
            else
            {
                //Raycast for grabbable objects.
                RaycastHit hit;
                if (Physics.Raycast(transform.position, playerCamera.transform.forward, out hit, grabDistance))
                {
                    // Check if looking at grabbable object
                    if (hit.transform.GetComponent<GrabbableObject>() != null)
                    {
                        GrabbableObject targetObject = hit.transform.GetComponent<GrabbableObject>();
                        // Hold the object
                        if (grabbableObject == null)
                        {
                            Hold(targetObject);
                        }
                    }
                }
            }

        }

        //Logic for holding grabbable object
        if (grabbableObject != null)
        {
            grabbableObject.transform.position = playerCamera.transform.position + playerCamera.transform.forward * grabDistance;
        }
    }

    private void Hold(GrabbableObject targetObject)
    {
        grabbableObject = targetObject;
        grabbableObject.GetComponent<Collider>().enabled = false;
        grabbableObject.GetComponent<Rigidbody>().useGravity = false;
    }

    private void Release()
    {
        grabbableObject.GetComponent<Collider>().enabled = true;
        grabbableObject.GetComponent<Rigidbody>().useGravity = true;
        grabbableObject.GetComponent<Rigidbody>().AddForce(playerCamera.transform.forward * throwingForce);
        grabbableObject = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Orb>() != null)
        {
            if (onCollectOrb != null)
            {
                onCollectOrb();
            }
        }
    }
}
