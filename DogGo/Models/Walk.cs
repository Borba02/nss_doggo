using System;
using System.ComponentModel.DataAnnotations;
namespace DogGo.Models
{
    public class Walk
    {
        public int Id { get; set; }
        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime Date { get; set; }
        public int Duration { get; set; }
        public int WalkerId { get; set; }
        public int DogId { get; set; }
        public Walker Walker { get; set; }
        public Dog Dog { get; set; }
        public Owner Owner { get; set; }
        public string WalkDuration
        {
            get
            {
                var totalMinutes = Duration / 60;
                var totalHours = totalMinutes / 60;
                var minutes = totalMinutes % 60;
                return $"{totalHours} hrs {minutes} min";
            }
        }
    }
}
