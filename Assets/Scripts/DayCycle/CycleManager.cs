using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleManager : MonoBehaviour
{

    public GameObject Sun;
    public GameObject Stars;
    public GameObject Clouds;
    public GameObject Moon;

    private List<INewCycleListner> cycleListners = new List<INewCycleListner>();

    private intensity sunScript;
    private MoveClouds cloudsScript;

    private int currentAngle; // angle of the cycle between 0 and 360 °(0-180 = day; 180-360 = night)    
    private int nextAngle = 0; // where the cycle must go
    private float skipSpeed = 0; // speed of the cycle after reaching percent goal
    private bool goToNextCycle = false; // must check if the cycle is running

    private bool paused = false;
    private float cachedSpeed = 0f;

    private void Awake()
    {
        sunScript = Sun.GetComponent<intensity>();
        sunScript.RotateSpeed = 0;
    }

    private void changeCycle()
    { // mask moon/sun/clouds or stars ?
        if (IsNight())
        {
            foreach(INewCycleListner listner in cycleListners)
            {
                listner.NightStart();
            }
        }
        if(IsDay())
        {
            foreach (INewCycleListner listner in cycleListners)
            {
                listner.DayStart();
            }
        }
    }

    private void setAngle()
    {
        bool old = currentAngle < 180;
        if (Sun.GetComponent<Transform>().eulerAngles.x < 100)
        { // day
            currentAngle = (int)(Sun.GetComponent<Transform>().eulerAngles.x);
            // between noon and nightfall
            if (Mathf.Abs(Sun.GetComponent<Transform>().rotation.x) > 0.71f)
                currentAngle = 90 + (90 - currentAngle);
        }
        else
        { // night
            currentAngle = (int)(360 - Sun.GetComponent<Transform>().eulerAngles.x);
            // between midnight and dawn
            if (Mathf.Abs(Sun.GetComponent<Transform>().rotation.x) < 0.71f)
                currentAngle = 90 + (90 - currentAngle);
            currentAngle = 180 + currentAngle;
        }

        if (old != currentAngle < 180)
        {
            changeCycle();
        }
    }
    public void GoToAngle(float speed = 10f, int angle = 185)
    {
        paused = false;
        nextAngle = angle;
        goToNextCycle = true;
        sunScript.RotateSpeed = speed;
        setAngle();
    }
    public void EndCycle(float speed = 10f)
    {
        if (nextAngle == currentAngle)
            return; // already ended

        if (IsNight())
            GoToAngle(speed, 355);
        else
            GoToAngle(speed, 175);
    }
    //end the current cycle and launch the second direcly (with différent speed)
    public void SkipCycle(float speed = 1f, float speedEnd = 10f)
    {
        skipSpeed = speed;
        EndCycle(speedEnd);
    }
    public void NextCycle(float speed = 1f)
    {
        if (IsNight())
            GoToAngle(speed, 175);
        else
            GoToAngle(speed, 355);
    }

    private bool checkAngleBetween(int lastAngle)
    { // check if the goal angle has been passed
        if (currentAngle == nextAngle)
        {
            return true;
        }
        if (lastAngle < nextAngle && nextAngle <= currentAngle)
        {
            return true;
        }   
        if(currentAngle < lastAngle)
        {
            if (nextAngle > lastAngle || nextAngle < currentAngle)
                return true;
        }

        return false;
    }
    public void PauseResume()
    {
        paused = !paused;
        if (paused)
        {
            cachedSpeed = sunScript.RotateSpeed;
            sunScript.RotateSpeed = 0;
        }
        else
        {
            sunScript.RotateSpeed = cachedSpeed;
            cachedSpeed = 0;
        }
    }

    private void Update()
    {
        if (!paused && goToNextCycle)
        {
            int lastAngle = currentAngle;
            setAngle();
            if(checkAngleBetween(lastAngle))
            {
                goToNextCycle = false;
                sunScript.RotateSpeed = 0;
                if (skipSpeed != 0)
                {
                    NextCycle(skipSpeed);
                    skipSpeed = 0;
                }
                else
                {
                    foreach (INewCycleListner listner in cycleListners)
                    {
                        listner.WaitingAt(nextAngle , currentAngle);
                    }
                }
            }
        }
    }

    public void SubscribCycle(INewCycleListner ncl)
    {
        cycleListners.Add(ncl);
    }
    public bool isPaused()
    {
        return paused;
    }

    public bool IsDay()
    {
        return !IsNight();
    }
    public bool IsNight()
    {
        return (currentAngle >= 180);
    }
    public int GetPercentCycle()
    {
        return currentAngle;
    }
}
