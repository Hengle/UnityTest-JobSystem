using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CubeHandler : MonoBehaviour {

    [Serializable]
    public struct IntVector2 { public int x, y; }

    public GameObject cubePrefab;
    public IntVector2 cubeNumber;
    public Rect positionBound;
    public Transform[,] cubeTransform;

    void Awake () {
        QualitySettings.vSyncCount = 0;
        GenerateCubes ();
    }

    void GenerateCubes () {
        if (cubePrefab == null) {
            return;
        }

        cubeTransform = new Transform[cubeNumber.x, cubeNumber.y];
        for (int x = 0; x < cubeNumber.x; x++) {
            for (int y = 0; y < cubeNumber.y; y++) {
                // var go = Instantiate<GameObject> (cubePrefab, this.transform);
                var go = Instantiate<GameObject> (cubePrefab);
                cubeTransform[x, y] = go.transform;
                cubeTransform[x, y].localPosition = new Vector2 (Mathf.Lerp(positionBound.xMin, positionBound.xMax, (float)x / cubeNumber.x), Mathf.Lerp(positionBound.yMin, positionBound.yMax, (float)y / cubeNumber.y));
            }   
        }
    }
}
