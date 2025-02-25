using UnityEngine;

public class FootCollider : MonoBehaviour {
    BubbleController _bubbleController;
	private void Start() {
		_bubbleController = transform.parent.GetComponent<BubbleController>();
	}
	private void OnTriggerEnter2D(Collider2D other) {
		_bubbleController.OnGroundCount += 1;
	}
	private void OnTriggerExit2D(Collider2D other) {
		_bubbleController.OnGroundCount -= 1;
	}
}
