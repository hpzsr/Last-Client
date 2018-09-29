using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerConfig
{
    public static List<ServerData> listData = new List<ServerData>();

    public static void init(string data)
    {
        listData.Clear();

        listData = JsonConvert.DeserializeObject<List<ServerData>>(data);

        LoginScript.init();
    }

    public static ServerData getServerDataByIndex(int index)
    {
        ServerData temp = null;

        if ((index + 1) <= listData.Count)
        {
            temp = listData[index];
        }

        return temp;
    }
}

public class ServerData
{
    public int state;       // 1流畅  2爆满  3维护
    public string name;
    public string ip;
    public int port;
}
