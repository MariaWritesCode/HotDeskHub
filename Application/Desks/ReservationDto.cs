using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Application.Desks
{
    public class ReservationDto
    {
        public EmployeeDto Employee { get; set; }
        public DateTime Date { get; set; }
    }
}