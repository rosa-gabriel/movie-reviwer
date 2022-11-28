using Microsoft.AspNetCore.Mvc;
using Persistence;

namespace API.Controllers
{
    public class ControllerInit : ControllerBase
    {
        protected readonly DataContext _context;
        public ControllerInit(DataContext context)
        {
            _context = context;
        }
    }
}