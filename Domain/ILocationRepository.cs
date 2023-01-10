using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public interface ILocationRepository
    {
        Task<Location> FindLocationWithDesksById(int id);
        void Remove(Location location);
        Task<int> Save();
    }
}