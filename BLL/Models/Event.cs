using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BLL.Models;

public class Event
{
    public Event()
    {
    }

    public Event(string title, DateTime start, DateTime end, int instructorId, int? studentId = null, string status = "Available", int duration = 1)
    {
        Title = title;
        Start = start;
        End = end;
        InstructorId = instructorId;
        StudentId = studentId;
        Status = status;
        Duration = duration;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public DateTime Start { get; set; }

    [Required]
    public DateTime End { get; set; }

    public int InstructorId { get; set; }

    public int? StudentId { get; set; }

    public string Status { get; set; } // "Available", "Booked", "Pending"

    public int Duration { get; set; } // 1 or 2 hours
}
