using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalyAdminApp.API.APISQL.Taules
{
    public class PlayList
    {
        public string Dispositiu { get; set; } = null!;
        public string PlaylistName { get; set; } = null!;

        public DateTime? CreationDate { get; set; }

        public ICollection<Song> Songs { get; set; } = new List<Song>();
    }
}
