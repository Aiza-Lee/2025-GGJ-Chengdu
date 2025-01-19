using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NSFrame {
	public class PrefabPool {

		const int MAXPOPPERFRAME = 4;

		private readonly Transform _fatherTransform;
		private readonly Queue<GameObject> _poolQue;
		private int _maxCapacity;
		private readonly GameObject _prefab;

		public int Count => _poolQue.Count;


		public PrefabPool(GameObject prefab, Transform poolRootTransform, int maxCapacity = -1) {
			_maxCapacity = maxCapacity;
			if (maxCapacity == -1) _poolQue = new();
			else _poolQue = new(maxCapacity);
			_prefab = prefab;
			GameObject fatherGO = new(prefab.name + " Pool");
			_fatherTransform = fatherGO.transform;
			_fatherTransform.SetParent(poolRootTransform);
		}
		public void Push(GameObject childGO) {
			if (_maxCapacity != -1 && _poolQue.Count == _maxCapacity) {
				Object.Destroy(childGO);
				return ;
			}
			_poolQue.Enqueue(childGO);
			childGO.SetActive(false);
			childGO.transform.SetParent(_fatherTransform);
		} 
		public GameObject Pop(Transform father) {
			if (!_poolQue.TryDequeue(out GameObject childGO)) {
				childGO = Object.Instantiate(_prefab);
				childGO.name = _prefab.name;
			}
			childGO.transform.SetParent(father);
			if (father != null) 
				childGO.transform.SetParent(father);
			else
				SceneManager.MoveGameObjectToScene(childGO, SceneManager.GetActiveScene());
			childGO.SetActive(true);
			return childGO;
		}
		public void SetMaxCapacity(int maxCapacity) {
			_maxCapacity = maxCapacity;
			MonoService.Instance.StartCoroutine(DoPopMoreElement());
		}
		IEnumerator DoPopMoreElement() {
			while (_poolQue.Count > _maxCapacity) {
				int tmp = Mathf.Min(MAXPOPPERFRAME, _poolQue.Count - _maxCapacity);
				for (int i = 0; i < tmp; ++i) 
					if (_poolQue.TryDequeue(out GameObject go)) 
						Object.Destroy(go);
				yield return null;
			}
		}
	}
}

