using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchLayerScript : MonoBehaviour {

    Vector3 beforePos3;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        // 按下
        if (Input.GetMouseButtonDown(0))
        {
            touchBegin(Input.mousePosition);
        }
        // 移动
        else if (Input.GetMouseButton(0))
        {
            touchMove(Input.mousePosition);
        }
        // 抬起
        else if (Input.GetMouseButtonUp(0))
        {
            touchEnd(Input.mousePosition);
        }
    }

    void touchBegin(Vector3 posVec3)
    {
        if (posVec3.y < (Screen.height * 0.7f))
        {
            return;
        }

        beforePos3 = posVec3;
    }

    void touchMove(Vector3 posVec3)
    {
        if (posVec3.y < (Screen.height * 0.7f))
        {
            return;
        }

        float rotateSpeed = 0.3f;

        // 右滑
        if (posVec3.x > beforePos3.x)
        {
            GameObject.Find("Main Camera").GetComponent<TrackGameObjScript>().RotateAround(rotateSpeed * CommonUtil.TwoPointDistance3D(posVec3, beforePos3));
        }
        // 左滑
        else if (posVec3.x < beforePos3.x)
        {
            GameObject.Find("Main Camera").GetComponent<TrackGameObjScript>().RotateAround(-rotateSpeed * CommonUtil.TwoPointDistance3D(posVec3, beforePos3));
        }

        beforePos3 = posVec3;
    }

    void touchEnd(Vector3 posVec3)
    {
    }
}
