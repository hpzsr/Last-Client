using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class GameUtil
{
    public static GameObject CreatePVPHero()
    {
        GameObject obj = Resources.Load("Prefabs/Role/Hoshi") as GameObject;
        GameObject role = GameObject.Instantiate(obj, GameObject.Find("Game").transform);
        role.transform.localPosition = new Vector3(-7.44f, 0, -7.2f);
        role.transform.localScale = new Vector3(3, 3, 3);
        role.transform.localRotation = Quaternion.Euler(0, 90, 0);

        return role;
    }

    public static GameObject CreateDiLaoHero()
    {
        GameObject obj = Resources.Load("Prefabs/Role/Hoshi") as GameObject;
        GameObject role = GameObject.Instantiate(obj, GameObject.Find("Game").transform);
        role.transform.localPosition = new Vector3(0,0,0);
        role.transform.localScale = new Vector3(3, 3, 3);
        role.transform.localRotation = Quaternion.Euler(0, 0, 0);

        return role;
    }

    public static GameObject CreateEnemy()
    {
        GameObject obj = Resources.Load("Prefabs/Role/Hobgoblin") as GameObject;
        GameObject role = GameObject.Instantiate(obj, GameObject.Find("Game").transform);
        role.transform.localScale = new Vector3(0.20f, 0.20f, 0.20f);

        return role;
    }

    
}
