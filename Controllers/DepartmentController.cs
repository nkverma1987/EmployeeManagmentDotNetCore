﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo1.Controllers
{
    public class DepartmentController : Controller
    {
        public IActionResult Index()
        {
            return View();    //
        }
       
    }
}