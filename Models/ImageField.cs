using System;
using System.Collections.Generic;

namespace SportBooking.Models;

public partial class ImageField
{
    public int ImageId { get; set; }

    public int FieldId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual Field Field { get; set; } = null!;
}
