using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrigger : MonoBehaviour {
	private BubbleController _bubbleController;
	private void Start() {
		_bubbleController = transform.parent.parent.GetComponent<BubbleController>();
	}
	private void OnTriggerEnter2D(Collider2D other) {
		var bullet = other.gameObject.GetComponent<STD.BubbleBullet>();
		if (bullet == null) {
			Debug.LogError("Not a bubble.");
			return;
		}
		_bubbleController.BubbleSize += _bubbleController.WeightGainPerBubble;
		bullet.DestroySelf();
	}
}
