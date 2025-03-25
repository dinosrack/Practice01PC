using System;
using System.Collections.Generic;

namespace ProdPructice01.Models;

public partial class Seller
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public virtual ICollection<MarketOffer> MarketOffers { get; set; } = new List<MarketOffer>();
}
