using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class AchievementPopUpScript : MonoBehaviour {
    public int MaxLeft;
    public int MaxRight;
    public RawImage Image;
    public Text Text;

    private float speed = 0f;
    private Queue<AchievementInfo> queue = new Queue<AchievementInfo>();
    private RectTransform rtrans;

    private void Start()
    {
        rtrans = GetComponent<RectTransform>();
    }

    public void AddAchievement(AchievementInfo achInfo)
    {
        queue.Enqueue(achInfo);
        if(queue.Count <= 1)
            displayInfo();
    }
    private void displayInfo()
    {
        if (queue.Count > 0)
        {
            AchievementInfo achInfo = queue.Peek();
            Text.text = achInfo.Name;
            Texture2D texture = SProfilePlayer.getInstance().DefaultSprite;
            if(SProfilePlayer.getInstance().SpritesAchievements != null)
                foreach (var t in SProfilePlayer.getInstance().SpritesAchievements)
                {
                    if (t.name.Equals(achInfo.Name))
                        texture = t;
                }
            else
            {
                Debug.LogError("Lance la scene depuis le menu !");
            }
            Image.texture = texture;
            speed = -GameVariables.Achievements.PopUp.speedCome;
        }
    }

    private IEnumerator wait()
    {
        yield return new WaitForSeconds(GameVariables.Achievements.PopUp.timeStay);
        speed = GameVariables.Achievements.PopUp.speedBack;
    }
	void Update () {
        if(speed != 0f)
        {
            if(speed < 0)
            {   //  moving left
                if (rtrans.localPosition.x <= MaxLeft)
                {
                    speed = 0f;
                    rtrans.localPosition = new Vector3(MaxLeft, rtrans.localPosition.y, rtrans.localPosition.z);
                    StartCoroutine(wait());
                }
                else
                    rtrans.localPosition += new Vector3(speed * Time.deltaTime, 0, 0);
            }
            else
            {   //  moving right
                if (rtrans.localPosition.x >= MaxRight)
                {
                    speed = 0f;
                    rtrans.localPosition = new Vector3(MaxRight, rtrans.localPosition.y, rtrans.localPosition.z);
                    queue.Dequeue();
                    displayInfo();
                }
                else
                    rtrans.localPosition += new Vector3(speed * Time.deltaTime, 0, 0);
            }
        }
	}
}
