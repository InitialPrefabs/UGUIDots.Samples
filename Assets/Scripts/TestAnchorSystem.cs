using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class TestAnchorSystem : MonoBehaviour
{

    RectTransform rect;
    // Start is called before the first frame update
    void Start()
    {
        rect = this.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"Local: {rect.localPosition}, Screen: {rect.position}, Resolution: {Screen.width}, {Screen.height}");
    }
}
