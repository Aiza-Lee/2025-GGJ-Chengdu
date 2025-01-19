using UnityEngine.Events;

namespace NSFrame
{
	public class MonoService : MonoSingleton<MonoService> {

		private readonly UnityEvent Updating = new();
		private readonly UnityEvent LateUPdating = new();
		private readonly UnityEvent FiexdUpdating = new();

		public void AddUpdateListener(UnityAction action) {
			Updating.AddListener(action);
		}
		public void RemoveUpdateListener(UnityAction action) {
			Updating.RemoveListener(action);
		}
		public void AddLateUpdateListener(UnityAction action) {
			LateUPdating.AddListener(action);
		}
		public void RemoveLateUpdateListener(UnityAction action) {
			LateUPdating.RemoveListener(action);
		}
		public void AddFixedUpdateListener(UnityAction action) {
			FiexdUpdating.AddListener(action);
		}
		public void RemoveFixedUpdateListener(UnityAction action) {
			FiexdUpdating.RemoveListener(action);
		}

		private void Update() {
			Updating?.Invoke();
		}
		private void LateUpdate() {
			LateUPdating?.Invoke();
		}
		private void FixedUpdate() {
			FiexdUpdating?.Invoke();
		}
	}
}


