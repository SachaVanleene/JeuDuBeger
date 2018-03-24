using UnityEngine;

namespace Assets
{
    public class SelectionElement : MonoBehaviour
    {

        // Use this for initialization
        void Start ()
        {
            transform.localScale = Vector3.one * 0.8f;
        }
	
        // Update is called once per frame
        void Update () {
		
        }

        public void Select()
        {
           // transform.GetChild(0).gameObject.SetActive(true);
            transform.localScale = Vector3.one;
        }
        public void Unselect()
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.localScale = Vector3.one * 0.8f;
        }
    }
}
