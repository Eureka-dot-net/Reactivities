using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Activities.DTO
{
    public class UserActivityDto
    {
        public string Id { get; set; } = null!; 

        public string Title { get; set; } = null!;

        public DateTime Date { get; set; }

        public string Category { get; set; } = null!;
    }
}
