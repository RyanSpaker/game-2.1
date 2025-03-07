using UnityEngine;
using UnityEngine.InputSystem;

public class Controls : MonoBehaviour
{
    public Vector2 mouseDelta;
    public bool jump, left, right, sprint, crouch, boost, Debug;
    public Vector2 movement;
    void OnJump(InputValue val)
    {
        if (!jump && val.Get<float>() > 0) gameObject.SendMessage("Jump");
        if (jump && val.Get<float>() <= 0) gameObject.SendMessage("JumpReleased");
        jump = val.Get<float>() > 0;
    }
    void OnLeftMouse(InputValue val)
    {
        left = val.Get<float>() > 0;
    }
    void OnRightMouse(InputValue val)
    {
        right = val.Get<float>() > 0;
    }
    void OnSprint(InputValue val)
    {
        if (!sprint && val.Get<float>() > 0) gameObject.SendMessage("Sprint");
        if (sprint && val.Get<float>() <= 0) gameObject.SendMessage("SprintReleased");
        sprint = val.Get<float>() > 0;
    }
    void OnCrouch(InputValue val)
    {
        if (!crouch && val.Get<float>() > 0) gameObject.SendMessage("Crouch");
        if (crouch && val.Get<float>() <= 0) gameObject.SendMessage("CrouchReleased");
        crouch = val.Get<float>() > 0;
    }
    void OnMouse(InputValue val)
    {
        mouseDelta = val.Get<Vector2>();
    }
    void OnMovement(InputValue val)
    {
        movement = val.Get<Vector2>();
    }
    void OnBoost(InputValue val)
    {
        if (!boost && val.Get<float>() > 0) gameObject.SendMessage("Boosted");
        if (boost && val.Get<float>() <= 0) gameObject.SendMessage("BoostReleased");
        boost = val.Get<float>() > 0;
    }
    void OnDebug(InputValue val) 
    {
        Debug = val.Get<float>() > 0;
    }
}
