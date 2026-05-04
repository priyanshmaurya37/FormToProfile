using System.ComponentModel.DataAnnotations;

namespace Form.Models
{
    public class States
    {
        [Key]
        public int StateId { get; set; }
        public string StateName { get; set; }
    }
}
