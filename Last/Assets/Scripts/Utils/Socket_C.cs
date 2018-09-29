using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Socket_C : MonoBehaviour
{
    static GameObject s_socketObj = null;
    static Socket_C s_Socket_C = null;

    public delegate void OnSocketEvent_Connect(bool result);    // 连接服务器结果
    public delegate void OnSocketEvent_Receive(string data);    // 收到服务器消息
    public delegate void OnSocketEvent_Close();                 // 与服务器非正常断开连接

    public OnSocketEvent_Connect m_onSocketEvent_Connect = null;
    public OnSocketEvent_Receive m_onSocketEvent_Receive = null;
    public OnSocketEvent_Close m_onSocketEvent_Close = null;

    public Socket m_socket = null;
    public string m_ipAddress = null;
    public string m_yuming;
    public int m_ipPort = 0;

    public bool m_isStart = false;

    // 数据包尾部标识
    public char m_packEndFlag = (char) 1;
    public string m_endStr = "";

    // 心跳包标记
    public string m_heartBeatFlag = "*HeartBeat*";
    public float m_heartBeatTime = 2.0f;

    int byteLength = 1024;

    List<string> ServerDataList = new List<string>();

    public static Socket_C getInstance()
    {
        try
        {
            if (!s_socketObj)
            {
                s_socketObj = new GameObject();
                s_socketObj.transform.name = "SocketObj";
                MonoBehaviour.DontDestroyOnLoad(s_socketObj);
                s_Socket_C = s_socketObj.AddComponent<Socket_C>();
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Socket_C----" + ex);
        }

        return s_Socket_C;
    }

    public void Start()
    {

    }

    public void OnDestroy()
    {
        Stop();
    }

    void Update()
    {
        if (ServerDataList.Count > 0)
        {
            string data = ServerDataList[0];

            if (m_onSocketEvent_Receive != null)
            {
                m_onSocketEvent_Receive(data);
                ServerDataList.RemoveAt(0);
            }
        }
    }

    public void Start(string ip, string yuming, int port)
    {
        if (!m_isStart)
        {
            m_ipAddress = ip;
            m_yuming = yuming;
            m_ipPort = port;

            Connect();
        }
        else
        {
            Debug.Log("Socket_C----连接服务器失败，因为当前已经连接  " + m_ipAddress.ToString() + "  " + m_ipPort);
        }
    }

    public void Stop()
    {
        if (m_isStart)
        {
            m_isStart = false;

            if (m_socket != null)
            {
                m_socket.Close();

                Debug.Log("Socket_C----主动与服务器断开连接");
            }
        }
        else
        {
            Debug.Log("Socket_C----主动与服务器断开连接失败，因为当前已经断开  " + m_ipAddress.ToString() + "  " + m_ipPort);
        }
    }

    public void Connect()
    {
        try
        {

            IPAddress ip;

            if (string.IsNullOrEmpty(m_yuming))
            {
                ip = IPAddress.Parse(m_ipAddress);
            }
            else
            {
                IPHostEntry IPinfo = Dns.GetHostEntry(m_yuming);
                if (IPinfo.AddressList.Length <= 0)
                {
                    ToastScript.createToast("域名解析出错");
                    return;
                }

                ip = IPinfo.AddressList[0];
            }            

            Debug.Log("Socket_C----ip = " + ip.ToString() + "    port = " + m_ipPort);

            IPEndPoint ipEndPort = new IPEndPoint(ip, m_ipPort);
            if (ip.AddressFamily.CompareTo(AddressFamily.InterNetworkV6) == 0)
            {
                m_socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            }
            else
            {
                m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }

            m_socket.Connect(ipEndPort);
            m_isStart = true;

            if (m_onSocketEvent_Connect != null)
            {
                m_onSocketEvent_Connect(true);
            }

            Debug.Log("Socket_C----连接服务器成功...");

            Task t = new Task(() => { StartReceive(); });
            t.Start();

            // 开始心跳包
            {
                StopHeartBeat();
                StartHeartBeat();
            }
        }
        catch (SocketException ex)
        {
            Debug.Log("Socket_C----连接服务器失败：" + ex + "  " + m_ipAddress.ToString() + "  " + m_ipPort);

            m_onSocketEvent_Connect(false);
        }
    }

    public void Send(object data)
    {
        if (m_isStart)
        {
            try
            {
                string sendData = "";
                if (data.ToString().CompareTo(m_heartBeatFlag) == 0)
                {
                    sendData = data.ToString();
                }
                else
                {
                    sendData = JsonConvert.SerializeObject(data);
                    Debug.Log("Socket_C----发送给服务端消息：" + sendData + "    size=" + sendData.Length);
                }

                // 增加数据包尾部标识
                sendData += m_packEndFlag;

                byte[] bytes = Encoding.UTF8.GetBytes(sendData);
                m_socket.Send(bytes);
            }
            catch (SocketException ex)
            {
                Debug.Log("Socket_C----与服务端连接断开：" + ex + "  " + m_ipAddress.ToString() + "  " + m_ipPort);

                m_isStart = false;

                if (m_onSocketEvent_Close != null)
                {
                    m_onSocketEvent_Close();
                }
            }
        }
        else
        {
            Debug.Log("Socket_C----发送消息失败：已经与服务端断开  " + m_ipAddress.ToString() + "  " + m_ipPort);
        }
    }

    public void StartReceive()
    {
        while (m_isStart)
        {
            try
            {
                byte[] rece = new byte[byteLength];
                int recelong = m_socket.Receive(rece, rece.Length, 0);
                if (recelong != 0)
                {
                    string reces = Encoding.UTF8.GetString(rece, 0, recelong);

                    reces = m_endStr + reces;

                    List<string> list = new List<string>();
                    bool b = CommonUtil.splitStrIsPerfect(reces, list, m_packEndFlag);

                    if (b)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            ServerDataList.Add(list[i]);
                        }

                        m_endStr = "";
                    }
                    else
                    {
                        for (int i = 0; i < list.Count - 1; i++)
                        {
                            ServerDataList.Add(list[i]);
                        }

                        m_endStr = list[list.Count - 1];
                    }
                }
                // 与服务端断开连接
                else
                {
                    Debug.Log("Socket_C----与服务端断开连接");

                    m_isStart = false;

                    if (m_onSocketEvent_Close != null)
                    {
                        m_onSocketEvent_Close();
                    }

                    return;
                }
            }
            catch (Exception ex)
            {
                Debug.Log("Socket_C----被动与服务端连接断开：" + ex + "  " + m_ipAddress.ToString() + "  " + m_ipPort);

                m_isStart = false;

                if (m_onSocketEvent_Close != null)
                {
                    m_onSocketEvent_Close();
                }

                return;
            }
        }
    }

    void StartHeartBeat()
    {
        InvokeRepeating("OnInvokeHeartBeat", m_heartBeatTime, m_heartBeatTime);
    }

    void OnInvokeHeartBeat()
    {
        if (m_isStart)
        {
            Send(m_heartBeatFlag);
        }
    }

    void StopHeartBeat()
    {
        CancelInvoke("OnInvokeHeartBeat");
    }

    void RetryConnect()
    {
        Start(m_ipAddress,m_yuming,m_ipPort);
    }
}