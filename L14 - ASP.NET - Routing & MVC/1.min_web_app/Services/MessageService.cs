using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webblog.Services;

namespace webblog.Services
{
    public class MessageService
    {
        IMessageSender _senderImpl;

        public MessageService(IMessageSender sender) {
            _senderImpl = sender;
        }

        public string Send() {
            return _senderImpl.Send();
        }
    }
}
