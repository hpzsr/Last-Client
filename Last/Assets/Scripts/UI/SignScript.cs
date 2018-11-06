using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignScript : MonoBehaviour {

    public static GameObject s_gameObject;
    public static SignScript s_signScript;

    public static void show()
    {
        GameObject obj = Resources.Load("Prefabs/UI/UISign") as GameObject;
        s_gameObject = Instantiate(obj, GameObject.Find("LowCanvas").transform);
        s_signScript = s_gameObject.GetComponent<SignScript>();
    }

    public static void close()
    {
        Destroy(s_gameObject);
    }
    
    void Start ()
    {
        transform.Find("Image_bg/Button_close").GetComponent<Button>().onClick.AddListener(() =>
        {
            close();
        });

        transform.Find("Image_bg/Button_Get").GetComponent<Button>().onClick.AddListener(() =>
        {
            reqSign();
        });
    }

    public void reqSign()
    {
        C2S_Sign c2s = new C2S_Sign();
        c2s.Tag = (int)CSParam.NetTag.Sign;
        c2s.Id = PlayerData.UserInfoData.Id;

        Socket_C.getInstance().Send(c2s);
    }
}
