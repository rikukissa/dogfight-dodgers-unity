﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour {

	Vector3 position;
	public float throttle = 0;
	public float vx = 0;
	public float vy = 0;

	public float angle = 0;
	public float speed = 0;
	public float airResistance = 0.995f;
	public float acceleration = 10f;
	public float maxSpeed = 5f;
	public float gravity = 0.5f;

	public float angleChangeSpeed = 0;
	private float altitudeMinLimitVelocity = 0;
	public float altitudeLimitSmoothTime;
	public float altitudeLimitMaxSpeed;

	public bool ghost = false;

	private float radian = Mathf.PI * 2;
	public int tick = 0;

	public float doubleTapInterval = 0.5f;

	private int timesTapped = 0;
	private bool wasTapped = false;

	private bool upsideDown = false;
	private float flippingVelocity;
	public float flippingTime;
	float Distance(float alpha, float beta) {
		float phi = Mathf.Abs(beta - alpha) % radian;
		float distance = phi > radian / 2 ? radian - phi : phi;
		return distance;
	}

	float Mod(float x, float m) {
		return (x % m + m) % m;
	}
	// Use this for initialization
	void Start () {

	}
	void FixedUpdate () {
		// Debug.Log(Application.isEditor);
		if(ghost) {
			return;
		}
		DoUpdate(Time.deltaTime);
	}
	private bool isDoubleTapped() {
				// Double tap handling
		if(Input.GetButton("Throttle")) {
      if(timesTapped == 0) {
				timesTapped = 1;
        doubleTapInterval = 0.5f;
			} else if(!wasTapped && timesTapped == 1 && doubleTapInterval > 0) {
				timesTapped = 0;
				return true;
			}
		} else if(doubleTapInterval < 0) {
			timesTapped = 0;
		}

		doubleTapInterval -= 1 * Time.deltaTime;
		wasTapped = Input.GetButton("Throttle");
		return false;
	}
	// Update is called once per frame
	public void DoUpdate (float delta) {
		tick++;

		position = transform.position;

		if(isDoubleTapped()) {
			upsideDown = !upsideDown;
		}

		if(
			(Application.isPlaying && Input.GetButton("Throttle")) ||
			(!Application.isPlaying && tick < 2320)
		) {
			throttle = 1;
		} else {
			throttle = 0;
		}

		if(position.y <= 0) {
			vy = 0;
			position.y = 0;
		}

		float radAngle = (angle + 90) * Mathf.Deg2Rad;

		speed += throttle * acceleration;
		speed = Mathf.Min(maxSpeed, speed);

		// Air resistance
		float ratio = 1 / (1 + (delta * airResistance));
		vx *= ratio;
		vy *= ratio;

		// Directional forces
		vx = Mathf.Sin(radAngle) * speed * delta;
		vy = Mathf.Cos(radAngle) * speed * delta;


		angle = Mod(angle, 360);

		bool shouldIncreaseAngle = speed > 300 && throttle == 1;

		if(shouldIncreaseAngle) {
			int direction = upsideDown ? -1 : 1;
			angle += (angleChangeSpeed * (speed / maxSpeed)) * direction * delta;
		}

		if(position.y > 0.1 && throttle == 0) {
			if(angle > 90 && angle < 270) {
				angle = Mathf.SmoothDampAngle(angle, 180, ref altitudeMinLimitVelocity, altitudeLimitSmoothTime, altitudeLimitMaxSpeed, delta);
			} else {
				angle = Mathf.SmoothDampAngle(angle, 270, ref altitudeMinLimitVelocity, altitudeLimitSmoothTime, angle / 90 * 20, delta);
			}
		}

		// gravity
		vy += gravity * delta;

		position.x += vx * delta;
		position.y -= vy * delta;

		Vector3 foo = transform.localScale;
		foo.y = upsideDown ?
			Mathf.SmoothDamp(foo.y, -1, ref flippingVelocity, flippingTime) :
			Mathf.SmoothDamp(foo.y, 1, ref flippingVelocity, flippingTime);

		transform.localScale = foo;
		transform.position = position;
		transform.eulerAngles = new Vector3(0, 0, angle);
	}
}