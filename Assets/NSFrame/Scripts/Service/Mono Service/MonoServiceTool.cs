using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace NSFrame {
	public static class MonoServiceTool {
		public static void NS_AddUpdListener(UnityAction action) {
			MonoService.Instance.AddUpdateListener(action);
		}
		public static void NS_RemoveUpdListener(UnityAction action) {
			MonoService.Instance.RemoveUpdateListener(action);
		}
		//LateUpdate
		public static void NS_AddLateUpdListener(UnityAction action) {
			MonoService.Instance.AddLateUpdateListener(action);
		}
		public static void NS_RemoveLateUpdListener(UnityAction action) {
			MonoService.Instance.RemoveLateUpdateListener(action);
		}
		//FixedUpdate
		public static void NS_AddFixedUpdListener(UnityAction action) {
			MonoService.Instance.AddFixedUpdateListener(action);
		}
		public static void NS_RemoveFixedUpdListener(UnityAction action) {
			MonoService.Instance.RemoveFixedUpdateListener(action);
		}
		//Coroutine
		public static Coroutine NS_StartCoroutine(IEnumerator routine) {
			return MonoService.Instance.StartCoroutine(routine);
		}
		public static void NS_StopCoroutine(Coroutine routine) {
			MonoService.Instance.StopCoroutine(routine);
		}
		public static void NS_StopCoroutine(IEnumerator routine) {
			MonoService.Instance.StopCoroutine(routine);
		}
		public static void NS_StopAllCoroutines() {
			MonoService.Instance.StopAllCoroutines();
		}
	}
}