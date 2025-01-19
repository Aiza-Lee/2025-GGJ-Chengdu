using System.Collections.Generic;
using UnityEngine;

namespace NSFrame 
{
	public class NSFrameRoot : MonoSingleton<NSFrameRoot> {
		public List<ConfigBase> ConfigBases;
		public static Transform FrameRootTransform { get; private set; }
		public static bool Initialized = false;
		protected override void Awake() {
			base.Awake();
			FrameRootTransform = transform;
			DontDestroyOnLoad(gameObject);
			Initialized = true;
		}
		private void OnDestroy() {
			Initialized = false;
		}

		public T GetConfig<T>() where T : ConfigBase {
			foreach (ConfigBase config in ConfigBases) 
				if (config is T t) return t;
			Debug.LogError($"NS: Can't find configuration named \"{typeof(T)}\".");
			return null;
		}
	}
}