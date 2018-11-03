using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.Models
{
    public class Track : BaseModel<string>
    {
        public Track()
        {
            this.Albums = new HashSet<TrackAlbum>();
        }
        public string Name { get; set; }

        public string Link { get; set; }

        public decimal Price { get; set; }

       public virtual ICollection<TrackAlbum> Albums { get; set; }

    }
}
