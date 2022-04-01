using System.Collections.Generic;
using DogGo.Models;

namespace DogGo.Interfaces
{
    public interface IWalksRepository
    {
        List<Walk> GetWalksByWalkerId(int walkerId);
    }
}
