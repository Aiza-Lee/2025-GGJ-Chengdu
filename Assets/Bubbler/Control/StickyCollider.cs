using UnityEngine;

public class StickyCollider : MonoBehaviour {

	private Transform _centerTrans;
	private BubbleController _bubbleController;
	private Vector2 _initDirection;

	private void Start() {
		_bubbleController = transform.parent.parent.GetComponent<BubbleController>();
		_centerTrans = transform.parent.parent.GetChild(0).transform;
	}

	private void OnTriggerEnter2D(Collider2D other) {
		_initDirection = (transform.position - _centerTrans.position).normalized;
		_bubbleController.DetectCount += 1;
		_bubbleController.StickForce += _initDirection;
	}
	private void OnTriggerExit2D(Collider2D other) {
		_bubbleController.DetectCount -= 1;
		_bubbleController.StickForce -= _initDirection;
	}
}
