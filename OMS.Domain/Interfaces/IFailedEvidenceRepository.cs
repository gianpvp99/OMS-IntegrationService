using OMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.Domain.Interfaces
{
    public interface IFailedEvidenceRepository
    {
        Task AddAsync(FailedEvidence evidence);
    }
}
