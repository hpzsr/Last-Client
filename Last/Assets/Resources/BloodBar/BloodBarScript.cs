using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodBarScript : MonoBehaviour {
    
    Image m_front;
    GameObject m_parent = null;

    public static BloodBarScript Create(Transform parent, GameObject target,bool isMy)
    {
        GameObject obj = Resources.Load("BloodBar/BloodBar") as GameObject;
        GameObject bloodBarObj = Instantiate(obj,parent);

        BloodBarScript script = bloodBarObj.GetComponent<BloodBarScript>();
        script.m_parent = target;
        script.setColor(isMy);

        return script;
    }

    private void Awake()
    {
        m_front = transform.Find("Image_front").GetComponent<Image>();
    }

    // Use this for initialization
    void Start ()
    {
        setPercent(100.0f);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (m_parent != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(m_parent.transform.position);
        }
    }

    public void setPercent(float percent)
    {
        m_front.fillAmount = percent / 100.0f;
    }

    public void setColor(bool isMy)
    {
        if (!isMy)
        {
            m_front.sprite = Resources.Load("BloodBar/blood_red",typeof(Sprite)) as Sprite;
        }
    }
}
