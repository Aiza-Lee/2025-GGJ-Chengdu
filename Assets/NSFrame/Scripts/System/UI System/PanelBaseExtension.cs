
namespace NSFrame {
	public static class PanelBaseExtension {
		public static void AddToFrame(this PanelBase panel) { UISystem.AddUIPanel(panel); }
		public static void Show(this PanelBase panel) { UISystem.Show(panel); }
		public static void Close(this PanelBase panel) { UISystem.Close(panel); }
	}
}