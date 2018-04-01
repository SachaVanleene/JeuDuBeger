using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptButtonTutoItem : MonoBehaviour {
    private Texture2D imageTuto;
    private PauseTuto instancePT;
    
    private void ActionClick()
    {
        instancePT.ShowTuto(imageTuto.name);
    }

    public void Initiate(Texture2D imageTuto, PauseTuto instancePauseTuto)
    {
        this.imageTuto = imageTuto;
        this.instancePT = instancePauseTuto;
        GetComponent<Button>().onClick.AddListener(this.ActionClick);
        this.GetComponent<RawImage>().texture = imageTuto;
    }
}
