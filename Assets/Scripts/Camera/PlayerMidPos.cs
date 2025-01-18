using UnityEngine;

public class PlayerMidPos : MonoBehaviour {

	[Header("挂载")]
	public Transform BubblerTransform;
	public Transform HumanTransform;
    void Update() {
        transform.position = (BubblerTransform.position + HumanTransform.position) / 2;
    }
}
