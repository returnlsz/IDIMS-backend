﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test.Service.User.Dto
{
    public class InputUserDto
    {
        public string id { get; set; }
        public string pwd { get; set; }
        public string validatekey { get; set; }
        public string validatecode { get; set; }
    }
}
