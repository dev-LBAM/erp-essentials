using ErpEssentials.Application.Contracts.Catalogs.Brands;
using ErpEssentials.SharedKernel.Pagination;
using ErpEssentials.SharedKernel.ResultPattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpEssentials.Application.Features.Catalogs.Brands.GetAllPaged;

public record GetAllBrandsPagedQuery(int Page = 1, int PageSize = 10, string OrderBy = "Name", bool Ascending = true) : IRequest<Result<PagedResult<BrandResponse>>>;
