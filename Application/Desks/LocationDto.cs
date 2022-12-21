using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Desks
{
    public class LocationDto
    {
        public int Id { get; set; }
        public string Building { get; set; }
        public int Floor { get; set; }
        public string Room { get; set; }
        public List<DeskDto> Desks { get; set; }
    }
}