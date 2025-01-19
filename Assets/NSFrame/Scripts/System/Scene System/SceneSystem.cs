using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using static NSFrame.MonoServiceTool;

// note: 在小项目中，通过名称加载场景完全没有问题，所以这里只需要做异步加载的进度监听，加载过渡就可以了
namespace NSFrame {
	public static class SceneSystem {

		private static readonly Dictionary<string, bool> _scenesLoadedDic = new();

		private static bool _isTransitioning = false;

		static SceneSystem() {
			int sceneCount = SceneManager.sceneCount;
			for (int i = 0; i < sceneCount; ++i)
				_scenesLoadedDic.Add(SceneManager.GetSceneAt(i).name, false);
			_scenesLoadedDic[SceneManager.GetActiveScene().name] = true;
		}

		/// <summary>
		/// 加载进度触发事件,传入的参数就是当前进度 "LoadSceneProcess_f"
		/// </summary>
		public static void LoadSceneAsync(string sceneName, UnityAction callBack = null) {
			if (_isTransitioning) return;
			if (!_scenesLoadedDic.ContainsKey(sceneName)) {
				Debug.LogError($"NS: Can't find scene named \"{sceneName}\".");
				return;
			}
			if (_scenesLoadedDic[sceneName] == true) {
				Debug.LogWarning($"NS: The scene named \"{sceneName}\" has been loaded.");
				return;
			}
			NS_StartCoroutine(DoLoadSceneAsync(sceneName, callBack));
		}
		static IEnumerator DoLoadSceneAsync(string sceneName, UnityAction callBack) {
			yield return NS_StartCoroutine(PlayTransitionAnimation(true));

			if (SceneManager.loadedSceneCount > 0)
				UnloadSceneAsync(SceneManager.GetActiveScene().name);

			AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName);
			loadOp.allowSceneActivation = false;
			while (!loadOp.isDone) {
				EventSystem.Invoke<float>("LoadSceneProcess_f", loadOp.progress);
				if (loadOp.progress >= 0.9f) 
					loadOp.allowSceneActivation = true;
				yield return null;
			}
			EventSystem.Invoke<float>("LoadSceneProcess_f", 1);
			_scenesLoadedDic[sceneName] = true;
			callBack.Invoke();

			NS_StartCoroutine(PlayTransitionAnimation(false));
		}
		static IEnumerator PlayTransitionAnimation(bool fadeOut) {
			_isTransitioning = true;
			float duration = 1f;
			float elapsedTime = 0f;

			// Color startColor = fadeOut ? Color.clear : Color.black;
			// Color endColor = fadeOut ? Color.black : Color.clear;
			// transitionImage.color = startColor;

			while (elapsedTime < duration) {
				// transitionImage.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
				elapsedTime += Time.deltaTime;
				yield return null;
			}
			// transitionImage.color = endColor;

			_isTransitioning = false;
		}

		private static void UnloadSceneAsync(string sceneName) {
			if (_scenesLoadedDic.ContainsKey(sceneName) && _scenesLoadedDic[sceneName]) {
				AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(sceneName);
				unloadOp.completed += (op) => {
					_scenesLoadedDic[sceneName] = false;
				};
			}
		}

	}
}