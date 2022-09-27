using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureButton : MonoBehaviour
{
    public TriggerableObject targetObject;
    public GameObject model;
    public float pressedHeight;
    public float pressedDuration = 1f;
    public float pressedSpeed = 2f;


    Vector3 targetPosition;
    float pressedTimer;
    bool isPressed;

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        //Hold button down for few seconds
        pressedTimer -= Time.deltaTime;
        if (pressedTimer > 0)
        {
            targetPosition = new Vector3(0, pressedHeight, 0);

            if (isPressed == false)
            {
                isPressed = true;
                OnPress();
            }
        }
        else
        {
            targetPosition = Vector3.zero;
            if (isPressed == true)
            {
                isPressed = false;
                OnDepress();
            }
        }

        //perform movement of button model
        model.transform.localPosition = Vector3.Lerp(model.transform.localPosition, targetPosition, Time.deltaTime * pressedSpeed);
    }

    private void OnDepress()
    {
        model.GetComponentInChildren<Renderer>().material.SetColor("_Color", Color.red);
        targetObject.OnUnTrigger();
    }

    private void OnPress()
    {
        model.GetComponentInChildren<Renderer>().material.SetColor("_Color", Color.green);
        targetObject.OnTrigger();
    }

    private void OnTriggerStay(Collider other)
    {
        //only player and grabbable objects should press button.
        if (other.GetComponent<Player>() != null || other.GetComponent<GrabbableObject>() != null)
        {
            pressedTimer = pressedDuration;
        }
    }
}
