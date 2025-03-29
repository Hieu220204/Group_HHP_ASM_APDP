using System;
using System.ComponentModel.DataAnnotations;

public class Schedule
{
    public int Id { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    public string Time { get; set; }

    [Required]
    public string Subject { get; set; }

    [Required]
    public string Room { get; set; }
}
