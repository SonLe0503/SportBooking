using System;
using System.Collections.Generic;

namespace SportBooking.Models;

public partial class Field
{
    public int FieldId { get; set; }

    public string? FieldName { get; set; }

    public string? Location { get; set; }

    public decimal? Price { get; set; }

    public string? Description { get; set; }

    public string? Image { get; set; }

    public int? OwnerId { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<ImageField> ImageFields { get; set; } = new List<ImageField>();

    public virtual User? Owner { get; set; }
}
