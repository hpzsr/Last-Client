using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

class HobgoblinData
{
    public int HP_All = 20;
    public int HP_Cur = 20;
    public int Atk = 5;
    public float Speed = 0.005f;
    public float AttackRange = 3;
    public float AttackDur = 3.0f;
}

public class HobgoblinScript : MonoBehaviour {

    Animator Animator = null;
    CharacterController CharacterController = null;

    bool m_canAttack = true;

    GameObject m_curAttackTarget = null;

    HobgoblinData hobgoblinData = new HobgoblinData();
    BloodBarScript bloodBarScript;

    bool isDie = false;

    // Use this for initialization
    void Start ()
    {
        Animator = gameObject.GetComponent<Animator>();
        CharacterController = gameObject.GetComponent<CharacterController>();
    }

    public void addBloodBar()
    {
        bloodBarScript = BloodBarScript.Create(GameObject.Find("LowCanvas").transform, transform.Find("BloodPoint").gameObject, false);
    }

    // Update is called once per frame
    void Update ()
    {
        // 在攻击范围内寻找攻击的对象
        {
            if ((gameObject == null) || (GameScript.s_script.HeroList == null))
            {
                return;
            }

            GameObject attackTarget = CommonUtil.minDistance(gameObject, GameScript.s_script.HeroList);
            if (attackTarget != null)
            {
                if (CommonUtil.TwoPointDistance3D(gameObject.transform.position, attackTarget.transform.position) <= hobgoblinData.AttackRange)
                {
                    if (m_canAttack)
                    {
                        if (attackTarget.tag.CompareTo("Hero") == 0)
                        {
                            // 让英雄面向敌人
                            {
                                float angle = CommonUtil.TwoPointAngle(new Vector2(gameObject.transform.position.x, gameObject.transform.position.z), new Vector2(attackTarget.transform.position.x, attackTarget.transform.position.z));
                                gameObject.transform.localRotation = Quaternion.Euler(0, angle, 0);
                            }

                            m_curAttackTarget = attackTarget;
                            Attack();
                        }
                    }
                }
                // 如果有敌方小兵距离自己3倍攻击范围之内的，则主动向这个小兵方向移动
                else if (CommonUtil.TwoPointDistance3D(gameObject.transform.position, attackTarget.transform.position) <= (hobgoblinData.AttackRange * 2))
                {
                    // 让小兵面向敌人移动
                    {
                        float angle = CommonUtil.TwoPointAngle(new Vector2(gameObject.transform.position.x, gameObject.transform.position.z), new Vector2(attackTarget.transform.position.x, attackTarget.transform.position.z));
                        Move(angle);
                    }
                }
                else
                {
                    Idle();
                }
            }
        }
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
            if (animString.Substring(0, 4).CompareTo("idle") != 0)
            {
                return;
            }
        }

        transform.rotation = Quaternion.Euler(0, angle, 0);

        ShowAnimation("run");
        CharacterController.Move(gameObject.transform.forward * (Time.deltaTime * hobgoblinData.Speed * 2 / (1.0f / 60.0f)) * 4);
    }

    public void Idle()
    {
        ShowAnimation("idle01");
    }

    public void Attack()
    {
        ShowAnimation("attack02");

        m_canAttack = false;
        StartCoroutine(CanAttack());
    }

    public bool Damage(int hitValue)
    {
        Debug.Log("Damage");
        // 动画
        {
            ShowAnimation("idle01");
            Animator.Update(0);
            ShowAnimation("damage");
        }

        // 逻辑
        {
            hobgoblinData.HP_Cur -= hitValue;
            if (hobgoblinData.HP_Cur < 0)
            {
                hobgoblinData.HP_Cur = 0;
            }

            bloodBarScript.setPercent((float)hobgoblinData.HP_Cur / (float)hobgoblinData.HP_All * 100);

            if (hobgoblinData.HP_Cur <= 0)
            {
                return true;
            }
        }

        return false;
    }

    public void Die()
    {
        ShowAnimation("dead");
        isDie = true;

        GameObject.Destroy(bloodBarScript.gameObject);
    }

    IEnumerator CanAttack()
    {
        yield return new WaitForSeconds(hobgoblinData.AttackDur);
        m_canAttack = true;
    }

    public void onAttackEnd()
    {
        Idle();
    }

    public void onTriggerAttack()
    {
        // 击杀
        if (m_curAttackTarget.GetComponent<HeroScript>().Damage(hobgoblinData.Atk))
        {
            m_curAttackTarget.GetComponent<HeroScript>().Die();
            GameScript.s_script.HeroList.Remove(m_curAttackTarget);
        }
    }
}
