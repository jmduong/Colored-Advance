using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rainbow : MonoBehaviour
{
    [SerializeField]
    bool _transparent;
    Color _init, _target;
    float _timer;
    Material _mat;

    // Start is called before the first frame update
    void Start()
    {
        _mat = GetComponent<MeshRenderer>().material;
        LoopColors();
    }

    private void LoopColors()
    {
        _timer = 0;
        _init = _mat.color;
        _target = _transparent ? Random.ColorHSV(0f, 1f, 1f, 1f, 0f, 1f, 0.3f, 0.3f) : Random.ColorHSV(0f, 1f, 1f, 1f, 0f, 1f);
    }

    private void Update()
    {
        if(_timer <= 1)
        {
            _timer += Time.deltaTime;
            _mat.color = Color32.Lerp(_init, _target, _timer);
        }
        else
            LoopColors();
    }
}
