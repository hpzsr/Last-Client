using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackGameObjScript : MonoBehaviour
{
    public GameObject m_target = null;
    public float m_damping = 0;
    Vector3 offset;
    bool m_isTrack = true;

    void Start()
    {
    }

    public void startTrack(GameObject target)
    {
        m_target = target;
        offset = transform.position - m_target.transform.position;
    }

    public void setTrackEnable(bool target)
    {
        m_isTrack = target;
    }

    void LateUpdate()
    {
        if(m_isTrack && (m_target != null))
        {
            if (m_damping > 0)
            {
                Vector3 desiredPosition = m_target.transform.position + offset;
                Vector3 position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * m_damping);
                transform.position = position;

                transform.LookAt(m_target.transform.position);
            }
            else
            {
                Vector3 desiredPosition = m_target.transform.position + offset;
                transform.position = desiredPosition;
            }
        }
    }
}
