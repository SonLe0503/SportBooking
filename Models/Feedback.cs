using System;
using System.Collections.Generic;

namespace SportBooking.Models;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public int? UserId { get; set; }

    public int? FieldId { get; set; }

    public int? Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Field? Field { get; set; }

    public virtual User? User { get; set; }
}
