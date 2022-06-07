using UnityEngine;

public static class Wait
{
    public static WaitForEndOfFrame Frame = new WaitForEndOfFrame();


    public static WaitForSeconds Seconds(float seconds)
    {
        return new WaitForSeconds(seconds);
    }

    public static WaitForSecondsRealtime RealtimeSeconds(float seconds)
    {
        return new WaitForSecondsRealtime(seconds);
    }

    public static WaitForSeconds Minutes(float minutes)
    {
        return Seconds(minutes * 60);
    }

    public static WaitForSeconds Hours(float hours)
    {
        return Seconds(hours * 3600);
    }
}