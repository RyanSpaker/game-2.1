using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Jump : MonoBehaviour
{
    public Gravity gravityHandler;
    public Vault vault;
    public float jumpHeight;
    public float upTime;
    public float jumpTime;
    public float standardGravPercent;
    public float jumpVelocity;
    private void Start()
    {
        jumpHeight += .01f;
        float upGrav = (2f * jumpHeight) / (Time.fixedDeltaTime * upTime - upTime*upTime);
        jumpVelocity = (-2f*jumpHeight)/(Time.fixedDeltaTime-upTime);
        float downTime = jumpTime - upTime;
        float downGrav = (2f * jumpHeight) / (Time.fixedDeltaTime * downTime - downTime * downTime);
        float standardGrav = downGrav * standardGravPercent;
        gravityHandler.setGravs(upGrav, downGrav, standardGrav);
        vault.setGravity(upGrav);
    }
    public Vector3 jump(Vector3 vel) 
    {
        return new Vector3(vel.x, jumpVelocity, vel.z);
    }
    public float getVel() 
    {
        return jumpVelocity;
    }
}
