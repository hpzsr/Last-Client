using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockerScript : MonoBehaviour {

    public delegate void RockerEvent_Move(float angle);
    public delegate void RockerEvent_Reset();

    public static RockerEvent_Move s_rockerEvent_Move = null;
    public static RockerEvent_Reset s_rockerEvent_Reset = null;
    
    public bool m_useMouse;

    public GameObject m_bg;
    public GameObject m_ball;

    Vector3 m_rockerInitPos;
    float m_bgRadius;               // 大圆的半径，这是小圆的最大移动范围
    float m_triggerMoveLength;      // 触发距离，小圆移动超过这个距离，即可生效
    bool m_isStartMove;

    // Use this for initialization
    void Start ()
    {
        m_bgRadius = m_bg.GetComponent<RectTransform>().sizeDelta.x / 2;
        m_triggerMoveLength = 20;
        m_rockerInitPos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_useMouse)
        { 
            Vector2 touchPos = Input.mousePosition;
            
            if (Input.GetMouseButtonDown(0))
            {
                if (TwoPointDistance2D(touchPos, m_bg.transform.position) <= m_bgRadius)
                {
                    m_isStartMove = true;
                    m_ball.transform.position = touchPos;
                }
                else if (TwoPointDistance2D(touchPos, new Vector2(0, 0)) <= 500)
                {
                    gameObject.transform.position = touchPos;

                    m_isStartMove = true;
                    m_ball.transform.position = touchPos;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (m_isStartMove)
                {
                    m_ball.transform.localPosition = new Vector2(0, 0);
                    m_isStartMove = false;

                    gameObject.transform.position = m_rockerInitPos;
                    if (s_rockerEvent_Reset != null)
                    {
                        s_rockerEvent_Reset();
                    }
                }
            }
            else if (Input.GetMouseButton(0))
            {
                if (m_isStartMove)
                {
                    if (TwoPointDistance2D(touchPos, m_bg.transform.position) <= m_bgRadius)
                    {
                        m_ball.transform.position = touchPos;
                    }
                    else
                    {
                        float k = (touchPos.y - m_bg.transform.position.y) / (touchPos.x - m_bg.transform.position.x);
                        float x = Mathf.Cos(Mathf.Atan(k)) * m_bgRadius;
                        float y = Mathf.Sin(Mathf.Atan(k)) * m_bgRadius;

                        x = Mathf.Abs(x);
                        y = Mathf.Abs(y);

                        if ((touchPos.x < m_bg.transform.position.x) && (touchPos.y > m_bg.transform.position.y))
                        {
                            x = -x;
                        }
                        else if ((touchPos.x < m_bg.transform.position.x) && (touchPos.y < m_bg.transform.position.y))
                        {
                            x = -x;
                            y = -y;
                        }
                        else if ((touchPos.x > m_bg.transform.position.x) && (touchPos.y < m_bg.transform.position.y))
                        {
                            y = -y;
                        }

                        m_ball.transform.localPosition = new Vector2(x, y);
                    }

                    {
                        float angle = getRockerAngle();
                        if (s_rockerEvent_Move != null)
                        {
                            s_rockerEvent_Move(angle);
                        }
                    }
                }
            }

            return;
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        else if (Input.touchCount > 0)
        {
            Vector2 ballPos = m_ball.transform.position;
            int touchId = -1;

            {
                for(int i = 0 ; i < Input.touchCount; i++)
                {
                    if (TwoPointDistance2D(Input.GetTouch(i).position, m_bg.transform.position) <= m_bgRadius)
                    {
                        touchId = i;
                        break;
                    }
                    else if (TwoPointDistance2D(Input.GetTouch(i).position, new Vector2(0, 0)) <= 500)
                    {
                        touchId = i;
                        break;
                    }
                }
            }

            if(touchId == -1)
            {
                return;
            }

            Vector2 touchPos = Input.GetTouch(touchId).position;
            if (Input.GetTouch(touchId).phase == TouchPhase.Began)
            {
                if (TwoPointDistance2D(touchPos, m_bg.transform.position) <= m_bgRadius)
                {
                    m_isStartMove = true;
                    m_ball.transform.position = touchPos;
                }
                else if (TwoPointDistance2D(touchPos, new Vector2(0,0)) <= 500)
                {
                    gameObject.transform.position = touchPos;

                    m_isStartMove = true;
                    m_ball.transform.position = touchPos;
                }
            }
            else if ((Input.GetTouch(touchId).phase == TouchPhase.Stationary) || (Input.GetTouch(touchId).phase == TouchPhase.Moved))
            {
                if (m_isStartMove)
                {
                    if (TwoPointDistance2D(touchPos, m_bg.transform.position) <= m_bgRadius)
                    {
                        m_ball.transform.position = touchPos;
                    }
                    else
                    {
                        float k = (touchPos.y - m_bg.transform.position.y) / (touchPos.x - m_bg.transform.position.x);
                        float x = Mathf.Cos(Mathf.Atan(k)) * m_bgRadius;
                        float y = Mathf.Sin(Mathf.Atan(k)) * m_bgRadius;

                        x = Mathf.Abs(x);
                        y = Mathf.Abs(y);

                        if ((touchPos.x < m_bg.transform.position.x) && (touchPos.y > m_bg.transform.position.y))
                        {
                            x = -x;
                        }
                        else if ((touchPos.x < m_bg.transform.position.x) && (touchPos.y < m_bg.transform.position.y))
                        {
                            x = -x;
                            y = -y;
                        }
                        else if ((touchPos.x > m_bg.transform.position.x) && (touchPos.y < m_bg.transform.position.y))
                        {
                            y = -y;
                        }

                        m_ball.transform.localPosition = new Vector2(x, y);
                    }

                    {
                        float angle = getRockerAngle();
                        if (s_rockerEvent_Move != null)
                        {
                            s_rockerEvent_Move(angle);
                        }
                    }
                }
            }
            else if (Input.GetTouch(touchId).phase == TouchPhase.Ended)
            {
                if(m_isStartMove)
                {
                    m_ball.transform.localPosition = new Vector2(0, 0);
                    m_isStartMove = false;
                    
                    gameObject.transform.position = m_rockerInitPos;

                    if (s_rockerEvent_Reset != null)
                    {
                        s_rockerEvent_Reset();
                    }
                }
            }
        }
    }

    public float getRockerAngle()
    {
        if (TwoPointDistance2D(new Vector2(0, 0), m_ball.transform.localPosition) <= m_triggerMoveLength)
        {
            return 0;
        }

        return TwoPointAngle(new Vector2(0, 0), m_ball.transform.localPosition);
    }

    // 2D中两点之间的距离
    public float TwoPointDistance2D(Vector3 point1, Vector3 point2)
    {
        float i = Mathf.Sqrt((point1.x - point2.x) * (point1.x - point2.x)
                           + (point1.y - point2.y) * (point1.y - point2.y));

        return i;
    }

    // 返回值：0~360,从正Y轴开始
    static public float TwoPointAngle(Vector2 point1, Vector2 point2)
    {
        float angle = 0;
        float k = (point2.y - point1.y) / (point2.x - point1.x);
        float tempAngle = Mathf.Atan(k) * Mathf.Rad2Deg;

        // 第一象限
        if ((point2.x > point1.x) && (point2.y > point1.y))
        {
            angle = 90 - tempAngle;
        }
        // 第二象限
        else if ((point2.x < point1.x) && (point2.y > point1.y))
        {
            angle = 360 - (90 + tempAngle);
        }
        // 第三象限
        else if ((point2.x < point1.x) && (point2.y < point1.y))
        {
            angle = 180 + (90 - tempAngle);
        }
        // 第四象限
        else if ((point2.x > point1.x) && (point2.y < point1.y))
        {
            angle = 90 + (-tempAngle);
        }

        return angle;
    }
}
