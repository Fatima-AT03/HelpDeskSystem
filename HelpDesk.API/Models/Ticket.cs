using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.API.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        public string ReferenceNumber { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int CategoryId { get; set; }

        public int PriorityId { get; set; }

        public int StatusId { get; set; }

        public int CreatedBy { get; set; }

        public int? AssignedTo { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }

        [ForeignKey(nameof(PriorityId))]
        public Priority? Priority { get; set; }

        [ForeignKey(nameof(StatusId))]
        public Status? Status { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        public User? Creator { get; set; }

        [ForeignKey(nameof(AssignedTo))]
        public User? Assignee { get; set; }
    }
}