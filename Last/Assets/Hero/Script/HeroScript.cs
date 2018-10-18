using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroData
{
    public int HP_All;
    public int HP_Cur;
    public int Atk;
    public float Speed;
    public float AttackRange;

    public HeroData()
    {
        HP_All = 500;
        HP_Cur = HP_All;
        Atk = 5;
        Speed = 0.015f;
        AttackRange = 4;
    }
}

public class HeroScript : MonoBehaviour {

    Animator Animator = null;
    CharacterController CharacterController = null;
    public HeroData heroData = new HeroData();
    BloodBarScript bloodBarScript;

    bool isDie = false;

    // Use this for initialization
    void Start ()
    {
        Animator = gameObject.GetComponent<Animator>();
        CharacterController = gameObject.GetComponent<CharacterController>();
    }
	
	// Update is called once per frame
	void LateUpdate ()
    {
        if (!CharacterController.isGrounded)
        {
            CharacterController.Move(Vector3.down * 10.0f);
        }
    }

    public void addBloodBar()
    {
        bloodBarScript = BloodBarScript.Create(GameObject.Find("LowCanvas").transform, transform.Find("BloodPoint").gameObject, true);
    }

    public void ShowAnimation(string name)
    {
        if (isDie)
        {
            return;
        }

        Animator.Play(name);
    }

    public void Move(float angle)
    {
        string animString = Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        if (animString.Length >= 4)
        {
            if (animString.Substring(0, 4).CompareTo("Idle") != 0)
            {
                return;
            }
        }

        transform.rotation = Quaternion.Euler(0, angle, 0);

        ShowAnimation("Run");
        CharacterController.Move(gameObject.transform.forward * (Time.deltaTime * heroData.Speed * 2 / (1.0f / 60.0f)) * 4);
    }

    public void FlashMove()
    {
        CharacterController.Move(gameObject.transform.forward * 4);
    }

    public void Idle()
    {
        ShowAnimation("Idle_A");
    }
    
    public bool Damage(int hitValue)
    {
        if (isDie)
        {
            return false;
        }

        // 动画
        {
            Animator.Play("Idle_A");
            Animator.Update(0);
            Animator.Play("Damage");
        }

        // 逻辑
        {
            heroData.HP_Cur -= hitValue;
            if (heroData.HP_Cur < 0)
            {
                heroData.HP_Cur = 0;
            }

            bloodBarScript.setPercent((float)heroData.HP_Cur / (float)heroData.HP_All * 100);

            if (heroData.HP_Cur <= 0)
            {
                return true;
            }
        }

        return false;
    }

    public void Attack()
    {
        if (isDie)
        {
            return;
        }

        ShowAnimation("Atk3");

        // 在攻击范围内寻找攻击的对象
        {
            GameObject attackTarget = CommonUtil.minDistance(gameObject, GameScript.s_script.HobgoblinList);
            if (attackTarget != null)
            {
                if (CommonUtil.TwoPointDistance3D(gameObject.transform.position, attackTarget.transform.position) <= heroData.AttackRange)
                {
                    if (attackTarget.tag.CompareTo("Hobgoblin") == 0)
                    {
                        // 击杀
                        if (attackTarget.GetComponent<HobgoblinScript>().Damage(heroData.Atk))
                        {
                            attackTarget.GetComponent<HobgoblinScript>().Die();
                            GameScript.s_script.HobgoblinList.Remove(attackTarget);
                        }
                    }
                }
            }
        }
    }

    public void Skill1()
    {
        ShowAnimation("Atk2");
    }

    public void Skill2()
    {
        ShowAnimation("Atk1");
    }

    public void Skill3()
    {
        ShowAnimation("Atk4");
    }

    public void Die()
    {
        if (isDie)
        {
            return;
        }

        ShowAnimation("DieA");
        isDie = true;

        GameObject.Destroy(bloodBarScript.gameObject);
    }

    public void onAttackEnd()
    {
        Idle();
    }

    public void onSkill1End()
    {
        Idle();
    }

    public void onSkill2End()
    {
        Idle();
    }

    public void onSkill3End()
    {
        Idle();
    }

    public void onDamageEnd()
    {
        Idle();
    }
}
