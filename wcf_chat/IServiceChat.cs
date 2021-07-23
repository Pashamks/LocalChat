using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace wcf_chat
{
    // ПРИМЕЧАНИЕ. Можно использовать команду "Переименовать" в меню "Рефакторинг", чтобы изменить имя интерфейса "IServiceChat" в коде и файле конфигурации.
    [ServiceContract(CallbackContract = typeof(IServerChatCallBack))]
    public interface IServiceChat
    {
        [OperationContract]
        int Connect(string name);// here we have to wait for serwer answer
        [OperationContract]
        void DisConnect(int id);
        [OperationContract(IsOneWay = true)]//we don't need to wait for answer from serwer
        void SentMsg(string msg, int id);
    }

    public interface IServerChatCallBack
    {
        [OperationContract(IsOneWay = true)]
        void MsgCallBack(string msg);
    }
}
