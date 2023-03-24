using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace LANChat
{
    // ПРИМЕЧАНИЕ. Можно использовать команду "Переименовать" в меню "Рефакторинг", чтобы изменить имя интерфейса "IServiceChat" в коде и файле конфигурации.
    [ServiceContract(CallbackContract =typeof(IServerChatCallback))]  //здесь мы передаём название интерфейса, который будет реализовывать CallbackContract
    public interface IServiceChat
    {
        [OperationContract] //здесь атрибута IsOneWay нет, потому что нужно обязательно дождаться, когда команда отработает и даст клиенту ID
        int Connect(string name);
        [OperationContract]
        void Disconnect(int id);

        [OperationContract(IsOneWay = true)] // этот атрибут показывает, что не нужно ждать ответа от сервера об успешном выполнении работы метода
        void SendMsg(string msg, int id);       
       
    }
    public interface IServerChatCallback
    {
        [OperationContract(IsOneWay =true)]
        void MsgCallback(string msg);
    }
}
