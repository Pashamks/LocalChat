using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace wcf_chat
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServiceChat : IServiceChat
    {
        List<ServerUser> users = new List<ServerUser>();

        int nextId = 1;
        public int Connect(string name)
        {
            ServerUser user = new ServerUser() { 
                ID = nextId, 
                Name = name, 
                operationContext = OperationContext.Current};

            ++nextId;
            SentMsg(user.Name + " connected to chat !", 0);
            users.Add(user);

            return user.ID;
        }

        public void DisConnect(int id)
        {
            var user = users.FirstOrDefault(i => i.ID == id);
            if(user!=null)
            {
                users.Remove(user);
                SentMsg(user.Name + " disconnected !", 0);
            }
        }

        public void SentMsg(string msg, int id)
        {
            string answer;
            foreach(var item in users)
            {
                answer = DateTime.Now.ToShortTimeString();
                var user = users.FirstOrDefault(i => i.ID == id);
                if (user != null)
                {
                    answer += " " + user.Name + " ";
                }
                answer += msg;
                item.operationContext.GetCallbackChannel<IServerChatCallBack>().MsgCallBack(answer);
            }
        }
    }
}
