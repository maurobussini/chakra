using System.ComponentModel.DataAnnotations;

namespace ZenProgramming.Chakra.Core.Entities
{
    /// <summary>
    /// Interface for entity with generic identifier
    /// </summary>
    public interface IModernEntity: IEntity
    {
		/// <summary>
		/// Primary entity value
		/// </summary>
		[StringLength(255)]
		string Id { get; set; }
    }
}
