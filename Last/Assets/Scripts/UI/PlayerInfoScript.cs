using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoScript : MonoBehaviour {

    public static GameObject s_gameObject;
    public static PlayerInfoScript s_playerInfoScript;

    GameObject BagContent;
    GameObject EquipItems;
    Text ZhanLi;

    public static void show()
    {
        GameObject obj = Resources.Load("Prefabs/UI/UIPlayerInfo") as GameObject;
        s_gameObject = Instantiate(obj, GameObject.Find("LowCanvas").transform);
        s_playerInfoScript = s_gameObject.GetComponent<PlayerInfoScript>();
    }

    public static void close()
    {
        Destroy(s_gameObject);
    }

    // Use this for initialization
    void Start ()
    {
        // 背包
        {
            BagContent = transform.Find("Image_bg/Bag/Scroll View/Viewport/Content").gameObject;

            for (int i = 0; i < (6 * 7); i++)
            {
                GameObject prefab = Resources.Load("Prefabs/Item/BagItem") as GameObject;
                GameObject item = Instantiate(prefab, BagContent.transform);
                item.transform.name = i.ToString();
            }
        }

        // 装备
        {
            EquipItems = transform.Find("Image_bg/EquipItems").gameObject;
            ZhanLi = transform.Find("Image_bg/zhanli/Text").GetComponent<Text>();

            initEquip();
            
            EquipItems.transform.Find("Equip_wuqi/Button").GetComponent<Button>().onClick.AddListener(()=>
            {
                PlayerData.UserInfoData.BagList.Add(101);
                PlayerData.UserInfoData.EquipList.Remove(101);
                initBag();
                initEquip();

                reqChangeEquip();
            });

            EquipItems.transform.Find("Equip_yifu/Button").GetComponent<Button>().onClick.AddListener(() =>
            {
                PlayerData.UserInfoData.BagList.Add(102);
                PlayerData.UserInfoData.EquipList.Remove(102);
                initBag();
                initEquip();

                reqChangeEquip();
            });

            EquipItems.transform.Find("Equip_xiezi/Button").GetComponent<Button>().onClick.AddListener(() =>
            {
                PlayerData.UserInfoData.BagList.Add(103);
                PlayerData.UserInfoData.EquipList.Remove(103);
                initBag();
                initEquip();

                reqChangeEquip();
            });

            EquipItems.transform.Find("Equip_maozi/Button").GetComponent<Button>().onClick.AddListener(() =>
            {
                PlayerData.UserInfoData.BagList.Add(104);
                PlayerData.UserInfoData.EquipList.Remove(104);
                initBag();
                initEquip();

                reqChangeEquip();
            });

            EquipItems.transform.Find("Equip_xianglian/Button").GetComponent<Button>().onClick.AddListener(() =>
            {
                PlayerData.UserInfoData.BagList.Add(105);
                PlayerData.UserInfoData.EquipList.Remove(105);
                initBag();
                initEquip();

                reqChangeEquip();
            });

            EquipItems.transform.Find("Equip_huwan/Button").GetComponent<Button>().onClick.AddListener(() =>
            {
                PlayerData.UserInfoData.BagList.Add(106);
                PlayerData.UserInfoData.EquipList.Remove(106);
                initBag();
                initEquip();

                reqChangeEquip();
            });
        }

        transform.Find("Image_bg/Button_close").GetComponent<Button>().onClick.AddListener(() =>
        {
            close();
        });

        initEquip();
        initBag();
    }

    void initBag()
    {
        for (int i = 0; i < PlayerData.UserInfoData.BagList.Count; i++)
        {
            CommonUtil.setImageSprite(BagContent.transform.GetChild(i).Find("Button").GetComponent<Image>(), "Sprites/PlayerInfo/icon_" + PlayerData.UserInfoData.BagList[i]);

            {
                int id = PlayerData.UserInfoData.BagList[i];
                Button btn = BagContent.transform.GetChild(i).Find("Button").GetComponent<Button>();
                btn.transform.localScale = new Vector3(1,1,1);
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener((() =>
                {
                    PlayerData.UserInfoData.BagList.Remove(id);
                    PlayerData.UserInfoData.EquipList.Add(id);

                    initEquip();
                    initBag();

                    reqChangeEquip();
                }));
            }
        }

        for (int i = PlayerData.UserInfoData.BagList.Count; i < 6 * 7; i++)
        {
            Button btn = BagContent.transform.GetChild(i).Find("Button").GetComponent<Button>();
            btn.transform.localScale = new Vector3(0,0,0);
        }
    }

    void initEquip()
    {
        // 战力
        {
            PlayerData.ZhanLi = 100 + PlayerData.UserInfoData.EquipList.Count * 1000;
            ZhanLi.text = PlayerData.ZhanLi.ToString();
        }

        EquipItems.transform.Find("Equip_wuqi/Button").transform.localScale = new Vector3(0,0,0);
        EquipItems.transform.Find("Equip_yifu/Button").transform.localScale = new Vector3(0, 0, 0);
        EquipItems.transform.Find("Equip_xiezi/Button").transform.localScale = new Vector3(0, 0, 0);
        EquipItems.transform.Find("Equip_maozi/Button").transform.localScale = new Vector3(0, 0, 0);
        EquipItems.transform.Find("Equip_xianglian/Button").transform.localScale = new Vector3(0, 0, 0);
        EquipItems.transform.Find("Equip_huwan/Button").transform.localScale = new Vector3(0, 0, 0);

        for (int i = 0; i < PlayerData.UserInfoData.EquipList.Count; i++)
        {
            int id = PlayerData.UserInfoData.EquipList[i];
            switch (id)
            {
                // 武器
                case 101:
                    {
                        EquipItems.transform.Find("Equip_wuqi/Button").transform.localScale = new Vector3(1,1,1);
                        CommonUtil.setImageSprite(EquipItems.transform.Find("Equip_wuqi/Button").GetComponent<Image>(), "Sprites/PlayerInfo/icon_" + id);
                    }
                    break;

                // 衣服
                case 102:
                    {
                        EquipItems.transform.Find("Equip_yifu/Button").transform.localScale = new Vector3(1, 1, 1);
                        CommonUtil.setImageSprite(EquipItems.transform.Find("Equip_yifu/Button").GetComponent<Image>(), "Sprites/PlayerInfo/icon_" + id);
                    }
                    break;

                // 鞋子
                case 103:
                    {
                        EquipItems.transform.Find("Equip_xiezi/Button").transform.localScale = new Vector3(1, 1, 1);
                        CommonUtil.setImageSprite(EquipItems.transform.Find("Equip_xiezi/Button").GetComponent<Image>(), "Sprites/PlayerInfo/icon_" + id);
                    }
                    break;

                // 帽子
                case 104:
                    {
                        EquipItems.transform.Find("Equip_maozi/Button").transform.localScale = new Vector3(1, 1, 1);
                        CommonUtil.setImageSprite(EquipItems.transform.Find("Equip_maozi/Button").GetComponent<Image>(), "Sprites/PlayerInfo/icon_" + id);
                    }
                    break;

                // 项链
                case 105:
                    {
                        EquipItems.transform.Find("Equip_xianglian/Button").transform.localScale = new Vector3(1, 1, 1);
                        CommonUtil.setImageSprite(EquipItems.transform.Find("Equip_xianglian/Button").GetComponent<Image>(), "Sprites/PlayerInfo/icon_" + id);
                    }
                    break;

                // 护腕
                case 106:
                    {
                        EquipItems.transform.Find("Equip_huwan/Button").transform.localScale = new Vector3(1, 1, 1);
                        CommonUtil.setImageSprite(EquipItems.transform.Find("Equip_huwan/Button").GetComponent<Image>(), "Sprites/PlayerInfo/icon_" + id);
                    }
                    break;
            }
        }
    }

    public void reqChangeEquip()
    {
        C2S_ChangeEquip c2s = new C2S_ChangeEquip();
        c2s.Tag = (int)Consts.NetTag.ChangeEquip;
        c2s.DeviceId = PlayerData.UserInfoData.Id;
        c2s.EquipList = PlayerData.UserInfoData.EquipList;
        c2s.BagList = PlayerData.UserInfoData.BagList;

        Socket_C.getInstance().Send(c2s);
    }
}
