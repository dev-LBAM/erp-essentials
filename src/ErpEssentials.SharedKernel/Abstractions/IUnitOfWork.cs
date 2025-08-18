﻿namespace ErpEssentials.SharedKernel.Abstractions;

using System.Threading;
using System.Threading.Tasks;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}