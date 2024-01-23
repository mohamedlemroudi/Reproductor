using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalyAdminApp.API.APISQL.Taules
{
    public class Extension
    {
        [Key]
        public string Name { get; set; } = null!;
        public ICollection<Song> Songs { get; set; } = new List<Song>();
    }
}
