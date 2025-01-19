
namespace NSFrame {
	public class SceneInfo {
		public string SceneName { get; private set; }
		public int SceneID { get; private set; }
		public bool IsLoaded { get; private set; }

		public SceneInfo(string sceneName, int sceneID, bool isloaded) {
			SceneName = sceneName;
			SceneID = sceneID;
			IsLoaded = isloaded;
		}
	}
}