using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Application.Desks
{
    public class DeskDto
    {
        public int Id { get; set; }
        public bool Available { get; set; }
        public List<ReservationDto> Reservations { get; set; }
    }
}