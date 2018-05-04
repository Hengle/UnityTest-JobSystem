using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Jobs;
using Unity.Collections;

public class WaveInJob : MonoBehaviour {

	CubeHandler m_cubeHandler;
	float m_time;
    readonly float DOUBLE_PI = Mathf.PI * 2f;
	
    struct WaveParallelJob : IJobParallelFor {
        [ReadOnly]
        public float time;
        [ReadOnly]
        public NativeArray<float> x;
        [ReadOnly]
        public NativeArray<float> y;
        public NativeArray<Vector3> result;
        public void Execute (int i) {
            Vector3 pos = result[i];
            pos.z = Mathf.Sin (time + x[i]) + Mathf.Sin (time + y[i]);
            result[i] = pos;
        }
    }

    struct SetPosParallelJob : IJobParallelForTransform {
        [ReadOnly]
        public NativeArray<Vector3> pos;
        public void Execute (int i, TransformAccess transform) {
            transform.localPosition = pos[i];
        }
    }

    int m_xNum;
    int m_yNum;
    int m_length;
    NativeArray<float> m_indexX;
    NativeArray<float> m_indexY;
    NativeArray<Vector3> m_position;
    TransformAccessArray m_transformsAccess;

    WaveParallelJob m_waveJob;
    SetPosParallelJob m_setPosJob;
    JobHandle m_waveJobHandle;
    JobHandle m_setPosJobHandle;

    void Start () {
        m_cubeHandler = GetComponent<CubeHandler> ();

        m_xNum = m_cubeHandler.cubeNumber.x;
        m_yNum = m_cubeHandler.cubeNumber.y;
        m_length = m_cubeHandler.cubeNumber.x * m_cubeHandler.cubeNumber.y;
        m_indexX = new NativeArray<float> (m_length, Allocator.Persistent);
        m_indexY = new NativeArray<float> (m_length, Allocator.Persistent);
        m_position = new NativeArray<Vector3> (m_length, Allocator.Persistent);

        Transform[] transformArray = new Transform[m_length];
        for (int x = 0; x < m_xNum; x++) {
            for (int y = 0; y < m_yNum; y++) {
                m_indexX[x * m_yNum + y] = x;
                m_indexY[x * m_yNum + y] = y;
                m_position[x * m_yNum + y] = m_cubeHandler.cubeTransform[x, y].localPosition;
                transformArray[x * m_yNum + y] = m_cubeHandler.cubeTransform[x, y];
            }   
        }
        m_transformsAccess = new TransformAccessArray (transformArray);
    }

	void Update () {
        // m_setPosJobHandle.Complete ();
        m_time += Time.deltaTime;
        m_time %= DOUBLE_PI;

        m_waveJob = new WaveParallelJob () {
            time = m_time,
            x = m_indexX,
            y = m_indexY,
            result = m_position
        };

        m_setPosJob = new SetPosParallelJob () {
            pos = m_position
        };

        m_waveJobHandle = m_waveJob.Schedule (m_length, 32);
        m_setPosJobHandle = m_setPosJob.Schedule (m_transformsAccess, m_waveJobHandle);
	}

    // It's an easy way to handle completing jobs in LateUpdate().
    // But it might be better to handle in Update() at somewhere for using the result in the same frame.
    void LateUpdate () {
        m_setPosJobHandle.Complete ();
    }

    void OnDestroy () {
        if (m_indexX.IsCreated) {
            m_indexX.Dispose ();
            m_indexY.Dispose ();
            m_position.Dispose ();
            m_transformsAccess.Dispose ();
        }
    }
}
