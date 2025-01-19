using System.Xml;
using UnityEngine;

namespace STD {
    public class Player : MonoBehaviour
    {
		[Header("挂载")]
		public GameController GameController;

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
            else if (Input.GetKey(KeyCode.RightArrow))
            {
				_animator.SetBool("IsRunning", true);
                thisRigidBody.velocity = thisRigidBody.velocity + new Vector2(-thisRigidBody.velocity.x + MoveSpeed, 0);
            }
            else
            {
				_animator.SetBool("IsRunning", false);
                thisRigidBody.velocity = thisRigidBody.velocity + new Vector2(-thisRigidBody.velocity.x, 0);
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
			_animator.SetTrigger("Die");
			GameController.FailGame();
		}
		public void Restart() {
			_animator.SetTrigger("Restart");
		}
    }
}

