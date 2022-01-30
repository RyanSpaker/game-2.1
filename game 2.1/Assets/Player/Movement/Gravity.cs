using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public ConstantForce gravity;
    private float upGrav, downGrav, standardGrav, crouch;
    public void updateGrav(bool jumping, float yVel, bool crouching, bool grounded, bool safety) 
    {
        float grav = standardGrav;
        if (grounded || safety || (yVel > 0 && jumping)) grav = upGrav;
        if (!grounded && !safety && yVel < 0) grav = downGrav;
        if (crouching && !grounded && !safety) grav *= crouch;

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
    public float getGrav(bool jumping, float yVel, bool crouching, bool grounded, bool safety) 
    {
        float grav = standardGrav;
        if (grounded || safety || yVel > 0) grav = upGrav;
        if (!grounded && !safety && yVel < 0) grav = downGrav;
        if (crouching && !grounded && !safety) grav *= crouch;

        return grav;
    }
}
