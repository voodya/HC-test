using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class FPSViwer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _fps;

    private void Awake()
    {
        Application.targetFrameRate = 120;
    }

    private void Update()
    {
        int fps = (int) (1 / Time.unscaledDeltaTime);
        _fps.text = $"{fps} / {Time.deltaTime} ms";
    }
}
