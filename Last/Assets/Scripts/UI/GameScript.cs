using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScript : MonoBehaviour {

    public static GameScript s_script;

    public GameObject GameObj = null;
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

            GameObj = GameObject.Find("Game").gameObject;
            AttackBtnList = transform.Find("AttackBtnList").gameObject;
            GameUI = transform.Find("UI").gameObject;
        }

        // 
        {
            GameObject obj = Resources.Load("Prefabs/Role/Hoshi") as GameObject;
            GameObject role = Instantiate(obj, GameObject.Find("Game").transform);
            role.transform.localPosition = new Vector3(-7.44f, 0, -7.2f);
            role.transform.localScale = new Vector3(3, 3, 3);
            role.transform.localRotation = Quaternion.Euler(0, 90, 0);

            HeroList.Add(role);
            heroScript = role.GetComponent<HeroScript>();
            heroScript.addBloodBar();
        }

        // 
        {
            GameObject obj = Resources.Load("Prefabs/Role/Hobgoblin") as GameObject;
            GameObject role = Instantiate(obj, GameObject.Find("Game").transform);
            role.transform.localPosition = new Vector3(8.1f, 0, -7.2f);
            role.transform.localScale = new Vector3(0.22f, 0.22f, 0.22f);
            role.transform.localRotation = Quaternion.Euler(0, -90, 0);

            HobgoblinList.Add(role);
            role.GetComponent<HobgoblinScript>().addBloodBar();
        }
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
        heroScript.Move(angle);
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
