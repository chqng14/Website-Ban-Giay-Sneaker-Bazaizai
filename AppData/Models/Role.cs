using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Models
{
    public class Role
    {
        public Guid IdRole { get; set; }
        public string CodeRole { get; set; }
        public string NameRole { get; set; }
        public string Status { get; set; }
        public virtual IEnumerable<User> Users { get; set; }
    }
}
