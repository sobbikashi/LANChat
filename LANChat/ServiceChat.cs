using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace LANChat
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)] //Три варианта создания сервиса: по вызову, по сессии или сингл, то есть единый для всех
    public class ServiceChat : IServiceChat
    {
        List<ServerUser> users = new List<ServerUser>(); //список юзеров чата
        int nextId = 1;
        public int Connect(string name)
        {
            ServerUser user = new ServerUser() //создаём нового юзера
            {
                ID = nextId,
                Name = name,
                operationContext = OperationContext.Current
            };
            nextId++;
            SendMsg(user.Name + " вошёл в чат", 0);
            users.Add(user); //добавляем его в список
            return user.ID; //отдаём клиенту его Id
        }

        public void Disconnect(int id)
        {
            var user = users.FirstOrDefault(i => i.ID == id);
            if (user != null)
            {
                users.Remove(user);
                SendMsg(user.Name + " покинула чат ©", 0);
            }
        }

        public void SendMsg(string msg, int id)
        {
            foreach (var item in users)
            {
                string answer = DateTime.Now.ToShortTimeString(); //формируем сообщение: дата, имя юзера (если это коннект или дисконнект, ид юзера равен 0, соответственно, он
                var user = users.FirstOrDefault(i => i.ID == id); // не ищется, затем собственно текст сообщения
                if (user != null)
                {
                    answer += ":  " + user.Name + " ";
                }
                answer += msg;

                item.operationContext.GetCallbackChannel<IServerChatCallback>().MsgCallback(answer); //вызываем кооллбэк у клиента, передаём ему наше сообщение

            }

        }
    }
}

