using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleManager : MonoBehaviour
{

    public GameObject Sun;
    public GameObject Stars;
    public GameObject Clouds;
    public GameObject Moon;

    private intensity sunScript;
    private MoveClouds cloudsScript;

    private int percentCycle;

    private int nextPercent = 0;
    private bool goToNextCycle = false;
    private bool firstFound = false;
    private bool SecondFound = false;

    private float skipSpeed = 0;

    private int beginNight = 358;
    private int beginDay = 2;

    private void Awake()
    {
        sunScript = Sun.GetComponent<intensity>();
        sunScript.RotateSpeed = 1;
        //NextCycle(10);
    }

    private void changeCycle()
    {
        // mask moon/sun/clouds or stars ?
    }

    private void setPercent()
    {
        percentCycle = (int)(Sun.GetComponent<Transform>().rotation.eulerAngles.x);
        return;
    }

    public void EndCycle(float speed = 10f)
    {
        if (IsNight())
            nextPercent = beginNight;
        else
            nextPercent = beginDay;

        if (nextPercent == percentCycle)
            return; // already ended

        goToNextCycle = true;
        sunScript.RotateSpeed = speed;
        firstFound = true;
        SecondFound = true;
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
            nextPercent = beginDay;
        else
            nextPercent = beginNight;

        goToNextCycle = true;
        sunScript.RotateSpeed = speed;
    }
    
    private void Update()
    {
        if (goToNextCycle)
        {
            setPercent();
            if(percentCycle <= nextPercent + 1 && percentCycle >= nextPercent - 1)
            {
                if (!firstFound)
                {
                    firstFound = true;
                }
                else
                {
                    if(SecondFound)
                    {
                        goToNextCycle = false;
                        firstFound = false;
                        SecondFound = false;
                        sunScript.RotateSpeed = 0;
                        if(skipSpeed != 0)
                        {
                            NextCycle(skipSpeed);
                            skipSpeed = 0;
                        }
                    }
                }
            }
            else
            {
                if (firstFound)
                    SecondFound = true;
            }


        }
    }

    public bool IsDay()
    {
        return !IsNight();
    }
    public bool IsNight()
    {
        return (percentCycle > 100); // unity EulerAngle shit
    }
    public int GetPercentCycle()
    {
        return percentCycle;
    }
}
