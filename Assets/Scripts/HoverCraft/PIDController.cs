﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PIDController
{
    //Our PID coefficients for tuning the controller
	public float pCoeff = .8f;
	public float iCoeff = .0002f;
	public float dCoeff = .2f;
	public float minimum = -1;
	public float maximum = 1;

	//Variables to store values between calculations
	float integral;
	float lastProportional;

	//Constructor
	public PIDController(PIDControllerSettings settings){
		this.pCoeff = settings.pCoeff;
		this.iCoeff = settings.iCoeff;
		this.dCoeff = settings.dCoeff;
		this.minimum = settings.minimum;
		this.maximum = settings.maximum;
	}

	//We pass in the value we want and the value we currently have, the code
	//returns a number that moves us towards our goal
	public float Seek(float seekValue, float currentValue)
	{
		float deltaTime = Time.fixedDeltaTime;
		float proportional = seekValue - currentValue;

		float derivative = (proportional - lastProportional) / deltaTime;
		integral += proportional * deltaTime;
		lastProportional = proportional;

		//This is the actual PID formula. This gives us the value that is returned
		float value = pCoeff * proportional + iCoeff * integral + dCoeff * derivative;
		value = Mathf.Clamp(value, minimum, maximum);

		return value;
	}
}
