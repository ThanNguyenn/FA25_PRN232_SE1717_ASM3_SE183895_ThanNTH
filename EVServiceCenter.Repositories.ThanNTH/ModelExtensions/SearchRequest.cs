using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVServiceCenter.Repositories.ThanNTH.ModelExtensions;
public class SearchRequest
{
    public int? CurrentPage { get; set; } = 1;

    public int? PageSize { get; set; } = 10;
}

public class CenterPartThanNthSearchRequest : SearchRequest
{
    public string? PartStatus { get; set; }
    public int? AvailableQuantity { get; set; }
    public string? PartName { get; set; }
}
