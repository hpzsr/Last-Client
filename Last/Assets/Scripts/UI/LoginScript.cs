using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginScript : MonoBehaviour {

    public static GameObject s_gameObject;
    public static LoginScript s_loginScript;

    GameObject choiceServer;
    GameObject curServer;
    ServerData serverData;

    public static void show()
    {
        GameObject obj = Resources.Load("Prefabs/UI/UILogin") as GameObject;
        s_gameObject = Instantiate(obj, GameObject.Find("LowCanvas").transform);
        s_loginScript = s_gameObject.GetComponent<LoginScript>();
    }

    public static void close()
    {
        Destroy(s_gameObject);
    }

    public static void init()
    {
        {
            int serverIndex = PlayerPrefs.GetInt("ServerIndex",0);
            s_loginScript.serverData = ServerConfig.listData[serverIndex];
            s_loginScript.setCurServer(s_loginScript.serverData);
        }

        for (int i = 0; i < ServerConfig.listData.Count; i++)
        {
            GameObject prefab = Resources.Load("Prefabs/Item/ServerInfo") as GameObject;
            GameObject item = Instantiate(prefab, s_loginScript.choiceServer.transform.Find("Scroll View/Viewport/Content"));
            item.transform.name = i.ToString();
            item.transform.Find("Text").GetComponent<Text>().text = ServerConfig.listData[i].name;

            switch (ServerConfig.listData[i].state)
            {
                // 流畅
                case 1:
                    {
                        CommonUtil.setImageSprite(item.transform.Find("Image").GetComponent<Image>(), "Sprites/Login/state_green");
                    }
                    break;

                // 爆满
                case 2:
                    {
                        CommonUtil.setImageSprite(item.transform.Find("Image").GetComponent<Image>(), "Sprites/Login/state_red");
                    }
                    break;

                // 维护
                case 3:
                    {
                        CommonUtil.setImageSprite(item.transform.Find("Image").GetComponent<Image>(), "Sprites/Login/state_gray");
                    }
                    break;
            }               

            item.GetComponent<Button>().onClick.AddListener(() =>
            {

                s_loginScript.serverData = ServerConfig.getServerDataByIndex(int.Parse(item.transform.name));
                s_loginScript.choiceServer.transform.localScale = new Vector3(0, 0, 0);

                s_loginScript.setCurServer(s_loginScript.serverData);
            });
        }
    }

    void Start ()
    {
        HttpReqUtil.getInstance().Get("http://fwdown.hy51v.com/Last/ServerConfig.json", ServerConfig.init);

        choiceServer = gameObject.transform.Find("ChoiceServer").gameObject;
        curServer = gameObject.transform.Find("CurServer").gameObject;

        gameObject.transform.Find("Button_start").GetComponent<Button>().onClick.AddListener(() =>
        {
            PlayerPrefs.SetInt("ServerIndex", ServerConfig.listData.IndexOf(serverData));

            Socket_C.getInstance().m_onSocketEvent_Receive = SocketEvent_C.OnReceive;
            Socket_C.getInstance().m_onSocketEvent_Connect = SocketEvent_C.OnConnect;

            Socket_C.getInstance().Start(serverData.ip, "", serverData.port);
        });

        curServer.transform.Find("Button").GetComponent<Button>().onClick.AddListener(() =>
        {
            choiceServer.transform.localScale = new Vector3(1, 1, 1);
        });

        choiceServer.transform.Find("Button_close").GetComponent<Button>().onClick.AddListener(() =>
        {
            choiceServer.transform.localScale = new Vector3(0,0,0);
        });
    }
	
	void Update ()
    {
		
	}

    public void reqLogin()
    {
        C2S_Login c2s = new C2S_Login();
        c2s.Tag = (int)Consts.NetTag.Login;
        c2s.DeviceId = SystemInfo.deviceUniqueIdentifier;

        Socket_C.getInstance().Send(c2s);
    }

    public void setCurServer(ServerData data)
    {
        curServer.transform.Find("Text").GetComponent<Text>().text = data.name;

        switch (data.state)
        {
            // 流畅
            case 1:
                {
                    CommonUtil.setImageSprite(curServer.transform.Find("Image_State").GetComponent<Image>(), "Sprites/Login/state_green");
                    CommonUtil.setFontColor(curServer.transform.Find("Text_State").GetComponent<Text>(),106,184,44);
                    curServer.transform.Find("Text_State").GetComponent<Text>().text = "流畅";
                }
                break;

            // 爆满
            case 2:
                {
                    CommonUtil.setImageSprite(curServer.transform.Find("Image_State").GetComponent<Image>(), "Sprites/Login/state_red");
                    CommonUtil.setFontColor(curServer.transform.Find("Text_State").GetComponent<Text>(), 231, 32, 25);
                    curServer.transform.Find("Text_State").GetComponent<Text>().text = "爆满";
                }
                break;

            // 维护
            case 3:
                {
                    CommonUtil.setImageSprite(curServer.transform.Find("Image_State").GetComponent<Image>(), "Sprites/Login/state_gray");
                    CommonUtil.setFontColor(curServer.transform.Find("Text_State").GetComponent<Text>(), 130, 129, 129);
                    curServer.transform.Find("Text_State").GetComponent<Text>().text = "维护";
                }
                break;
        }
    }
}
