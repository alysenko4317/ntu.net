
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Globalization;

namespace webapiSample.Controllers
{
    public class serverrandomdata
    {
        public string x { get; set; }
        public string y { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class ServerRandomController : ControllerBase
    {
        public static List<serverrandomdata> data = new List<serverrandomdata>();

        static Random random = new Random();

        [HttpGet]
        public List<serverrandomdata> Get()
        {
            int i = random.Next(100);

            // generate new data (point)

            serverrandomdata current = new serverrandomdata()
            {
                x = DateTime.Now.ToString("u", DateTimeFormatInfo.InvariantInfo),
                y = i.ToString(),
            };

            // add elements to the list and remove oldest items

            data.Add(current);
            if (data.Count > 10)
                data.RemoveRange(0, data.Count - 10);

            return data;
        }
    }
}
