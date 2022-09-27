using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Portal")]
    public GameObject portalPrefab;

    [Header("Grabbable Objects")]
    public float grabDistance = 3f;
    public float throwingForce = 300f;

    public Action onCollectOrb;

    private GrabbableObject grabbableObject;
    private Camera playerCamera;
    private List<Portal> portals;
    private bool shouldUseFirstPortal = true;


    // Start is called before the first frame update
    void Start()
    {
        playerCamera = transform.GetComponentInChildren<Camera>();
        portals = new List<Portal>();
    }

    // Update is called once per frame
    void Update()
    {
        // Clicking logic for grabbing objects
        // and shooting portals.
        bool interactedWithObject = false;
        if (Input.GetMouseButtonDown(0))
        {
            // If holding grabbable object, release it
            if (grabbableObject != null)
            {
                Release();
                interactedWithObject = true;
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
                            interactedWithObject = true;
                        }
                    }
                }
            }


        }

        //Logic for spawning portals
        if (interactedWithObject == false && Input.GetMouseButtonDown(0))
        {
            //Perform the raycast.
            RaycastHit hit;
            if (Physics.Raycast(transform.position, playerCamera.transform.forward, out hit))
            {
                if (hit.transform.GetComponent<PortalArea>() != null)
                {
                    SpawnPortal(hit.point, hit.transform.GetComponent<PortalArea>());
                }
            }

        }

        //Logic for holding grabbable object
        if (grabbableObject != null)
        {
            grabbableObject.transform.position = playerCamera.transform.position + playerCamera.transform.forward * grabDistance;
        }
    }

    private void SpawnPortal(Vector3 spawnPoint, PortalArea portalArea)
    {
        Portal currentPortal;

        if (portals.Count < 2)
        {
            GameObject portalObject = Instantiate(portalPrefab);
            currentPortal = portalObject.GetComponent<Portal>();
            portals.Add(currentPortal);
        }
        else
        {
            currentPortal = portals[shouldUseFirstPortal ? 1 : 0];
            shouldUseFirstPortal = !shouldUseFirstPortal;
        }
        currentPortal.transform.position = spawnPoint;
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
