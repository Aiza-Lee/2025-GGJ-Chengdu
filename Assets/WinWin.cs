using UnityEngine;
using UnityEngine.SceneManagement;

public class WinWin : MonoBehaviour {
	public string Scenename;
	private int _cnt;
	private void OnTriggerEnter2D(Collider2D other) {
		_cnt++;
	}
	private void OnTriggerExit2D(Collider2D other) {
		_cnt--;
	}
	private void Update() {
		if (_cnt == 2) {
			SceneManager.LoadScene(Scenename);
		}
	}
}
