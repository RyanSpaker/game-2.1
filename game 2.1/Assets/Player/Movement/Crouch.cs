using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch : MonoBehaviour
{
	public Transform playerTrans;
	public Gravity gravityHandler;
	public float crouchHeight;
	public float crouchTime;
	public float crouchGravityFactor;
	public float modelHeight = 2;
	private float scaleStep, originalHeight, scaleConvert, finalScale, originalScale;
	private bool safety = false;
	private int crouchState = 0;
    private void Start()
    {
		originalScale = playerTrans.localScale.y;
		originalHeight = originalScale * modelHeight;
		finalScale = crouchHeight / modelHeight;
		scaleStep = (finalScale - originalScale) / crouchTime;
		scaleConvert = originalHeight;
		gravityHandler.setCrouch(crouchGravityFactor);
	}
	public void setSafety() 
	{
		safety = true;
	}
	public void stopSafety() 
	{
		safety = false;
	}
	public bool getSafety() 
	{
		return safety;
	}
	public void setState(int state) 
	{
		crouchState = state;
	}
    public Vector3 crouch(bool grounded, Vector3 position) 
    {
		if (crouchState == 0 || crouchState == 2 || crouchState == 3 || crouchState == 5 || crouchState == 6) safety = false;
		float scale = playerTrans.localScale.y;
		float pos = position.y;
		float Sstep = scaleStep * Time.fixedDeltaTime;
		float Hstep = Sstep * scaleConvert /2f;
		switch (crouchState)
		{
			case 1:
				scale += Sstep;
				pos += Hstep;
				if (scale*modelHeight <= crouchHeight)
				{
					pos += (scale - finalScale) * modelHeight / 2f;
					scale = finalScale;
					crouchState = 2;
				}
				if (!grounded && !safety) crouchState = 4;
				break;
			case 3:
				scale -= Sstep;
				pos -= Hstep;
				if (scale * modelHeight >= originalHeight)
				{
					pos -= (scale - originalScale) * modelHeight / 2f;
					scale = originalScale;
					crouchState = 0;
				}
				if (!grounded) crouchState = 6;
				break;
			case 4:
				scale += Sstep;
				pos -= Hstep;
				if (scale * modelHeight <= crouchHeight)
				{
					pos -= (scale - finalScale) * modelHeight / 2f;
					scale = finalScale;
					crouchState = 5;
				}
				if (grounded) crouchState = 1;
				break;
			case 6:
				scale -= Sstep;
				pos += Hstep;
				if (scale * modelHeight >= originalHeight)
				{
					pos += (scale - originalScale) * modelHeight / 2f;
					scale = originalScale;
					crouchState = 0;
				}
				if (grounded) crouchState = 3;
				break;
		}
		playerTrans.localScale = new Vector3(playerTrans.localScale.x, scale, playerTrans.localScale.z);
		return new Vector3(position.x, pos, position.z);
	}
}
