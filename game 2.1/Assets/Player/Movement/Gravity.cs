using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public ConstantForce gravity;
    private float upGrav, downGrav, standardGrav, crouch;
    public void updateGrav(bool jumping, float yVel, bool crouching, bool grounded) 
    {
        float grav = standardGrav;
        if (jumping) if (yVel >= 0) grav = upGrav; else grav = downGrav;
        if (crouching) grav *= crouch;
        gravity.force = new Vector3(0, grav, 0);
    }
    public void setGravs(float up, float down, float standard) 
    {
        upGrav = up;
        downGrav = down;
        standardGrav = standard;
    }
    public void setCrouch(float crouchFactor) 
    {
        crouch = crouchFactor;
    }
}
