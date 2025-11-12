using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpEssentials.Stock.Application.Contracts.Products;

public record UpdateProductDetailsRequest(
    string? NewName,
    string? NewDescription,
    string? NewBarcode
);
