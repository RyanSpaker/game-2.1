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
    public float upGrav = 0f;
    public float downGrav = 0f;
    private void Start()
    {
        upGrav = (2f * jumpHeight) / (Time.fixedDeltaTime * upTime - upTime*upTime);
        jumpVelocity = (-2f*jumpHeight)/(Time.fixedDeltaTime-upTime);
        float downTime = jumpTime - upTime;
        downGrav = (2f * jumpHeight) / (Time.fixedDeltaTime * downTime - downTime * downTime);
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
