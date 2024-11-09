using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class Script
{
    [Key]
    public int Id { get; set; }
    public int AudioId { get; set; }
    public required string Content { get; set; }
    
    public virtual Audio Audio { get; set; }
}