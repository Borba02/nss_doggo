using System;
using System.Collections.Generic;
using System.Linq;

namespace DogGo.Models.ViewModels
{
    public class WalkerProfileViewModel
    {
        public Walker Walker { get; set; }
        public List<Walk> Walks { get; set; }
        public string TotalWalkTime
        {
            get
            {
                var totalMinutes = Walks.Select(w => w.Duration).Sum() / 60;
                var totalHours = totalMinutes / 60;
                var minutes = totalMinutes % 60;
                return $"{totalHours} hrs {minutes} min";
            }
        }
    }
}
