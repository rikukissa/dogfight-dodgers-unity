using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerBehaviour : MonoBehaviour {

	Vector3 position;
	public GameObject boundaries;
	private Bounds bounds;
	public float throttle = 0;
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

	public float doubleTapInterval = 0.3f;

	private int timesTapped = 0;
	private bool wasTapped = false;

	private bool upsideDown = false;
	private float flippingVelocity;
	public float flippingTime;

	private System.DateTime startTime;

	private Rigidbody2D body;
	float Distance(float alpha, float beta) {
		float phi = Mathf.Abs(beta - alpha) % radian;
		float distance = phi > radian / 2 ? radian - phi : phi;
		return distance;
	}

	float Mod(float x, float m) {
		return (x % m + m) % m;
	}
	void FixedUpdate () {
		// Debug.Log(Application.isEditor);
		if(ghost) {
			return;
		}
		DoUpdate(Time.deltaTime);
	}
	private bool isDoubleTapped() {
		if(isTouching()) {
      if(timesTapped == 0 && doubleTapInterval < 0) {
				timesTapped = 1;
        doubleTapInterval = 0.3f;
			} else if(!wasTapped && timesTapped == 1 && doubleTapInterval > 0) {
				timesTapped = 0;
				doubleTapInterval = 0.3f;
				return true;
			}
		} else if(doubleTapInterval < 0) {
			timesTapped = 0;
		}

		doubleTapInterval -= 1 * Time.deltaTime;
		wasTapped = isTouching();
		return false;
	}

	private bool isMobile() {
		return Application.platform == RuntimePlatform.IPhonePlayer && Input.touchCount > 0;
	}
	private bool isTouching() {
		if(isMobile()) {
			return Input.touchCount > 0;
		}

		return Input.GetButton("Throttle");
	}
	private Vector2 getTouchPosition() {
		if(isMobile()) {
			return Input.GetTouch(0).position;
		}
		return new Vector2(0, 0);
	}
	void Start() {
		startTime = System.DateTime.Now;
		bounds = boundaries.GetComponent<SpriteRenderer>().bounds;
		body = GetComponent<Rigidbody2D>();
	}
	void SaveScore() {
		int total = (int)(System.DateTime.Now - startTime).TotalMilliseconds;
		int current = PlayerPrefs.GetInt("record", total);

		if(current >= total) {
			PlayerPrefs.SetInt("record", total);
		}
	}
	void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.tag == "Finish") {
			this.SaveScore();
			this.StartNewRound();
		}
	}
	void StartNewRound() {
		Scene scene = SceneManager.GetActiveScene();
		SceneManager.LoadScene(scene.name);
	}
	void CollidedWithGround(Collision2D collision) {
		if(Mathf.Abs(collision.relativeVelocity.x) > 5 || Mathf.Abs(collision.relativeVelocity.y) > 5) {
			this.StartNewRound();
		}

	}
	void OnCollisionEnter2D(Collision2D collision) {
		if(collision.gameObject.tag == "Ground") {
			this.CollidedWithGround(collision);
		}
	}
	bool InBounds(Vector3 position) {
		return position.x > bounds.min.x &&
			position.x < bounds.max.x &&
			position.y > bounds.min.y &&
			position.y < bounds.max.y;
	}

	public void DoUpdate (float delta) {

		tick++;


		bool touching = isTouching();

		float vx = body.velocity.x;
		float vy = body.velocity.y;
		position = body.position;

		if(!this.InBounds(position)) {
			this.StartNewRound();
			return;
		}

		Vector2 velocity = body.velocity;

		if(isDoubleTapped()) {
			upsideDown = !upsideDown;
		}

		if(
			(Application.isPlaying && touching) ||
			(!Application.isPlaying && tick < 2320)
		) {
			throttle = 1;
		} else {
			throttle = 0;
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
		vy = Mathf.Cos(radAngle) * speed * delta * -1;

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

		// Gravity
		vy -= gravity * delta;

		// position.x += vx * delta;
		// position.y -= vy * delta;

		velocity.x = vx;
		velocity.y = vy;


		// Flip animation
		Vector3 foo = transform.localScale;
		foo.y = upsideDown ?
			Mathf.SmoothDamp(foo.y, -1, ref flippingVelocity, flippingTime) :
			Mathf.SmoothDamp(foo.y, 1, ref flippingVelocity, flippingTime);

		transform.localScale = foo;
		body.velocity = velocity;
		body.position = position;
		body.rotation = angle;
		transform.eulerAngles = new Vector3(0, 0, angle);
	}
}
