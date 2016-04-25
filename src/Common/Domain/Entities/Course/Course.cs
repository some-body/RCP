using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Course : Entity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string MaterialText { get; set; }

        [Required]
        [Range(0, 100)]
        public int MinPercentage { get; set; } = 75;

        public virtual ICollection<Question> Questions { get; set; }
    }
}
