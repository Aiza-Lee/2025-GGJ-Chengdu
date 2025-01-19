using System.Collections.Generic;
using UnityEngine;

namespace NSFrame {
	[CreateAssetMenu(fileName = "AudioConfig", menuName = "NSFrame/AudioConfig")]
	public class AudioConfig : ConfigBase {
		public GameObject SFXAudioSourcePrefab;
		[HideInInspector] public float _globalVolume;
		[HideInInspector] public float _bgmVolume;
		[HideInInspector] public float _sfxVolume;
		[HideInInspector] public bool _muteBGM;
		[HideInInspector] public bool _muteSFX;


		// todo: 外面一层对音效的分类先不做，以后要做的话还是改成枚举加数组的形式，更安全，更快
		public List<NSPair<string, AudioClip>> BGMAudioClips;
		public List<NSPair<string, AudioClip>> SFXAuidoClips;
	}
}