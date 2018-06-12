﻿using System;
using System.Collections.Generic;
using System.Net;

namespace Sodao.FastSocket.Server
{
    /// <summary>
    /// socket server.
    /// </summary>
    /// <typeparam name="TCommandInfo"></typeparam>
    public class SocketServer<TCommandInfo> : AbsSocketServer where TCommandInfo : class, Command.ICommandInfo
    {
        #region Private Members
        private readonly List<SocketListener> _listListener = new List<SocketListener>();
        private readonly AbsSocketService<TCommandInfo> _socketService = null;
        private readonly Protocol.IProtocol<TCommandInfo> _protocol = null;
        private readonly int _maxMessageSize;
        private readonly int _maxConnections;
        #endregion

        #region Constructors
        /// <summary>
        /// new
        /// </summary>
        /// <param name="socketService"></param>
        /// <param name="protocol"></param>
        /// <param name="socketBufferSize"></param>
        /// <param name="messageBufferSize"></param>
        /// <param name="maxMessageSize"></param>
        /// <param name="maxConnections"></param>
        /// <exception cref="ArgumentNullException">socketService is null.</exception>
        /// <exception cref="ArgumentNullException">protocol is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">maxMessageSize</exception>
        /// <exception cref="ArgumentOutOfRangeException">maxConnections</exception>
        public SocketServer(AbsSocketService<TCommandInfo> socketService,
            Protocol.IProtocol<TCommandInfo> protocol,
            int socketBufferSize,
            int messageBufferSize,
            int maxMessageSize,
            int maxConnections)
            : base(socketBufferSize, messageBufferSize)
        {
            if (socketService == null) throw new ArgumentNullException("socketService");
            if (protocol == null) throw new ArgumentNullException("protocol");
            if (maxMessageSize < 1) throw new ArgumentOutOfRangeException("maxMessageSize");
            if (maxConnections < 1) throw new ArgumentOutOfRangeException("maxConnections");

            this._socketService = socketService;
            this._protocol = protocol;
            this._maxMessageSize = maxMessageSize;
            this._maxConnections = maxConnections;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// socket accepted handler
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="connection"></param>
        private void listener_Accepted(ISocketListener listener, SocketBase.IConnection connection)
        {
            if (base.CountConnection() > this._maxConnections)
            {
                connection.BeginDisconnect();
                return;
            }
            base.RegisterConnection(connection);
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// start
        /// </summary>
        public override void Start()
        {
            foreach (var child in this._listListener) child.Start();
        }
        /// <summary>
        /// stop
        /// </summary>
        public override void Stop()
        {
            foreach (var child in this._listListener) child.Stop();
            base.Stop();
        }
        /// <summary>
        /// add socket listener
        /// </summary>
        /// <param name="name"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public override ISocketListener AddListener(string name, IPEndPoint endPoint)
        {
            var listener = new SocketListener(name, endPoint, this);
            this._listListener.Add(listener);
            listener.Accepted += this.listener_Accepted;
            return listener;
        }
        /// <summary>
        /// OnConnected
        /// </summary>
        /// <param name="connection"></param>
        protected override void OnConnected(SocketBase.IConnection connection)
        {
            base.OnConnected(connection);
            this._socketService.OnConnected(connection);
        }
        /// <summary>
        /// send callback
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="packet"></param>
        /// <param name="isSuccess"></param>
        protected override void OnSendCallback(SocketBase.IConnection connection, SocketBase.Packet packet, bool isSuccess)
        {
            base.OnSendCallback(connection, packet, isSuccess);
            this._socketService.OnSendCallback(connection, packet, isSuccess);
        }
        /// <summary>
        /// OnMessageReceived
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="e"></param>
        protected override void OnMessageReceived(SocketBase.IConnection connection, SocketBase.MessageReceivedEventArgs e)
        {
            base.OnMessageReceived(connection, e);

            int readlength;
            TCommandInfo cmdInfo = null;
            try { cmdInfo = this._protocol.FindCommandInfo(connection, e.Buffer, this._maxMessageSize, out readlength); }
            catch (Exception ex)
            {
                this.OnConnectionError(connection, ex);
                connection.BeginDisconnect(ex);
                e.SetReadlength(e.Buffer.Count);
                return;
            }

            if (cmdInfo != null) this._socketService.OnReceived(connection, cmdInfo);
            e.SetReadlength(readlength);
        }
        /// <summary>
        /// OnDisconnected
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="ex"></param>
        protected override void OnDisconnected(SocketBase.IConnection connection, Exception ex)
        {
            base.OnDisconnected(connection, ex);
            this._socketService.OnDisconnected(connection, ex);
        }
        /// <summary>
        /// on connection error
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="ex"></param>
        protected override void OnConnectionError(SocketBase.IConnection connection, Exception ex)
        {
            base.OnConnectionError(connection, ex);
            this._socketService.OnException(connection, ex);
        }
        #endregion
    }
}