namespace NSFrame {
	public interface IObjectPool {
		void Push(object obj);
		object Pop();
		void SetMaxCapacity(int maxCapacity);
		public int Count { get; }
	}
}