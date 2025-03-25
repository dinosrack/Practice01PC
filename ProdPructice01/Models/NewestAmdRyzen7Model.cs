using System;
using System.Collections.Generic;

namespace ProdPructice01.Models;

public partial class NewestAmdRyzen7Model
{
    public string ManufacturerName { get; set; } = null!;

    public string ModelName { get; set; } = null!;

    public DateOnly ReleaseDate { get; set; }
}
