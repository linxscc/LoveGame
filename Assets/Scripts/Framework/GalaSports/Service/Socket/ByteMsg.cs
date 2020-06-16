using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;

namespace com.galasports.basketballmaster
{
    namespace Service.Sockets
    {
        public class ByteMsg
        {
            private List<Action<byte[]>> handlerList = new List<Action<byte[]>>();
            public void addPush(Action<byte[]> handler)
            {
                handlerList.Add(handler);
            }

            public void removePush(Action<byte[]> handler)
            {
                handlerList.Remove(handler);
            }

            public void removePush()
            {
                handlerList.Clear();
            }

            public void Emit(byte[] res)
            {
                foreach (var item in handlerList)
                {
                    item.Invoke(res);
                }
            }

        }
    }
}
