using ParkyAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Repository.IRepository
{
    public interface INationalParkRepository
    {
        ICollection<NationalPark> GetNationalParks();
        NationalPark GetNationalPark(int nationalParkId);
        bool NationalParkExists(string name);
        bool NationalParkExists(int id);
        bool CreateNationNalPark(NationalPark nationalPark);
        bool UpdateNationNalPark(NationalPark nationalPark);
        bool DeleteNationNalPark(NationalPark nationalPark);
        bool Save(); 
    }
}
