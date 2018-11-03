using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRunes.Models
{
    public class Album : BaseModel<string>
    {
        public Album()
        {
            this.Tracks = new HashSet<TrackAlbum>();
        }
        public string Name { get; set; }

        public string Cover { get; set; }

        //public decimal Price { get; set; }

        private decimal price;

        public decimal Price
        {
            get { return price; }
            set { price = this.Tracks.Where(x=>x.AlbumId==Id).Sum(x => x.Track.Price); ; }
        }
        
        public virtual ICollection<TrackAlbum> Tracks { get; set; }

    }
}
