using STD;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	[Header("挂载")]
	public Transform BubblerTransform;
	public Transform HumanTransform;
	public GameObject GameOverCanvas;

	public bool IsFinalStage = false;

	private Vector3 _bubblerPos;
	private Vector3 _humanPos;
	private BubbleController _bubbleController;
	private Player _playerController;

	private void Start() {
		_bubbleController = BubblerTransform.gameObject.GetComponent<BubbleController>();
		_playerController = HumanTransform.gameObject.GetComponent<Player>();

		GameOverCanvas.SetActive(false);

		_bubblerPos = BubblerTransform.position;
		_humanPos = HumanTransform.position;
	}

    void Update() {
        transform.position = (BubblerTransform.position + HumanTransform.position) / 2;
		if (Input.GetKeyDown(KeyCode.P)) {
			ResetGame();
		}
	}

	public void FailGame() {
		GameOverCanvas.SetActive(true);
	}

	public void ResetGame() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
