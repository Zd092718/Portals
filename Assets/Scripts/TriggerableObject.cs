using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerableObject : MonoBehaviour
{
    public virtual void OnTrigger() { }
    public virtual void OnUnTrigger() { }
}
