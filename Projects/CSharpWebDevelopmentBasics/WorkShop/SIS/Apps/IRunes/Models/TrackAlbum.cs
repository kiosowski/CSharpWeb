using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.Models
{
    public class TrackAlbum : BaseModel<string>
    {
        public string TrackId { get; set; }
        public virtual Track Track { get; set; }

        public string AlbumId { get; set; }
        public virtual Album Album { get; set; }
    }
}
