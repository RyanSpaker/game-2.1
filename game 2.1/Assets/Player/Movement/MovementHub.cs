using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHub : MonoBehaviour
{
    public Walk walkModule;
    public GroundChecker groundCheck;
    public Controls input;
    public Crouch crouchModule;
    public Jump jumpModule;
    public Gravity gravityHandler;
    public Vault vaultModule;
    private int gameState = 0;
    public Rigidbody physics;
    private bool crouching, grounded, sprinting, jumping;
    void Setup() 
    {
        physics = gameObject.GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        grounded = groundCheck.checkGrounded();
        switch (gameState) 
        {
            case 0:
                physics.velocity = walkModule.move(physics.velocity, input.movement, transform.forward, transform.right, crouching, grounded, sprinting);
                physics.position = crouchModule.crouch(grounded, physics.position);
                gravityHandler.updateGrav(jumping, physics.velocity.y, crouching, grounded);
                Debug.Log(physics.velocity.y + "    " + physics.position.y);
                break;
        }
    }
    void Crouch() 
    {
        if (grounded)
        {
            crouchModule.setState(1);
            crouchModule.setSafety();
            crouching = true;
        }
        else
        {
            crouching = true;
            crouchModule.setState(4);
        }
    }
    void CrouchReleased()
    {
        if (grounded)
        {
            crouchModule.setState(3);
            crouching = false;
        }
        else
        {
            crouchModule.setState(6);
            crouching = false;
        }
    }
    void Sprint() 
    {
        sprinting = true;
    }
    void SprintReleased() 
    {
        sprinting = false;
    }
    void Jump() 
    {
        if (grounded || crouchModule.getSafety()) { physics.velocity = jumpModule.jump(physics.velocity); crouchModule.stopSafety(); }
        else 
        {
            if (vaultModule.canVault()) { physics.velocity = vaultModule.vault(physics.velocity, grounded, jumpModule.getVel()); }
        }
        jumping = true;
    }
    void JumpReleased() 
    {
        jumping = false;
    }
}
