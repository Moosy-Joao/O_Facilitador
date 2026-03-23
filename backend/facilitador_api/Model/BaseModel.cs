using System.ComponentModel.DataAnnotations;

namespace facilitador_api.Model
{
    public abstract class BaseModel
    {
        [Key]
        private int Id { get; set; }
        private bool Active { get; set; } = true;
        private DateTime CreatedAt { get; set; }
        private DateTime UpdatedAt { get; set; }

        public BaseModel()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
