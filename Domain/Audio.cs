using System.ComponentModel.DataAnnotations;
using Models;

namespace Models;

public class Audio : BaseAuditableEntity
{
    [Key]
    public int Id { get; set; }
    public required string Title { get; set; }   
    public int Duration { get; set; }
    public required string AudioUrl { get; set; }  
    
    public virtual Script Script { get; set; }
}