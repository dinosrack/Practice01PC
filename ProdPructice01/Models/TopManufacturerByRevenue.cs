using System;
using System.Collections.Generic;

namespace ProdPructice01.Models;

public partial class TopManufacturerByRevenue
{
    public string ManufacturerName { get; set; } = null!;

    public decimal? TotalRevenue { get; set; }
}
