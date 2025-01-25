using System.Xml;
using NSFrame;
using UnityEngine;
using Random = UnityEngine.Random;

namespace STD {
    public class Player : MonoBehaviour
    {
		[Header("挂载")]
		public GameController GameController;
		public GameObject BubblePrefab;

		public int BubbleCountOnDie = 10;

        private Rigidbody2D thisRigidBody;
		private Animator _animator;

        public IShotGun shotGun { get; private set; }

        public float MoveSpeed = 5.0f;
        public float JumpSpeed = 15.0f;
        public void ChangeGun(IShotGun gun)
        {
            shotGun = gun;
        }
        void Start()
        {
			_animator = GetComponent<Animator>();
			_animator.enabled = true;

			thisRigidBody = GetComponent<Rigidbody2D>();
            var doc = new XmlDocument();
            doc.LoadXml(Resources.Load("BubbleGunPara").ToString());
            shotGun =new BubbleGun(doc);

        }
        void FixedUpdate()
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
				_animator.SetBool("IsRunning", true);
                thisRigidBody.velocity = thisRigidBody.velocity + new Vector2(-thisRigidBody.velocity.x - MoveSpeed, 0);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
				_animator.SetBool("IsRunning", true);
                thisRigidBody.velocity = thisRigidBody.velocity + new Vector2(-thisRigidBody.velocity.x + MoveSpeed, 0);
            }
            
            if (Input.GetKey(KeyCode.Mouse0))
            {
				if(shotGun ==  null) return;
                shotGun.Shot(transform);
            }
            shotGun.Update();
        }
		private void Update() {
			if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                var hitLeft = Physics2D.Raycast(transform.position - new Vector3(0.45f, 0), new Vector2(0, -1f), 0.02f);
                var hitRight = Physics2D.Raycast(transform.position + new Vector3(0.45f,0), new Vector2(0, -1f), 0.02f);
                var hitMid = Physics2D.Raycast(transform.position, new Vector2(0, -1f), 0.02f);
                if (hitMid.collider != null|| hitLeft.collider != null || hitRight.collider != null)
                {
					_animator.SetTrigger("Jump");
                    thisRigidBody.velocity = thisRigidBody.velocity + new Vector2(0, JumpSpeed - thisRigidBody.velocity.y);
                }
            }
			if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow)) {
				_animator.SetBool("IsRunning", false);
				thisRigidBody.velocity = Vector2.zero;
			}
			if (Input.GetKeyDown(KeyCode.DownArrow)){
				GetComponent<ContactMono>().InteractW();
			}
			CheckDirection();
		}
		private void CheckDirection() {
			// 考虑使用 Sprite Render 组件的 Flip 实现
			if (Input.GetKey(KeyCode.LeftArrow) && transform.localScale.x < 0) {
				transform.localScale = new(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
			} 
			if (Input.GetKey(KeyCode.RightArrow) && transform.localScale.x > 0) {
				transform.localScale = new(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
			} 
		}

		public void Die() {
			for (int i = 0; i < BubbleCountOnDie; ++i) {
				var bubble = GameObject.Instantiate(BubblePrefab, transform.position + new Vector3(0f, 1f), new());
				var rb = bubble.GetComponent<Rigidbody2D>();
				Vector2 randomDirection = Random.insideUnitCircle.normalized;
				float speed = Random.Range(0f, 1f);
				rb.AddForce(randomDirection * speed, ForceMode2D.Impulse);
			}
			gameObject.SetActive(false);
			GameController.FailGame();
		}
    }
}

