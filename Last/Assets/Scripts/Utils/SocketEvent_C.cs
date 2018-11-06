using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class SocketEvent_C
{
    public static void OnReceive(string data)
    {
        try
        {
            Debug.Log("收到服务端消息：" + data + "    size=" + data.Length);

            try
            {
                CSBaseData csBaseData = JsonConvert.DeserializeObject<CSBaseData>(data);

                switch (csBaseData.Tag)
                {
                    case (int)CSParam.NetTag.Login:
                        {
                            S2C_Login s2c = JsonConvert.DeserializeObject<S2C_Login>(data);
                            PlayerData.UserInfoData.Id = s2c.Id;

                            LoginScript.close();
                            MainScript.show();
                        }
                        break;

                    case (int)CSParam.NetTag.UserInfo:
                        {
                            S2C_UserInfo s2c = JsonConvert.DeserializeObject<S2C_UserInfo>(data);
                            PlayerData.UserInfoData = s2c.UserInfoData;
                            MainScript.s_script.refreshUI();
                        }
                        break;

                    case (int)CSParam.NetTag.ChangeEquip:
                        {
                            
                        }
                        break;

                    case (int)CSParam.NetTag.Sign:
                        {
                            S2C_Sign s2c = JsonConvert.DeserializeObject<S2C_Sign>(data);
                            PlayerData.UserInfoData.Gold += CommonUtil.splitStr_End(s2c.Reward, ':');

                            MainScript.s_script.refreshUI();

                            //SignScript.close();
                            ShowRewardUtil.Show(s2c.Reward);
                        }
                        break;

                    default:
                        {
                            Debug.Log("未知tag，不予处理：" + data);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.Log("服务端传的数据异常：" + ex + "内容：" + data);
            }
        }
        catch (Exception ex)
        {
            Debug.Log("OnReceive:" + ex);
        }
    }

    public static void OnConnect(bool result)
    {
        if (result)
        {
            LoginScript.s_loginScript.reqLogin();
        }
        else
        {
            ToastScript.createToast("连接服务器失败");
        }
    }
}