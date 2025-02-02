using System;
using System.Collections.Generic;

namespace CourseWorkStore.Models;

public partial class Purchase
{
    public int PurchaseId { get; set; }

    public int ProductId { get; set; }

    public int PurchaseQuantity { get; set; }

    public decimal PurchaseAmount { get; set; }

    public DateOnly? PurchaseDate { get; set; }

    public virtual Product Product { get; set; } = null!;
}
