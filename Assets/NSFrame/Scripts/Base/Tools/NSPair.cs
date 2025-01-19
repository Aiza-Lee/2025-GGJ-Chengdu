namespace NSFrame 
{
	[System.Serializable]
	public struct NSPair<T1, T2> {
		public T1 Key;
		public T2 Value;
		
		public NSPair(T1 t1, T2 t2) {
			Key = t1;
			Value = t2;
		}
	}
}