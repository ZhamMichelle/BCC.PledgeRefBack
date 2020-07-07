using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BCC.PledgeRefBack.Services
{
    public interface IApplicationEntity
    {
        int Id { get; set; }
    }

    public abstract class ApplicationEntity : IApplicationEntity
    {
        public int Id { get; set; }
    }
}
