using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GliwickiDzik.DTOs
{
    public class ExerciseForCreateDTO
    {
        [Required(ErrorMessage = "Name cannot be empty!")]
        public string Name { get; set; }
    }
}
