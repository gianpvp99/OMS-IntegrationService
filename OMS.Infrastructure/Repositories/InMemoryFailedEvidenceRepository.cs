using OMS.Domain.Entities;
using OMS.Domain.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.Infrastructure.Repositories
{
    public class InMemoryFailedEvidenceRepository: IFailedEvidenceRepository
    {
        private readonly ConcurrentBag<FailedEvidence> _store = new();
        public Task AddAsync(FailedEvidence evidence)
        {
            if (evidence is null) throw new ArgumentNullException(nameof(evidence));
            _store.Add(evidence);
            return Task.CompletedTask;
        }
    }
}
