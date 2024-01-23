using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalyAdminApp.API.APISQL.Taules
{
    public class Album    
    {
        [Key]
        public string? AlbumName { get; set; }

        [Key]
        public int Year { get; set; }
        [Key]
        public Guid SongUID { get; set; }
        public Song? Song { get; set; }
    }
}
