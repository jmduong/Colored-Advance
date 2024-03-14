using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Backdrops : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ColorChange();
    }

    public void ColorChange()
    {
        GetComponent<Image>().color = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);
    }
}
