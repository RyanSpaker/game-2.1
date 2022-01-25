using UnityEngine;
using System.Collections.Generic;
using System;

public class GroundChecker : MonoBehaviour
{
    public LayerMask whatIsGround;
    public float maxGroundAngle;
    public float minWallAngle;
    public bool walled;
    public bool grounded;
    public Rigidbody physics;
    public Vector3 normalVector;
    public List<GameObject> walls = new List<GameObject>();
    public List<GameObject> grounds = new List<GameObject>();
    private bool IsFloor(Vector3 v)
    {
        return Vector3.Angle(transform.up, v) <= maxGroundAngle;
    }
    private bool IsWall(Vector3 v)
    {
        return Vector3.Angle(transform.up, v) >= minWallAngle;
    }
    private void OnCollisionEnter(Collision other)
    {
        int layer = other.gameObject.layer;
        if (whatIsGround.value != (whatIsGround.value | (1 << layer))) return;
        Vector3 Normal;
        bool tempG = false;
        bool tempW = false;
        foreach (ContactPoint contact in other.contacts)
        {
            Normal = contact.normal;
            if (IsFloor(Normal)) tempG = true;
            if (IsWall(Normal)) tempW = true;
        }
        if(tempG) grounds.Add(other.gameObject);
        if(tempW) walls.Add(other.gameObject);
    }
    private void OnCollisionStay(Collision other) 
    {
        int layer = other.gameObject.layer;
        if (whatIsGround.value != (whatIsGround.value | (1 << layer))) return;
        Vector3 Normal;
        bool tempG = false;
        bool tempW = false;
        foreach (ContactPoint contact in other.contacts)
        {
            Normal = contact.normal;
            if (IsFloor(Normal)) tempG = true;
            if (IsWall(Normal)) tempW = true;
        }
        if (tempG && !grounds.Contains(other.gameObject)) grounds.Add(other.gameObject);
        if (tempW && !walls.Contains(other.gameObject)) walls.Add(other.gameObject);
        if (!tempG) grounds.Remove(other.gameObject);
        if (!tempW) walls.Remove(other.gameObject);
    }
    private void OnCollisionExit(Collision other)
    {
        int layer = other.gameObject.layer;
        if (whatIsGround.value != (whatIsGround.value | (1 << layer))) return;
        grounds.Remove(other.gameObject);
        walls.Remove(other.gameObject);
    }
    public bool checkGrounded() 
    {
        grounded = grounds.Count> 0;
        return grounds.Count > 0;
    }
    public bool checkWalled()
    {
        walled = walls.Count > 0;
        return walls.Count > 0;
    }
}
