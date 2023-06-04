using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webblog.Services
{
    public interface IMessageSender
    {
        public string Send();
    }
}
