using System.ComponentModel.DataAnnotations;

namespace BanquetHallProject.Models
{
    public class HallAPIDataModel
    {
        public int Id {  get; set; }

        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }

        public int Price { get; set; }
    }
}
