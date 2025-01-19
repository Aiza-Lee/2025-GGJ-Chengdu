using UnityEngine;

namespace NSFrame {
	public abstract class MonoSingleton<T> : MonoBehaviour 
	where T : MonoSingleton<T> {
		public static T Instance { get; set; }

		protected virtual void Awake() {
			if (Instance != null && Instance != this) {
				Destroy(gameObject);
				return ;
			}
			if (Instance == null) 
				Instance = this as T;
		}
	}
}