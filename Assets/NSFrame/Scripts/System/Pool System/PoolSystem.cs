using System;
using System.Collections.Generic;
using UnityEngine;

namespace NSFrame {
	public static class PoolSystem {
		private static readonly Dictionary<string, PrefabPool> _prefabPoolDic;
		private static readonly Dictionary<Type, IObjectPool> _objectPoolDic;
		private static readonly Transform _poolRootTransform;

		// note: 不太清楚这样写是否会出问题，按理来说可能会调用这个静态的地方都发生在整个程序集编译完成过后，所以在这里new一个GameObject应该没啥问题
		// note: 这样可以省去使用事件或者延迟初始化，但是一定要保证在播放游戏之前“不要动这里面任何东西”，与编辑模式相关的都应该卸载Editor文件夹下
		static PoolSystem() {
			_prefabPoolDic = new();
			_objectPoolDic = new();
			_poolRootTransform = new GameObject("Pool Root").transform;
			_poolRootTransform.position = Vector3.zero;
			_poolRootTransform.SetParent(NSFrameRoot.FrameRootTransform);
		}

	  #region Prefab Pool
		private static bool CheckPrefabContained(string name) {
			if (_prefabPoolDic.ContainsKey(name)) return true;
			Debug.LogError($"NS: Have not initialize the \"{name}\" Prefab Pool");
			return false;
		}

		public static void InitPrefabPool(GameObject prefab, int maxCapacity = -1) {
			string name = prefab.name;
			if (_prefabPoolDic.ContainsKey(name)) {
				// Debug.LogError($"NS: Have areadily initialized the \"{name}\" Prefab Pool");
				return;
			}
			_prefabPoolDic.Add(name, new(prefab, _poolRootTransform, maxCapacity));
		}

		public static GameObject PopGO(GameObject prefab, Transform father = null) {
			return PopGO(prefab.name, father);
		}
		public static GameObject PopGO(string name, Transform father = null) {
			if (CheckPrefabContained(name)) return _prefabPoolDic[name].Pop(father);
			return null;
		}
		public static T PopGO<T>(GameObject prefab, Transform father = null) where T : Component {
			return PopGO<T>(prefab.name, father);
		}
		public static T PopGO<T>(string name, Transform father = null) where T : Component {
			if (CheckPrefabContained(name)) return _prefabPoolDic[name].Pop(father).GetComponent<T>();
			return null;
		}

		public static void PushGO(GameObject go) {
			string name = go.name;
			if (CheckPrefabContained(name)) _prefabPoolDic[name].Push(go);
		}

		public static void SetMaxCapacity(string name, int maxCapacity) {
			if (CheckPrefabContained(name)) _prefabPoolDic[name].SetMaxCapacity(maxCapacity);
		}
	  #endregion

	  #region Object Pool
		private static bool CheckObjectContained(Type type) {
			if (_objectPoolDic.ContainsKey(type)) return true;
			Debug.LogError($"NS: Have not initialize the \"{type}\" Object Pool");
			return false;
		}

		public static void InitObjectPool<T>(int maxCapacity = -1) where T : class, new() {
			Type type = typeof(T);
			if (_objectPoolDic.ContainsKey(type)) {
				// Debug.LogError($"NS: Have areadily initialized the \"{type}\" Object Pool");
				return;
			}
			_objectPoolDic.Add(type, new ObjectPool<T>(maxCapacity));
		}

		public static T PopObj<T>() where T : class, new() {
			return PopObj(typeof(T)) as T;
		}
		public static object PopObj(Type type) {
			if (CheckObjectContained(type)) return _objectPoolDic[type].Pop();
			return null;
		}

		public static void PushObj<T>(T obj) where T : class, new() {
			PushObj(typeof(T), obj);
		}
		public static void PushObj(Type type, object obj) {
			if (CheckObjectContained(type)) _objectPoolDic[type].Push(obj);
		}

		public static void SetMaxCapacity(Type type, int maxCapacity) {
			if (CheckObjectContained(type)) _objectPoolDic[type].SetMaxCapacity(maxCapacity);
		}
	  #endregion
	}
}


