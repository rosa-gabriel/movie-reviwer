using Application;
using Microsoft.AspNetCore.Mvc;
using Persistence;

namespace API.Controllers
{
    public class ControllerInit : ControllerBase
    {
        protected readonly DataContext _context;
       protected readonly MovieLogic movieLogic;
        public ControllerInit(DataContext context)
        {
            _context = context;
            movieLogic = new MovieLogic(_context);
        }
    }
}