using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : MonoBehaviour
{
	public float walkSpeed;
	public float accTime;
	public float sprintFactor;
	public float crouchSpeedFactor;
	public float airAccFactor;
	public float crouchAirAccFactor;
	public float speedAccBlockSize;
	private float momentum = 0;
	private float momentumStep = 0;
	private float sprintSpeed;
	private float walkAcc;
	private float sprintAcc;
	
    void Start()
    {
		walkAcc = walkSpeed / accTime;
		sprintAcc = sprintFactor * walkSpeed / accTime;
		sprintSpeed = sprintFactor * walkSpeed;
		setMomentum(sprintSpeed);
	}
    private Vector2 getVec2(Vector3 vec) 
	{
		return new Vector2(vec.x, vec.z);
	}
	private void setMomentum(float speed) 
	{
		momentum = speed;
		momentumStep = speed / 90f;
	}
	public Vector3 move(Vector3 vel, Vector2 moveVec, Vector3 forward, Vector3 right, bool crouching, bool grounded, bool sprinting) 
    {
		//define variables in 2 dimension space
		Vector2 curVel = getVec2(vel);
		moveVec = getVec2(right * moveVec.x + forward * moveVec.y).normalized;
		//get our maximum speed if moving toward moveVec
		float maxSpeed = walkSpeed;
		if (crouching && grounded) maxSpeed *= crouchSpeedFactor;
		if (sprinting) maxSpeed *= sprintFactor;
		//if we are sprinting, we need to check our momentum. our momentum decreases by a constant factor based on  our change in movement angle.
		if (sprinting && !crouching) maxSpeed = Mathf.Max(curVel.magnitude - Vector2.Angle(moveVec, curVel) * momentumStep, sprintSpeed);
		//using our new maximum speed, calculate our step vector
		moveVec *= maxSpeed;
		Vector2 step = (moveVec - curVel);
		Vector2 curStep = step.normalized;
		//find out our acceleration to be used
		float acceleration = walkAcc;
		if (sprinting && !crouching) acceleration = sprintAcc;
		if (!grounded) acceleration *= airAccFactor;
		if (crouching && !grounded) acceleration *= crouchAirAccFactor;
		//if we are going real fast, our acceleration increases based on blocks
		if (curVel.magnitude > sprintSpeed && grounded)
		{
			acceleration *= (int)((curVel.magnitude - sprintSpeed) / speedAccBlockSize) + 1;
		}
		//using our acceleration, change our velocity
		curStep *= acceleration * Time.fixedDeltaTime;
		Vector3 returning = new Vector3();
		if (curStep.magnitude > step.magnitude)
		{
			returning = new Vector3(moveVec.x, vel.y, moveVec.y);
		}
		else
		{
			returning = new Vector3(vel.x + curStep.x, vel.y, vel.z + curStep.y);
		}
		//if we have exhausted all of our momentum, set momentum to sprint speed.
		if (getVec2(returning).magnitude <= sprintSpeed) setMomentum(sprintSpeed);
		return returning;
    }
}
