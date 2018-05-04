using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour {

    CubeHandler m_cubeHandler;
	float m_time;
    readonly float DOUBLE_PI = Mathf.PI * 2f;
	
    void Start () {
        m_cubeHandler = GetComponent<CubeHandler> ();
    }

	void Update () {
        m_time += Time.deltaTime;
        m_time %= DOUBLE_PI;
		for (int x = 0; x < m_cubeHandler.cubeNumber.x; x++) {
            for (int y = 0; y < m_cubeHandler.cubeNumber.y; y++) {
                Vector3 pos = m_cubeHandler.cubeTransform[x, y].localPosition;
                pos.z = Mathf.Sin (m_time + x) + Mathf.Sin (m_time + y);
                m_cubeHandler.cubeTransform[x, y].localPosition = pos;
            }   
        }
	}
}
