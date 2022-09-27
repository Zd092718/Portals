using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : TriggerableObject
{
    public GameObject model;
    public float speed = 3f;
    public float openedHeight;

    Vector3 targetPosition;
    // Start is called before the first frame update
    void Start()
    {
        targetPosition = Vector3.zero;

    }

    // Update is called once per frame
    void Update()
    {
        model.transform.localPosition = Vector3.Lerp(model.transform.localPosition, targetPosition, Time.deltaTime * speed);
    }

    public override void OnTrigger()
    {
        base.OnTrigger();

        targetPosition = new Vector3(0, openedHeight, 0);
    }

    public override void OnUnTrigger()
    {
        base.OnUnTrigger();

        targetPosition = Vector3.zero;
    }
}
