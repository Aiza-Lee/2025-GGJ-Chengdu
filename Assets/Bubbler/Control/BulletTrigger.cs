using UnityEngine;

public class BulletTrigger : MonoBehaviour {
	private BubbleController _bubbleController;
	private void Start() {
		_bubbleController = transform.parent.parent.GetComponent<BubbleController>();
	}
	private void OnTriggerEnter2D(Collider2D other) {
		var bullet = other.gameObject.GetComponent<STD.ContactMono>();
		if (bullet == null) {
			return;
		}
		if (bullet.ContactType == STD.ContactType.BubbleBullet) {
			_bubbleController.BubbleSize += _bubbleController.WeightGainPerBubble;
			other.gameObject.GetComponent<STD.BubbleBullet>().DestroySelf();
		}
		
	}
}
