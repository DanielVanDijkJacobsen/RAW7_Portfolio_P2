﻿using WebService.DataService.DTO;

namespace WebService.DataService.Repositories
{
    class GenreRepository : GenericRepository<Genres>
    {
        public GenreRepository(ImdbContext context) : base(context)
        {

        }
    }
}
