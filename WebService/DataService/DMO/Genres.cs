﻿using System.Collections.Generic;

namespace WebService.DataService.DMO
{
    public class Genres
    {
        public int GenreId { get; set; }
        public string Genre { get; set; }
        public virtual ICollection<TitleGenres> TitleGenres { get; set; }
    }
}
