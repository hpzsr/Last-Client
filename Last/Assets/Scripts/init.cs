using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class init : MonoBehaviour {
    
	// Use this for initialization
	void Start ()
    {
        // 设置帧率
        Application.targetFrameRate = 60;

        if (GameData.isShowedLogin)
        {
            MainScript.show();
        }
        else
        {
            GameData.isShowedLogin = true;
            LoginScript.show();
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
