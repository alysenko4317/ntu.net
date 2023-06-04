using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webblog.Services
{
    public class SmsMessageSender : IMessageSender
    {
        public string Send()
        {
            return "Send by Sms";
        }
    }
}
