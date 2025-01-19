using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSFrame
{
	public static class AudioSystem {
		private static readonly AudioConfig _config;
		private static readonly Dictionary<string, AudioClip> _bgmClipsDic;
		private static readonly Dictionary<string, AudioClip> _sfxClipsDic;
		private static readonly AudioSource _bgmPlayer;
		private static readonly Transform _audioRoot, _sfxRoot;
		static AudioSystem() {
			_config = NSFrameRoot.Instance.GetConfig<AudioConfig>();
			_bgmClipsDic = new();
			_sfxClipsDic = new();
			foreach (var pair in _config.BGMAudioClips) _bgmClipsDic.Add(pair.Key, pair.Value);
			foreach (var pair in _config.SFXAuidoClips) _sfxClipsDic.Add(pair.Key, pair.Value);
			GameObject audioRoot = new("Audio Root");
			_audioRoot = audioRoot.transform;
			_audioRoot.SetParent(NSFrameRoot.FrameRootTransform);

			GameObject sfxRoot = new("SFX Root");
			_sfxRoot = sfxRoot.transform;
			_sfxRoot.SetParent(_audioRoot);

			GameObject go = new("BGM Player");
			go.transform.SetParent(_audioRoot);
			_bgmPlayer = go.AddComponent<AudioSource>();
			go.transform.SetParent(_audioRoot);

			PoolSystem.InitPrefabPool(_config.SFXAudioSourcePrefab);

			GlobalVolume = _config._globalVolume;
			BGMVolume = _config._bgmVolume;
			SFXVolume = _config._sfxVolume;
			MuteBGM = _config._muteBGM;
			MuteSFX = _config._muteSFX;
		}

		public static float GlobalVolume { 
			get => _config._globalVolume;
			set {
				if (value.IsApproximatelyEqual(_config._globalVolume)) return;
				_config._globalVolume = value;
				OnBGMVolumeChanged();
			}
		}
		public static float BGMVolume {
			get => _config._bgmVolume;
			set {
				if (value.IsApproximatelyEqual(_config._bgmVolume)) return;
				_config._bgmVolume = value;
				OnBGMVolumeChanged();
			}
		}
		public static float SFXVolume {
			get => _config._sfxVolume;
			set {
				if (value.IsApproximatelyEqual(_config._sfxVolume)) return;
				_config._sfxVolume = value;
			}
		}
		private static void OnBGMVolumeChanged() {
			_bgmPlayer.volume = GlobalVolume * BGMVolume;
		}
	
		public static bool MuteBGM {
			get => _config._muteBGM;
			set {
				if (value == _config._muteBGM) return;
				_config._muteBGM = value;
				_bgmPlayer.mute = _config._muteBGM;
			}
		}
		public static bool MuteSFX {
			get => _config._muteSFX;
			set {
				if (value == _config._muteSFX) return;
				_config._muteSFX = value;
			}
		}

		public static void PlayBGM(string clipName, bool isloop = true) {
			if (!_bgmClipsDic.ContainsKey(clipName)) {
				Debug.LogError($"NS: No BGM named \"{clipName}\".");
				return;
			}
			_bgmPlayer.clip = _bgmClipsDic[clipName];
			_bgmPlayer.loop = isloop;
			_bgmPlayer.volume = GlobalVolume * BGMVolume;
			_bgmPlayer.Play();
		}
		public static void PauseBGM() {
			if (_bgmPlayer.clip == null) return;
			if (_bgmPlayer.isPlaying == false) return;
			_bgmPlayer.Pause();
		}
		public static void ContinueBGM() {
			if (_bgmPlayer.clip == null) return;
			if (_bgmPlayer.isPlaying) return;
			_bgmPlayer.UnPause();
		}
		public static void StopBGM() {
			if (_bgmPlayer.clip == null) return;
			if (_bgmPlayer.isPlaying) return;
			_bgmPlayer.Stop();
		}

		public static void PlaySFX(string clipName) {
			if (!_sfxClipsDic.ContainsKey(clipName)) {
				Debug.LogError($"NS: No SFX named \"{clipName}\".");
				return;
			}
			if (MuteSFX) return;
			AudioSource player = PoolSystem.PopGO<AudioSource>(_config.SFXAudioSourcePrefab, _sfxRoot);
			player.clip = _sfxClipsDic[clipName];
			player.volume = GlobalVolume * SFXVolume;
			player.loop = false;
			player.Play();
			MonoServiceTool.NS_StartCoroutine(DoRecycle(player));
		}
		static IEnumerator DoRecycle(AudioSource player) {
			yield return new WaitForSeconds(player.clip.length);
			player.clip = null;
			PoolSystem.PushGO(player.gameObject);
		}
	}
}