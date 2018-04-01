using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollListTuto : MonoBehaviour {
    public GameObject ListScrollTutos;
    public GameObject PrefabTutoItem;
    public GameObject ObjectPauseTuto;

    private float paddingX = 190f;
    public void Start()
    {
        CreateListPanel();
    }
    public void CreateListPanel()
    {
        float padding = 210f;
        PauseTuto instancePT = ObjectPauseTuto.GetComponent<PauseTuto>();
        ListScrollTutos.GetComponent<RectTransform>().sizeDelta = new Vector2(0, padding * instancePT.ImagesTuto.Count);
        ListScrollTutos.transform.localPosition += new Vector3(0, (instancePT.ImagesTuto.Count * padding) / 2, 0);
        float y = (instancePT.ImagesTuto.Count * padding) / 2 + padding / 2;
        foreach (var textureImage in instancePT.ImagesTuto)
        {
            y -= padding;
            GameObject tutoItem = Instantiate(PrefabTutoItem, ListScrollTutos.transform);

            tutoItem.transform.localScale = new Vector3(1f, 1f, 1f);
            tutoItem.transform.localPosition = new Vector3(0, y, 0);

            //set attributes
            tutoItem.GetComponent<ScriptButtonTutoItem>().Initiate(textureImage, ObjectPauseTuto.GetComponent<PauseTuto>());
        }
    }
}
