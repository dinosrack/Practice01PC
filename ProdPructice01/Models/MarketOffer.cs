using System;
using System.Collections.Generic;

namespace ProdPructice01.Models;

public partial class MarketOffer
{
    public int Id { get; set; }

    public int ComputerId { get; set; }

    public int SellerId { get; set; }

    public int BatchSize { get; set; }

    public decimal BatchPrice { get; set; }

    public virtual Computer Computer { get; set; } = null!;

    public virtual Seller Seller { get; set; } = null!;
}
