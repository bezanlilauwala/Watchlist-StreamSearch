using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI3.ViewModels
{
    public class FullTitleViewModel
    {
        public string Id { get; set; }
        public string ImdbId { get; set; }
        public string Name { get; set; }
        public string ImdbRating { get; set; }
        public string ReleaseDate { get; set; }
        public string Runtime { get; set; }
        public string RuntimeStr { get; set; }
        public string Genres { get; set; }
        public string Type { get; set; }
        public string Image { get; set; }
        public string Plot { get; set; }
        public string Tagline { get; set; }
        public bool Watched { get; set; }
    }
}
