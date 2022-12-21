using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Desks;
using AutoMapper;
using Domain;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Location, Location>();
            CreateMap<Location, LocationDto>();
            CreateMap<Desk, DeskDto>();
            CreateMap<Employee, EmployeeDto>();
            CreateMap<Reservation, ReservationDto>();
        }   
    }
}