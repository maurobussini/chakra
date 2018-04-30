namespace ZenProgramming.Chakra.Core.Entities
{
    /// <summary>
    /// Base interface for entiry with id
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IEntity<TKey>: IEntity
        where TKey: struct
    {
        /// <summary>
        /// Primary entity value
        /// </summary>
        TKey? Id { get; set; }
    }
}
