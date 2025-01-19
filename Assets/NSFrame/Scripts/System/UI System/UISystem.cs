using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NSFrame {
	public static class UISystem {

		const int REF_RESOLUTION_X = 1920;
		const int REF_RESOLUTION_Y = 1080;

		private static readonly Transform _UIRootTransform;
		private static readonly Dictionary<string, PanelBase>[] _panelDic;
		private static readonly Transform[] _canvases;
		private static int[] _activatedPanelCnt;
		private static readonly int TYPE_SIZE;

		// private static PanelBase _curPanel;

		static UISystem() {
			TYPE_SIZE = (int)UITypeEnum.AlwaysTop + 1;
			GameObject uiRoot = new("UI Root");
			_UIRootTransform = uiRoot.transform;

			_UIRootTransform.SetParent(NSFrameRoot.FrameRootTransform);
			_panelDic = new Dictionary<string, PanelBase>[TYPE_SIZE];
			
			_canvases = new Transform[TYPE_SIZE];
			_activatedPanelCnt = new int[TYPE_SIZE];
			for (int i = 0; i < TYPE_SIZE; ++i) {
				_activatedPanelCnt[i] = 0;
				_panelDic[i] = new();

				GameObject go = new("Canvas " + i.ToString());
				go.transform.SetParent(uiRoot.transform);

				Canvas canvas = go.AddComponent<Canvas>();
				canvas.renderMode = RenderMode.ScreenSpaceOverlay;
				canvas.vertexColorAlwaysGammaSpace = true;

				CanvasScaler canvasScaler = go.AddComponent<CanvasScaler>();
				canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
				canvasScaler.referenceResolution = new(REF_RESOLUTION_X, REF_RESOLUTION_Y);

				go.AddComponent<GraphicRaycaster>();

				_canvases[i] = go.transform;

				go.SetActive(false);
			}
		}

		public static void Show(string panelName, UITypeEnum uiType) {
			if (!_panelDic[(int)uiType].ContainsKey(panelName)) {
				Debug.LogError($"NS: No UI named \"{panelName}\".");
				return ;
			}
			Show(_panelDic[(int)uiType][panelName]);
		}
		public static void Show(PanelBase panel) {
			if (panel.isLoaded) return;
			panel.isLoaded = true;
			panel.transform.SetAsLastSibling();
			if (_activatedPanelCnt[panel.GetTypeNum] == 0)
				_canvases[panel.GetTypeNum].gameObject.SetActive(true);
			++_activatedPanelCnt[panel.GetTypeNum];
			panel.gameObject.SetActive(true);
			panel.OnShow();
		}
		
		public static void Close(string panelName, UITypeEnum uiType) {
			if (!_panelDic[(int)uiType].ContainsKey(panelName)) {
				Debug.LogError($"NS: No UI named \"{panelName}\".");
				return ;
			}
			Close(_panelDic[(int)uiType][panelName]);
		}
		public static void Close(PanelBase panel) {
			if (!panel.isLoaded) return;
			panel.isLoaded = false;
			panel.transform.SetAsFirstSibling();
			if (_activatedPanelCnt[panel.GetTypeNum] == 1)
				_canvases[panel.GetTypeNum].gameObject.SetActive(false);
			--_activatedPanelCnt[panel.GetTypeNum];
			panel.gameObject.SetActive(false);
			panel.OnClose();
		}

		public static void AddUIPanel(PanelBase panel) {
			if (_panelDic[panel.GetTypeNum].ContainsKey(panel.name)) return;
			_panelDic[panel.GetTypeNum].Add(panel.name, panel);
			panel.transform.SetParent(_canvases[panel.GetTypeNum]);
			panel.gameObject.SetActive(false);
		}

		private static void RemoveUIPanel(string panelName, UITypeEnum uiType) {}
		private static void RemoveUIPanel(PanelBase panel) {}

		public static void InitUISystem() {
			Debug.Log("UI System Init");
		}
	}
}