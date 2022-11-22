using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Persistence;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application
{
    public class GenerateIds
    {
        private readonly DataContext _context;
        public GenerateIds(DataContext context)
        {
            _context = context;
        }


        public async Task<int> newTagId()
        {
            Random rnd = new Random();
            int id = 0;
            TagName? dataTag = null;
            do
            {
                id = rnd.Next(111111, 999999);
                dataTag = await _context.TagNames.FindAsync(id);
            } while (dataTag != null);
            return id;
        }

        public async Task<int> newCastId()
        {
            Random rnd = new Random();
            int id = 0;
            Cast? dataCast = null;
            do
            {
                id = rnd.Next(111111, 999999);
                dataCast = await _context.Casts.FindAsync(id);
            } while (dataCast != null);
            return id;
        }
        public async Task<int> newMovieId()
        {
            Random rnd = new Random();
            int id = 0;
            Movie? dataMovie = null;
            do
            {
                id = rnd.Next(111111, 999999);
                dataMovie = await _context.Movies.FindAsync(id);
            } while (dataMovie != null);
            return id;
        }
        public async Task<int> newTagEntryId()
        {
            Random rnd = new Random();
            int id = 0;
            TagEntry? dataTag = null;
            do
            {
                id = rnd.Next(111111, 999999);
                dataTag = await _context.TagEntries.FindAsync(id);
            } while (dataTag != null);
            return id;
        }

        public async Task<int> newCastEntryId()
        {
            Random rnd = new Random();
            int id = 0;
            CastEntry? dataCast = null;
            do
            {
                id = rnd.Next(111111, 999999);
                dataCast = await _context.CastEntries.FindAsync(id);
            } while (dataCast != null);
            return id;
        }
    }
}