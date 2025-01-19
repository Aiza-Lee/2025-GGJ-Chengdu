using UnityEngine;
using UnityEditor;
namespace NSFrame {
	[CustomEditor(typeof(AudioConfig))]
	public class AudioConfigInspector : Editor {
		AudioConfig config;
		private void OnEnable(){
			config = (AudioConfig)target;
		}

		public override void OnInspectorGUI() {

			EditorGUILayout.Space();

			if (NSFrameRoot.Initialized) {
				AudioSystem.GlobalVolume = EditorGUILayout.Slider("Global Volume", config._globalVolume, 0f, 1f);
				AudioSystem.BGMVolume = EditorGUILayout.Slider("BGM Volume", config._bgmVolume, 0f, 1f);
				AudioSystem.SFXVolume = EditorGUILayout.Slider("SFX Volume", config._sfxVolume, 0f, 1f);
				AudioSystem.MuteBGM = EditorGUILayout.Toggle("Mute BGM", config._muteBGM);
				AudioSystem.MuteSFX = EditorGUILayout.Toggle("Mute SFX", config._muteSFX);
			}
			else {
				config._globalVolume = EditorGUILayout.Slider("Global Volume", config._globalVolume, 0f, 1f);
				config._bgmVolume = EditorGUILayout.Slider("BGM Volume", config._bgmVolume, 0f, 1f);
				config._sfxVolume = EditorGUILayout.Slider("SFX Volume", config._sfxVolume, 0f, 1f);
				config._muteBGM = EditorGUILayout.Toggle("Mute BGM", config._muteBGM);
				config._muteSFX = EditorGUILayout.Toggle("Mute SFX", config._muteSFX);
			}

			EditorGUILayout.Space(); EditorGUILayout.Space(); EditorGUILayout.Space();

			base.OnInspectorGUI();
			
			if (GUI.changed)
				EditorUtility.SetDirty(target);
		}
	}

}