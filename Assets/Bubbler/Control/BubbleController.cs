using System.Collections;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class BubbleController : MonoBehaviour {

	[Header("挂载")]
	public GameController GameController;
	public Transform Bubble;
	public Transform FloatingSizeInspect;
	public GameObject FootCollider;
	public Animator BubbleAnimator;
	public GameObject BoomArea;
	public GameObject BubblePrefab;

	[Space(20)]
	public float MoveSpeed;
	public float StickedSpeed;
	public float JumpStrength;
	public float BlowStrength;
	public float StickStrength;
	public float MaxVelocityOnGround;
	[Range(0, 10)] public float OriBubbleSize;
	[Range(0, 10)] public float FlyableMinSize;
	[Range(0, 1)] public float GroundGravityScale;
	[Range(0, 1)] public float FlyGravityScale;
	public float DecreaseSpeed;
	public AnimationCurve SizeCurve;
	public float WeightGainPerBubble;
	public float BubbleCountOnDie = 10;
	public float BoomForce;


	[Header("Debug Area")]
	public float CurVelocityX;
	public float CurVelocityY;

	public bool InAir;
	public bool IsSticked;

	[SerializeField] private float _bubbleSize;
	public float BubbleSize {
		get => _bubbleSize;
		set {
			if (value < 0 || value > 10) return;

			if (value > FlyableMinSize) {
				IsFlying = true;
				_rigidbody.gravityScale = FlyGravityScale;
			}
			else {
				IsFlying = false;
				_rigidbody.gravityScale = GroundGravityScale;
			}

			_bubbleSize = value;
			float scale = SizeCurve.Evaluate(value);
			Bubble.localScale = new(scale, scale, scale);
		}
	}
	[SerializeField] private bool _isFlying;
	public bool IsFlying {
		get => _isFlying;
		set {
			if (value == _isFlying) return;
			if (value && !InAir) {
				PreFlyMoveUp();
			}

			if (value) {
				FootCollider.SetActive(false);
				BubbleAnimator.SetTrigger("Fly");
				BubbleAnimator.SetBool("IsFlying", true);
			}
			else {
				FootCollider.SetActive(true);
				BubbleAnimator.SetBool("IsFlying", false);
			}
			
			_isFlying = value;
		}
	}
	[SerializeField] private int _detectCount;
	public int DetectCount {
		get => _detectCount;
		set {
			_detectCount = value;
			if (value == 0) {
				IsSticked = false;
			}
			else {
				IsSticked = true;
			}
		}
	}
	[SerializeField] protected int _onGroundCount;
	public int OnGroundCount {
		get => _onGroundCount;
		set {
			_onGroundCount = value;
			if (value == 0) InAir = true;
			else InAir = false;
		}
	}

	public Vector2 StickForce { get; set; }


	private Rigidbody2D _rigidbody;

    void Start() {
        _rigidbody = GetComponent<Rigidbody2D>();

		BubbleSize = OriBubbleSize;

		BubbleAnimator.enabled = true;

		float scale = SizeCurve.Evaluate(FlyableMinSize);
		FloatingSizeInspect.localScale = new(scale, scale, scale);
    }

	private void Update() {
		// test:
		CurVelocityX = _rigidbody.velocityX;
		CurVelocityY = _rigidbody.velocityY;
		// test:
		if (Input.GetKey(KeyCode.LeftShift)) {
			BubbleSize += Time.deltaTime * DecreaseSpeed * 10;
		}

		if (InAir || IsFlying) return;

		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))  {
			BubbleAnimator.SetBool("IsRunning", true);
			CheckDirection();
		}
		else {
			BubbleAnimator.SetBool("IsRunning", false);
		}

		/* Jump */
		if (Input.GetKeyDown(KeyCode.W)) {
			BubbleAnimator.SetTrigger("Jump");
			_rigidbody.AddForceY(JumpStrength, ForceMode2D.Impulse);
		}
	}

    void FixedUpdate() {
		UpdateMoveOnGround();
		UpdateBlow();
		UpdateShrink();
		UpdateStick();
    }

	private void UpdateMoveOnGround() {

		if (InAir || IsFlying) return;

		/* Groud Move Left Right */
		if (Input.GetKey(KeyCode.A))  {
			_rigidbody.AddForceX(-MoveSpeed, ForceMode2D.Force);
		}
		else {
			if (_rigidbody.velocityX < 0) _rigidbody.velocityX /= 1.1f;
		}
		if (Input.GetKey(KeyCode.D)) {
			_rigidbody.AddForceX(MoveSpeed, ForceMode2D.Force);
		}
		else {
			if (_rigidbody.velocityX > 0) _rigidbody.velocityX /= 1.1f;
		}

		// 最大速度限制
		if (_rigidbody.velocityX < -MaxVelocityOnGround) _rigidbody.velocityX = -MaxVelocityOnGround;
		if (_rigidbody.velocityX > MaxVelocityOnGround) _rigidbody.velocityX = MaxVelocityOnGround;
	}
	private void UpdateBlow() {
		if (!IsFlying || IsSticked) return;


		if (Input.GetKey(KeyCode.Q)) {
			_rigidbody.AddForceX(BlowStrength, ForceMode2D.Force);
			BubbleSize -= DecreaseSpeed / 30;
		}
		if (Input.GetKey(KeyCode.E)) {
			_rigidbody.AddForceX(-BlowStrength, ForceMode2D.Force);
			BubbleSize -= DecreaseSpeed / 30;
		}
		if (Input.GetKey(KeyCode.S)) {
			_rigidbody.AddForceY(BlowStrength, ForceMode2D.Force);
			BubbleSize -= DecreaseSpeed / 30;
		}
		if (Input.GetKey(KeyCode.W)) {
			_rigidbody.AddForceY(-BlowStrength, ForceMode2D.Force);
			BubbleSize -= DecreaseSpeed / 30;
		}

		if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E))  {
			if (Input.GetKey(KeyCode.Q) && transform.localScale.x > 0) {
				transform.localScale = new(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
			} 
			if (Input.GetKey(KeyCode.E) && transform.localScale.x < 0) {
				transform.localScale = new(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
			}
		}
	}
	private void UpdateShrink() {
		if (Input.GetKey(KeyCode.Tab)) {
			BubbleSize -= DecreaseSpeed / 30;
		}
	}
	private void UpdateStick() {
		if (!IsSticked || !IsFlying) return;
		_rigidbody.AddForce(StickForce.normalized * StickStrength, ForceMode2D.Force);
		if (Input.GetKey(KeyCode.A))  {
			_rigidbody.AddForceX(-StickedSpeed, ForceMode2D.Force);
		}
		if (Input.GetKey(KeyCode.D)) {
			_rigidbody.AddForceX(StickedSpeed, ForceMode2D.Force);
		}
		if (Input.GetKey(KeyCode.W)) {
			_rigidbody.AddForceY(StickedSpeed, ForceMode2D.Force);
		}
		if (Input.GetKey(KeyCode.S))  {
			_rigidbody.AddForceY(-StickedSpeed, ForceMode2D.Force);
		}
		CheckDirection();
	}

	private void PreFlyMoveUp() {
		// InAir = true;
		StartCoroutine(DoFly());
	}
	IEnumerator DoFly() {
		_rigidbody.velocityY = 0.5f;
		yield return new WaitForSeconds(0.15f);
		_rigidbody.velocityY = 0;
	}

	public void Die() {
		// Debug.Log("Bubble Die");
		BubbleAnimator.SetTrigger("Die");

		Vector3 centerPos = transform.GetChild(0).position;
		for (int i = 0; i < BubbleCountOnDie; ++i) {
			Vector2 randomDirection = Random.insideUnitCircle.normalized;
			GameObject.Instantiate(BubblePrefab, centerPos + (Vector3)randomDirection / 2, new());
		}

		StartCoroutine(WaitFor(0.1f, ()=> {
			BoomArea.SetActive(true);
		}));

		if (!GameController.IsFinalStage)
			GameController.FailGame();
	}
	private IEnumerator WaitFor(float time, Action action) {
		yield return new WaitForSecondsRealtime(time);
		action?.Invoke();
	}

  #region Utilities 
	private void CheckDirection() {
		// 考虑使用 Sprite Render 组件的 Flip 实现
		if (Input.GetKey(KeyCode.A) && transform.localScale.x > 0) {
			transform.localScale = new(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
		} 
		if (Input.GetKey(KeyCode.D) && transform.localScale.x < 0) {
			transform.localScale = new(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
		} 
	}
  #endregion
}
