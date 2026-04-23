using System.ComponentModel.DataAnnotations;

namespace facilitador_api.Model
{
    public abstract class BaseModel
    {
        [Key]
        public int Id { get; set; }
        public bool Active { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public BaseModel()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
