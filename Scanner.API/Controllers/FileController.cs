using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scanner.Core.Abstract;
using Scanner.Core.Models;

namespace Scanner.API.Controllers
{
    public class FileController : BaseController<File>
    {
        public FileController(IService<File> service) : base(service)
        {
        }
    }
}
