using System;
using System.Collections.Generic;

namespace CourseWorkStore.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string? ProductDescription { get; set; }

    public decimal ProductPrice { get; set; }

    public int ProductQuantity { get; set; }

    public int? SupplierId { get; set; }

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();

    public virtual Supplier? Supplier { get; set; }
}
