using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwitchMaster : MonoBehaviour
{
    public Button cont;
    private PlayerData _data;
    public AdManager ad;

    private void Start()
    {
        string path = Application.persistentDataPath + "/player.ColoredAdvance";
        Debug.Log(path);
        if (File.Exists(path))
        {
            _data = SaveSystem.LoadPlayer();
            cont.interactable = !_data.lost;
        }
    }

    public void SelectGame(string value)
    { 
        ad.scene = value;
        ad.ShowInterstitial();
    }

    public void Continue()
    { 
        ad.scene = _data.dimension + "x" + _data.dimension + " (cont.)";
        ad.ShowInterstitial();
    }
}
