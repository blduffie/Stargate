using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StargateAPI.Business.Dtos
{
    public class AstronautDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Rank { get; set; } = string.Empty;
        public string CurrentDutyTitle { get; set; } = string.Empty;
        public DateTime? CareerStartDate { get; set; }
        public DateTime? CareerEndDate { get; set; }
        public List<AstronautDutyDto> Duties { get; set; } = new();
    }

    public class AstronautDutyDto
    {
        public int Id { get; set; }
        public string DutyTitle { get; set; } = string.Empty;
        public string Rank { get; set; } = string.Empty;
        public DateTime DutyStartDate { get; set; }
        public DateTime? DutyEndDate { get; set; }
    }

    public class AstronautRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Rank { get; set; }
        public string? CurrentDutyTitle { get; set; }
        public DateTime? CareerStartDate { get; set; }
        public DateTime? CareerEndDate { get; set; }
    }

    public class AstronautDutyRequest
    {
        public string DutyTitle { get; set; } = string.Empty;
        public string Rank { get; set; } = string.Empty;
        public DateTime DutyStartDate { get; set; }
    }
}