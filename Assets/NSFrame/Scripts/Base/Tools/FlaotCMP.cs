using System;
using UnityEngine;

namespace NSFrame {
	public static class FlaotCMP {
		const float EPS = 1e-3f;
		public static bool IsApproximatelyEqual(this float a, float b) {
			return Math.Abs(a - b) < EPS;
		}
		public static bool IsApproximatelyEqual(this Vector3 a, Vector3 b) {
			return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y) + Math.Abs(a.z - b.z) < EPS;
		}
	}
}