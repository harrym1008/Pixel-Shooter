using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodManager : MonoBehaviour
{
    public int[] dieTimes;
    public GameObject[] bloodObjects;

    const int BLOOD_COLOUR_OFFSET = 6;


    public void CreateSmallBlood(Vector3 position, Quaternion rotation, BloodType bloodType)
    {
        int index = RNG.NextInclusive(0, 1) + BLOOD_COLOUR_OFFSET * (int) bloodType;
        GameObject g = Instantiate(bloodObjects[RNG.NextInclusive(0, 1)], position, rotation);
        Destroy(g, dieTimes[0]);
    }

    public void CreateBigBlood(Vector3 position, Quaternion rotation, BloodType bloodType)
    {
        int index = RNG.NextInclusive(2, 3) + BLOOD_COLOUR_OFFSET * (int)bloodType;
        GameObject g = Instantiate(bloodObjects[RNG.NextInclusive(2, 3)], position, rotation);
        Destroy(g, dieTimes[1]);
    }

    public void CreateBloodExplosion(Vector3 position, Quaternion rotation, BloodType bloodType)
    {
        int index = RNG.NextInclusive(4, 5) + BLOOD_COLOUR_OFFSET * (int)bloodType;
        GameObject g = Instantiate(bloodObjects[RNG.NextInclusive(4, 5)], position, rotation);
        Destroy(g, dieTimes[2]);
    }


    public enum BloodType
    {
        Crimson,
        Lime,
    }
}