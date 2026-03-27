using System.Linq;
using System.ServiceModel;
using System.Collections.Generic;

namespace ChatLibrary
{
    [ServiceContract(CallbackContract = typeof(IChatServiceCallback))]
    public interface IChatService
    {
        [OperationContract]
        int Connect(string userName);

        [OperationContract(IsOneWay = true)]
        void Disconnect(int userId);

        [OperationContract(IsOneWay = true)]
        void SendMessage(string msg);
    }

    [ServiceContract]
    public interface IChatServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void SendMessageToClient(string msg);
    }

    public class ChatUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public OperationContext Context { get; set; }
    }

    [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single)]
    public class ChatService : IChatService
    {
        List<ChatUser> _connectedUsers = new List<ChatUser>();
        int _nextUserId = 1;

        public int Connect(string userName)
        {
            ChatUser user = new ChatUser()
            {
                Id = _nextUserId++,
                Name = userName,
                Context = OperationContext.Current
            };

            _connectedUsers.Add(user);
            return user.Id;
        }

        public void Disconnect(int userId)
        {
            var user = _connectedUsers.FirstOrDefault(x => x.Id == userId);
            if (user != null) {
                _connectedUsers.Remove(user);
            }
        }

        public void SendMessage(string msg)
        {
            foreach (ChatUser user in _connectedUsers)
            {
                user.Context.GetCallbackChannel<IChatServiceCallback>().SendMessageToClient(msg);
            }
        }
    }
}
