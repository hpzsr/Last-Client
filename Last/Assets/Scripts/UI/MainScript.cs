using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainScript : MonoBehaviour
{
    static GameObject s_gameObject;
    public static MainScript s_script;

    public static void show()
    {
        GameObject obj = Resources.Load("Prefabs/UI/UIMain") as GameObject;
        s_gameObject = Instantiate(obj, GameObject.Find("LowCanvas").transform);
        s_script = s_gameObject.GetComponent<MainScript>();
    }

    public static void close()
    {
        Destroy(s_gameObject);
    }

    // Use this for initialization
    void Start ()
    {
        gameObject.transform.Find("BtnList/Button_sign").GetComponent<Button>().onClick.AddListener(() =>
        {
            SignScript.show();
        });

        gameObject.transform.Find("BtnList/Button_activity").GetComponent<Button>().onClick.AddListener(()=> 
        {
            ToastScript.createToast("暂无活动");
        });

        gameObject.transform.Find("BtnList/Button_bag").GetComponent<Button>().onClick.AddListener(() =>
        {
            ToastScript.createToast("背包");
        });

        gameObject.transform.Find("BtnList/Button_shop").GetComponent<Button>().onClick.AddListener(() =>
        {
            ToastScript.createToast("商城");
        });

        gameObject.transform.Find("BtnList/Button_task").GetComponent<Button>().onClick.AddListener(() =>
        {
            ToastScript.createToast("任务");
        });

        gameObject.transform.Find("BtnList/Button_jingji").GetComponent<Button>().onClick.AddListener(() =>
        {
            ToastScript.createToast("竞技");
        });

        gameObject.transform.Find("BtnList/Button_zhanyi").GetComponent<Button>().onClick.AddListener(() =>
        {
            //SceneManager.LoadScene("GameScene");
            SceneManager.LoadScene("DiLaoScene");
        });

        gameObject.transform.Find("Head/Button").GetComponent<Button>().onClick.AddListener(() =>
        {
            PlayerInfoScript.show();
        });

        reqUserInfo();
    }
	
	// Update is called once per frame
	void Update ()
    {
	}

    public void refreshUI()
    {
        gameObject.transform.Find("Head/Gold/Text").GetComponent<Text>().text = PlayerData.UserInfoData.Gold.ToString();
    }

    public void reqUserInfo()
    {
        C2S_UserInfo c2s = new C2S_UserInfo();
        c2s.Tag = (int)CSParam.NetTag.UserInfo;
        c2s.DeviceId = PlayerData.UserInfoData.Id;

        Socket_C.getInstance().Send(c2s);
    }
}
