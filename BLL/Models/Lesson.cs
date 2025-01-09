using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models;

public class Lesson
{
    public Lesson()
    {
    }

    public Lesson(string title, DateTime date)
    {
        Title = title;
        Date = date;
    }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }

    public required string Title { get; set; }

    public DateTime Date { get; set; }
    // Other lesson properties
}