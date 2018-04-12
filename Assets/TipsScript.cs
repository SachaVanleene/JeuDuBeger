using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsScript : MonoBehaviour
{
    private float _nextActionTime = 0.0f;
    private float _period = 15f;
    private List<string> _tipsList = new List<string>()
    {
        "Le saviez vous : un loup avec du plomb dans le corps aura bien moins faim !",
        "La mine inflige de lourds dégats de zone, mais bon elle est a usage unique",
        "Rien de mieux qu'un lapin pour appater les loups loin de mes enclos !",
        "Si j'aurais su que les montagnes étaient eux aussi truffé de loups j'aurais pas venu"
    };
	// Use this for initialization
	void Start () {
		
	}
    void FixedUpdate()
    {
        if (Time.time > _nextActionTime)
        {
            _nextActionTime += _period;
            int rand = Random.Range(0, _tipsList.Count - 1);
            this.GetComponent<Text>().text = _tipsList[rand];
            if (_tipsList.Count > 1)
            {
                _tipsList.Remove(_tipsList[rand]);
            }
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
