using System.ComponentModel.DataAnnotations;

namespace Form.Models
{
    public class Cities
    {
        [Key]
        public int CityId { get; set; }
        public string CityName { get; set; }

        public int StateId { get; set; }
    }
}
