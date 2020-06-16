using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Assets.Scripts.Framework.GalaSports.Service.Socket
{
    public class SocketClient
    {

        private System.Net.Sockets.Socket _socket; //当前套接字  
        private ByteArray _byteArray = new ByteArray(); //字节数组缓存  

        private UnityInvoke _unityInvoke;

        public delegate void OnRecevieData(int cmd, int index, byte[] data);

        public OnRecevieData OnReceiveData;

        public delegate void OnConnected();

        public OnConnected onConnected;

        public delegate void OnNetError(string e);

        public OnNetError onNetError;

        public delegate void OnConnectError(string e);

        public OnNetError onConnectError;

        public delegate void OnDecodeError(string e);

        public OnNetError onDecodeError;

        private int _msgCounter;

        private IPEndPoint _currentIpEndPoint;

        /// <summary>
        /// 网络客户端状态
        /// </summary>
        public enum NetworkClientState
        {
            Connected,
            Connecting,
            Closed
        }

        /// <summary>
        /// 网路客户端状态
        /// </summary>
        private NetworkClientState _connectState;

        public SocketClient()
        {
            _msgCounter = 0;
            // ipEndPoint = new IPEndPoint(IPAddress.Parse(SocketManager.SeverURL), SocketManager.Port);
            _connectState = NetworkClientState.Closed;
        }

        public System.Net.Sockets.Socket GetSocket()
        {
            return _socket;
        }

        public void Destroy()
        {
            if (_socket != null)
            {
                _socket.Close();
            }
            if (_byteArray != null)
            {
                _byteArray.Destroy();
                _byteArray = null;
            }
            if (_unityInvoke != null)
                _unityInvoke.Dispose();
            _isCalled = true;
            _connectState = NetworkClientState.Closed;
        }

        public void Connect()
        {
            if (_currentIpEndPoint != null)
                Connect(_currentIpEndPoint);
        }

        /// <summary>  
        /// 异步连接服务器  
        /// </summary>  
        public void Connect(IPEndPoint ipEndPoint)
        {
            if (_connectState != NetworkClientState.Closed)
            {
                if (onConnected != null)
                    onConnected.Invoke();
                Debug.Log("客户端非关闭状态，无法进行该操作!");
                return;
            }
            _currentIpEndPoint = ipEndPoint;
            _connectState = NetworkClientState.Connecting;
            _unityInvoke = new UnityInvoke("SocketClient");
            Debug.Log("connect Start");
            try
            {
                _msgCounter = 0;
                _socket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _socket.BeginConnect(ipEndPoint, DoConnect, _socket);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
            }


        }

        private void DoConnect(IAsyncResult ar)
        {
            System.Net.Sockets.Socket client = null;
            try
            {
                client = (System.Net.Sockets.Socket)ar.AsyncState;
                client.EndConnect(ar);
                _connectState = NetworkClientState.Connected;
                _msgCounter = 0;
                Debug.Log("connect success!");
                if (_byteArray == null) _byteArray = new ByteArray();
                _unityInvoke.Call((int p) =>
                {
                    if (onConnected != null)
                        onConnected.Invoke();
                }, 0);
            }
            catch (SocketException ex)
            {
                DoConnectError(ex);
                return;
            }

            try
            {
                StateObject obj = new StateObject(client);
                client.BeginReceive(obj.Buffer, 0, obj.Size, 0, DoReceive, obj);
            }
            catch (SocketException ex)
            {
                DoNetworkError(ex);
            }
        }

        private void DoReceive(IAsyncResult ar)
        {
            StateObject state = null;
            try
            {
                state = (StateObject)ar.AsyncState;
                state.Length = state.Client.EndReceive(ar);
            }
            catch (SocketException ex)
            {
                DoConnectError(ex);
            }

            if (state != null && state.Length > 0)
            {
                byte[] temp = new byte[state.Length];
                Debug.Log("接受到的字节数为" + state.Length);
                Buffer.BlockCopy(state.Buffer, 0, temp, 0, state.Length); //
                if (_msgCounter > 0) //第一个包跨域文件丢弃
                {
                    try
                    {
                        _byteArray.Write(temp);
                        HandleMessage();
                    }
                    catch (Exception)
                    {
                        _byteArray.Clear();
                        _byteArray.Write(temp);
                        HandleMessage();
                    }

                }
                _msgCounter++;
                try
                {
                    StateObject obj = new StateObject(state.Client);
                    state.Client.BeginReceive(obj.Buffer, 0, obj.Size, 0, DoReceive, obj);
                }
                catch (SocketException ex)
                {
                    DoNetworkError(ex);
                }
            }
            else
            {
                DoConnectError(null);
            }
        }


        /// <summary>  
        /// 异步发送信息  
        /// </summary>  
        public void Send(int cmd, byte[] dataMsg)
        {
            if (_connectState == NetworkClientState.Closed)
            {
                Debug.LogError(cmd + "send error,connectState Closed");
                return;
            }
            try
            {
                _msgCounter++;
                Debug.Log("<color=#00CA66FF>Send:" + cmd + "</color>");
                ByteArray ba = new ByteArray();
                if (dataMsg != null)
                {
                    ba.Write(dataMsg.Length + 8);
                    ba.Write(cmd);
                    ba.Write(_msgCounter);
                    ba.Write(dataMsg);
                }
                else
                {
                    ba.Write(8);
                    ba.Write(cmd);
                    ba.Write(_msgCounter);
                }
                byte[] data = ba.GetByteArray();
                ba.Destroy();
                _socket.BeginSend(data, 0, data.Length, 0, DoSend, _socket);
            }
            catch (SocketException ex)
            {
                DoSendError(ex);
            }

        }

        private void DoSend(IAsyncResult ar)
        {
            try
            {
                System.Net.Sockets.Socket client = (System.Net.Sockets.Socket)ar.AsyncState;
                client.EndSend(ar);
            }
            catch (SocketException ex)
            {
                DoNetworkError(ex);
            }
        }

        private bool _isCalled = true;
        private int _currentMsgLength; //用来暂存信息的长度  
        private bool _hasGetMessageLength; //是否得到了消息的长度  

        /// <summary>  
        /// 解析信息  
        /// </summary>  
        void HandleMessage()
        {
            if (_byteArray == null) return;
            if (!_hasGetMessageLength)
            {
                if (_byteArray.GetLength() - _byteArray.GetReadIndex() > 4) //消息的长度为int，占四个字节  
                {
                    _currentMsgLength = _byteArray.ReadInt32(); //读取消息的长度  
                    _hasGetMessageLength = true;
                    //Debug.Log("currentMsgLength=" + currentMsgLength);
                    if (_currentMsgLength > 100000)
                    {
                        _byteArray.Clear();
                        _hasGetMessageLength = false;
                        _currentMsgLength = 0;
                    }
                }
            }
            if (_hasGetMessageLength)
            {
                //根据长度就可以判断消息是否完整  
                //GetReadIndex()可以得到已读的字节  
                //注意上面的ReadInt32读取后，读的索引会加上4，要把多余的减去  
                //Debug.Log("byteArray.GetLength()"+ byteArray.GetLength())
                if (_byteArray.GetLength() >= (_currentMsgLength + _byteArray.GetReadIndex()) && _isCalled)
                {
                    _isCalled = false;
                    _unityInvoke.Call((int len) =>
                    {
                        try
                        {
                            int cmd = _byteArray.ReadInt32();
                            int index = _byteArray.ReadInt32();
                                //Debug.Log("currentMsgLength call =" + len);
                                byte[] data = _byteArray.ReadBytes(len - 8); //cmd 和 index 占了8个字节，需要减去

                                OnReceiveData.Invoke(cmd, index, data);
                            _hasGetMessageLength = false;
                            _isCalled = true;
                            HandleMessage();
                        }
                        catch (Exception ex)
                        {
                            _byteArray.Clear();
                            _currentMsgLength = 0;
                            Debug.LogError(" Socket Call Error " + ex);
                            _hasGetMessageLength = false;
                            _isCalled = true;
                        }

                    }, _currentMsgLength);

                }
            }
        }

        private void DoConnectError(SocketException error)
        {
            Debug.LogError("ConnectError " + error.Message + " onConnectError=" + onConnectError);
            if (onConnectError != null)
            {
                _unityInvoke.Call((int p) =>
                {
                    onConnectError.Invoke(error.Message);
                }, 0);
            }

        }

        private void DoSendError(SocketException error)
        {
            if (error == null) return;
            Debug.LogError("socket send error:" + error.Message);
        }

        private void DoNetworkError(SocketException error)
        {
            if (error == null) return;
            Debug.LogError("connect error:" + error.Message);
            switch (error.SocketErrorCode)
            {
                case SocketError.AccessDenied:
                    break;
                case SocketError.AddressAlreadyInUse:
                    break;
                case SocketError.AddressFamilyNotSupported:
                    break;
                case SocketError.AddressNotAvailable:
                    break;
                case SocketError.AlreadyInProgress:
                    break;
                case SocketError.ConnectionAborted:
                    break;
                case SocketError.ConnectionRefused:
                    break;
                case SocketError.ConnectionReset:
                    break;
                case SocketError.DestinationAddressRequired:
                    break;
                case SocketError.Disconnecting:
                    break;
                case SocketError.Fault:
                    break;
                case SocketError.HostDown:
                    break;
                case SocketError.HostNotFound:
                    break;
                case SocketError.HostUnreachable:
                    break;
                case SocketError.InProgress:
                    break;
                case SocketError.Interrupted:
                    break;
                case SocketError.InvalidArgument:
                    break;
                case SocketError.IOPending:
                    break;
                case SocketError.IsConnected:
                    break;
                case SocketError.MessageSize:
                    break;
                case SocketError.NetworkDown:
                    break;
                case SocketError.NetworkReset:
                    break;
                case SocketError.NetworkUnreachable:
                    break;
                case SocketError.NoBufferSpaceAvailable:
                    break;
                case SocketError.NoData:
                    break;
                case SocketError.NoRecovery:
                    break;
                case SocketError.NotConnected:
                    break;
                case SocketError.NotInitialized:
                    break;
                case SocketError.NotSocket:
                    break;
                case SocketError.OperationAborted:
                    break;
                case SocketError.OperationNotSupported:
                    break;
                case SocketError.ProcessLimit:
                    break;
                case SocketError.ProtocolFamilyNotSupported:
                    break;
                case SocketError.ProtocolNotSupported:
                    break;
                case SocketError.ProtocolOption:
                    break;
                case SocketError.ProtocolType:
                    break;
                case SocketError.Shutdown:
                    break;
                case SocketError.SocketError:
                    break;
                case SocketError.SocketNotSupported:
                    break;
                case SocketError.Success:
                    break;
                case SocketError.SystemNotReady:
                    break;
                case SocketError.TimedOut:
                    break;
                case SocketError.TooManyOpenSockets:
                    break;
                case SocketError.TryAgain:
                    break;
                case SocketError.TypeNotFound:
                    break;
                case SocketError.VersionNotSupported:
                    break;
                case SocketError.WouldBlock:
                    break;
                default:
                    break;
            }
            if (onNetError != null)
            {
                _unityInvoke.Call((int p) =>
                {
                    onNetError.Invoke(error.Message);
                }, 0);
            }
        }

        public void ResetSocketConnect()
        {
            try
            {
                if (_connectState == NetworkClientState.Closed) return;
                _connectState = NetworkClientState.Closed;
                if (_byteArray != null)
                {
                    _byteArray.Destroy();
                    _byteArray = null;
                }

                if (_socket != null)
                {
                    _socket.Close();
                    _socket = null;
                }

                _msgCounter = 0;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public bool ResetSocket()
        {
            if (_connectState != NetworkClientState.Closed)
            {
                _connectState = NetworkClientState.Closed;
                try
                {
                    if (_socket.Connected)
                    {
                        _socket.Shutdown(SocketShutdown.Both);
                        _socket.Dispose();
                    }
                }
                finally
                {
                    _socket.Close();
                    _socket = null;
                }
                _byteArray.Destroy();
                _byteArray = null;
                return true;
            }
            if (_unityInvoke != null)
            {
                _unityInvoke.Dispose();
                _unityInvoke = null;
            }
            _isCalled = true;
            return false;
        }
        public string GetStatus()
        {
            return _connectState.ToString();
        }
        // 检查一个Socket是否可连接  
        public bool IsSocketConnected()
        {
            System.Net.Sockets.Socket client = _socket;
            bool blockingState = client.Blocking;
            try
            {
                byte[] tmp = new byte[1];
                client.Blocking = false;
                client.Send(tmp, 0, 0);
                return false;
            }
            catch (SocketException e)
            {
                // 产生 10035 == WSAEWOULDBLOCK 错误，说明被阻止了，但是还是连接的  
                if (e.NativeErrorCode.Equals(10035))
                    return false;
                else
                    return true;
            }
            finally
            {
                client.Blocking = blockingState;    // 恢复状态  
            }
        }

    }

    internal class StateObject
    {
        public int Size = 2048;
        public byte[] Buffer;
        public System.Net.Sockets.Socket Client;
        public int Length = 0;

        public StateObject(System.Net.Sockets.Socket client)
        {
            this.Client = client;
            this.Buffer = new byte[Size];
        }
    }
}
