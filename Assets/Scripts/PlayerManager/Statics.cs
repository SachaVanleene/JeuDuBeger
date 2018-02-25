using System.Collections;
using UnityEngine;

public static class Statics
{
    #region hash
    public static string horizontal = "horizontal";
    public static string vertical = "vertical";
    public static string special = "special";
    public static string specialType = "specialType";
    public static string onLocomotion = "onLocomotion";
    public static string Horizontal = "Horizontal";
    public static string Vertical = "Vertical";
    public static string jumpType = "jumpType";
    public static string Jump = "Jump";
    public static string onAir = "onAir";
    public static string mirrorJump = "mirrorJump";
    public static string incline = "incline";
    public static string Run = "Run";
    #endregion

    public static int GetAnimSpecialType(AnimSpecials i)
    {
        int retVal = 0;
        switch (i)
        {
            case AnimSpecials.run:
                retVal = 10;
                break;

            case AnimSpecials.runToStop:
                retVal = 11;
                break;

            case AnimSpecials.jump_idle:
                retVal = 21;
                break;

            case AnimSpecials.run_jump:
                retVal = 22;
                break;
            default:
                break;
        }

        return retVal;
    }

}

public enum AnimSpecials
{
    run, runToStop, jump_idle, run_jump
}