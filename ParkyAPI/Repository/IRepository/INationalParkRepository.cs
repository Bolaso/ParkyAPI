using ParkyAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Repository.IRepository
{
    public interface INationalParkRepository
    {
        ICollection<NationalParkDto> GetNationalParks();
        NationalParkDto GetNationalPark(int nationalParkId);
        bool NationalParkExists(string name);
        bool NationalParkExists(int id);
        bool CreateNationNalPark(NationalParkDto nationalPark);
        bool UpdateNationNalPark(NationalParkDto nationalPark);
        bool DeleteNationNalPark(NationalParkDto nationalPark);
        bool Save(); 
    }
}
