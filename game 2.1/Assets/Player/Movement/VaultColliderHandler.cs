using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaultColliderHandler : MonoBehaviour
{
    public bool colliding = false;
    public LayerMask layermask;
    public bool Colliding() 
    {
        return colliding;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (layermask == (layermask | (1 << other.gameObject.layer))) colliding = true;
    }
    private void OnTriggerStay(Collider other)
    {
        if (layermask == (layermask | (1 << other.gameObject.layer))) colliding = true;
    }
    private void OnTriggerExit(Collider other)
    {
        colliding = false;
    }
}
