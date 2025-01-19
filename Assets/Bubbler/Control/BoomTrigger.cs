using System.Linq;
using UnityEngine;

public class BoomTrigger : MonoBehaviour {
	private BubbleController _bubbleController;
	private Transform _center;
	private void Start() {
		_bubbleController = transform.parent.GetComponent<BubbleController>();
		_center = transform.parent.GetChild(0);
	}
	private void OnTriggerStay2D(Collider2D other) {
		var rb = other.attachedRigidbody;
		if (rb == null) return;
		var direction = other.gameObject.transform.position - _center.position;
		direction = direction.normalized;
		rb.AddForce(direction * _bubbleController.BoomForce, ForceMode2D.Impulse);
		_bubbleController.gameObject.SetActive(false);
	}
	// private void OnTriggerEnter2D(Collider2D other) {
		// Debug.Log(other);
		
	// }
	// private void OnTriggerExit2D(Collider2D other) {
	// 	// Debug.Log(other);
		
	// }
}