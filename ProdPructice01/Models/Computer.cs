using System;
using System.Collections.Generic;

namespace ProdPructice01.Models;

public partial class Computer
{
    public int Id { get; set; }

    public int ManufacturerId { get; set; }

    public string ModelName { get; set; } = null!;

    public string Cputype { get; set; } = null!;

    public decimal Cpufrequency { get; set; }

    public int Ram { get; set; }

    public int Hdd { get; set; }

    public DateOnly ReleaseDate { get; set; }

    public virtual Manufacturer Manufacturer { get; set; } = null!;

    public virtual ICollection<MarketOffer> MarketOffers { get; set; } = new List<MarketOffer>();
}
