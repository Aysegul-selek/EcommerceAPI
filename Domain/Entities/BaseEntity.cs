using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public abstract class BaseEntity
    {
        public long Id { get; set; }                
        public DateTime CreatedDate { get; set; } 
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? CreateUser { get; set; }  
        public string? UpUser { get; set; }    
    }
}
