namespace ZenProgramming.Chakra.Core.Persistences
{
	/// <summary>
	/// Interface for persistence element
	/// </summary>
	public interface IPersistence
	{
		/// <summary>
		/// Unique key of element
		/// </summary>
		string Key { get; set; }
	}
}
