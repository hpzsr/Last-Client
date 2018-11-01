using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScript : MonoBehaviour {

    public static GameScript s_script;

    public GameObject MainCamera;
    public GameObject AttackBtnList = null;
    public GameObject GameUI = null;

    public HeroScript heroScript = null;

    public List<GameObject> HeroList = new List<GameObject>();
    public List<GameObject> HobgoblinList = new List<GameObject>();

    // Use this for initialization
    void Start ()
    {
        init();
        registerEvent();
    }

    void init()
    {
        {
            s_script = transform.GetComponent<GameScript>();

            MainCamera = GameObject.Find("Main Camera");
            AttackBtnList = transform.Find("AttackBtnList").gameObject;
            GameUI = transform.Find("UI").gameObject;
        }

        //// 创建PVP英雄
        //{
        //    GameObject obj = GameUtil.CreatePVPHero();

        //    HeroList.Add(obj);
        //    heroScript = obj.GetComponent<HeroScript>();
        //    heroScript.addBloodBar();
        //}

        // 创建地牢英雄
        {
            GameObject obj = GameUtil.CreateDiLaoHero();
            HeroList.Add(obj);
            heroScript = obj.GetComponent<HeroScript>();
            heroScript.addBloodBar();
            heroScript.heroData.Speed = 0.02f;

            GameObject.Find("Main Camera").GetComponent<TrackGameObjScript>().startTrack(obj);
            GameObject.Find("Lights").GetComponent<TrackGameObjScript>().startTrack(obj);
        }

        // 怪物1
        {
            GameObject obj = GameUtil.CreateEnemy();
            obj.transform.localPosition = new Vector3(11.92f, -4.61f, 20f);

            obj.transform.localRotation = Quaternion.Euler(0, -90, 0);
            HobgoblinList.Add(obj);
            obj.GetComponent<HobgoblinScript>().addBloodBar();
        }

        // 怪物2
        {
            GameObject obj = GameUtil.CreateEnemy();
            obj.transform.localPosition = new Vector3(-11.92f, -4.61f, 20f);
            obj.transform.localRotation = Quaternion.Euler(0, 90, 0);

            HobgoblinList.Add(obj);
            obj.GetComponent<HobgoblinScript>().addBloodBar();
        }
        
        //Debug.Log(Mathf.Sin(Mathf.Deg2Rad * 0));
        //Debug.Log(Mathf.Cos(Mathf.Deg2Rad * 0));
    }

    void registerEvent()
    {
        AttackBtnList.transform.Find("Button_attack").GetComponent<Button>().onClick.AddListener(() =>
        {
            onClickAttack();
        });

        AttackBtnList.transform.Find("Button_skill1").GetComponent<Button>().onClick.AddListener(() =>
        {
            onClickSkill1();
        });

        AttackBtnList.transform.Find("Button_skill2").GetComponent<Button>().onClick.AddListener(() =>
        {
            onClickSkill2();
        });

        AttackBtnList.transform.Find("Button_skill3").GetComponent<Button>().onClick.AddListener(() =>
        {
            onClickSkill3();
        });

        GameUI.transform.Find("Button_exit").GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene("MainScene");
        });

        RockerScript.s_rockerEvent_Move = RockerEvent_Move;
        RockerScript.s_rockerEvent_Reset = RockerEvent_Reset;
    }

	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            heroScript.Attack();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            heroScript.Skill1();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            heroScript.Skill2();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            heroScript.Skill3();
        }
        else if (Input.GetKey(KeyCode.W))
        {
            heroScript.Move(0);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            heroScript.Move(-90);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            heroScript.Move(180);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            heroScript.Move(90);
        }
        else if ((Input.GetKeyUp(KeyCode.W)) || (Input.GetKeyUp(KeyCode.A)) || (Input.GetKeyUp(KeyCode.S)) || (Input.GetKeyUp(KeyCode.D)))
        {
            heroScript.Idle();
        }
    }

    void RockerEvent_Move(float angle)
    {
        /*
         * 为什么要+相机的角度？
         * 因为场景旋转（其实是相机旋转）后，摇杆的方向就跟实际的偏移了
         */
        heroScript.Move(angle + MainCamera.transform.localEulerAngles.y);
    }

    void RockerEvent_Reset()
    {
        heroScript.Idle();
    }

    void onClickAttack()
    {
        heroScript.Attack();
    }

    void onClickSkill1()
    {
        heroScript.Skill1();

        //heroScript.FlashMove();
    }

    void onClickSkill2()
    {
        heroScript.Skill2();
    }

    void onClickSkill3()
    {
        heroScript.Skill3();
    }
}
