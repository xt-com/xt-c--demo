using System;
using System.Globalization;
using System.Reflection;
using System.Security.Authentication;
using System.Timers;
using WebSocketSharp;

namespace ApiDemo
{
    public class XWebsocket
    {
        /// <summary>
        /// true:open，
        /// false:closed
        /// </summary>
        public event Action<bool> StateChanged;

        /// <summary>
        ///  
        /// </summary>
        public event Action<string> DataReceived;


        public WebSocketState WsState
        {
            get
            {
                if (m_webSocket != null)
                    return m_webSocket.ReadyState;
                else
                    return WebSocketState.Closed;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public AutoConnectionMode AutoConnection { get; set; } = AutoConnectionMode.Linear;

        #region private field

        private const int BaseReconnectDelay = 2;

        private const int MaxReconnectDelay = 256;

        private const int GuardTimerInterval = 2;

        private string m_name;
        private string m_url;
        private WebSocket m_webSocket;
        private Timer m_guardTimer;

        /// <summary>
        /// 
        /// </summary>
        private long m_guardTimerTick = 0;

        private int m_nextReconnectDelay = BaseReconnectDelay;

        private long m_lastReconnectTick = 0;

        private readonly object socketLock = new object();

        #endregion

        public XWebsocket(string name)
        {
            m_name = name;
            m_guardTimer = new Timer(GuardTimerInterval * 1000);
            m_guardTimer.Elapsed += OnGuardTimerElapsed;
        }

        public void ConnectAsync(string url)
        {
            if (m_webSocket != null)
            {
                return;
            }

            m_url = url;
            m_webSocket = new WebSocket(m_url);

         
            //SSL
            var sslProtocols = (SslProtocols)(SslProtocolsHack.Tls12 | SslProtocolsHack.Tls11 | SslProtocolsHack.Tls);
            m_webSocket.SslConfiguration.EnabledSslProtocols = sslProtocols;


            // 开始连接
            m_webSocket.OnOpen += OnSocketOpen;
            m_webSocket.OnMessage += OnSocketMessage;
            m_webSocket.OnError += OnSocketError;
            m_webSocket.OnClose += OnSocketClose;

            lock (socketLock)
            {
                m_webSocket.ConnectAsync();

                m_guardTimer.Start();
                m_guardTimerTick = 0;
            }
        }

        public void Connect(string url)
        {
            if (m_webSocket != null)
            {
                return;
            }

            m_url = url;
            m_webSocket = new WebSocket(m_url);

         

            // 
            var sslProtocols = (SslProtocols)(SslProtocolsHack.Tls12 | SslProtocolsHack.Tls11 | SslProtocolsHack.Tls);
            m_webSocket.SslConfiguration.EnabledSslProtocols = sslProtocols;

            m_webSocket.Connect();
        }



        public void CloseAsync()
        {
            if (m_webSocket == null)
            {
                return;
            }

            lock (socketLock)
            {
                var socket = m_webSocket;
                m_webSocket = null;

                m_guardTimer.Stop();
                m_guardTimerTick = 0;

                socket.OnOpen -= OnSocketOpen;
                socket.OnMessage -= OnSocketMessage;
                socket.OnError -= OnSocketError;
                socket.OnClose -= OnSocketClose;
                socket.CloseAsync();
                StateChanged?.BeginInvoke(false, null, null);
            }
        }

        public void SendAsync(string data, Action<bool> completed)
        {
            this.m_webSocket?.SendAsync(data, completed);
        }
        public void Send(string data)
        {
            if (this.m_webSocket.ReadyState == WebSocketState.Open)
            {
                this.m_webSocket?.Send(data);
            }
          
        }
        public void SendAsync(byte[] data, Action<bool> completed)
        {
            this.m_webSocket?.SendAsync(data, completed);
        }


        private void OnGuardTimerElapsed(object sender, ElapsedEventArgs e)
        {
            m_guardTimerTick += GuardTimerInterval;

            
            if (m_webSocket != null && m_webSocket.ReadyState == WebSocketState.Closed)
            {
                if (m_guardTimerTick - m_lastReconnectTick >= m_nextReconnectDelay)
                {


                    m_webSocket.ConnectAsync();

                 
                    m_lastReconnectTick = m_guardTimerTick;


                 
                    if (AutoConnection == AutoConnectionMode.Linear)
                    {
                        m_nextReconnectDelay += 2;
                    }
                    else
                    {
                        m_nextReconnectDelay *= 2;
                    }
                    if (m_nextReconnectDelay > MaxReconnectDelay)
                    {
                        m_nextReconnectDelay = BaseReconnectDelay;
                    }
                  
                }
            }

        }

        private void OnSocketOpen(object sender, EventArgs e)
        {
         

            if (sender == m_webSocket)
            {
               
                m_nextReconnectDelay = BaseReconnectDelay;

                StateChanged?.BeginInvoke(true, null, null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSocketClose(object sender, CloseEventArgs e)
        {
        
            if (sender == m_webSocket)
            {
               
                m_lastReconnectTick = m_guardTimerTick;
                StateChanged?.BeginInvoke(false, null, null);
            }
        }

        private void OnSocketError(object sender, ErrorEventArgs e)
        {
         //log

        }

        private void OnSocketMessage(object sender, MessageEventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("EN-US");
          
            DataReceived?.Invoke(e.Data);
            
        }


    }

    public enum SslProtocolsHack
    {
        Tls = 192,
        Tls11 = 768,
        Tls12 = 3072
    }

    public enum AutoConnectionMode
    {
        /// <summary>
        /// 
        /// </summary>
        Linear,

        /// <summary>
        /// 
        /// </summary>
        Exponential
    }
}
