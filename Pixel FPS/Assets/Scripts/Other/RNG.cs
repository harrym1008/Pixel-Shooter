using UnityEngine;

public static class RNG
{
    static System.Random random = new System.Random();

    /// <summary>
    /// Returns a number of type double between 0 inclusive and 1 exclusive.
    /// </summary>
    /// <returns></returns>
    public static double NextDouble()
    {
        //return 0;
        return random.NextDouble();
    }



    /// <summary>
    /// Returns an integer between the inclusive variable "lower" and the exclusive variable "upper".
    /// </summary>
    /// <param name="lower">The inclusive lower bound of the integer</param>
    /// <param name="upper">The exclusive upper bound of the integer</param>
    /// <returns></returns>
    public static int Next(int lower, int upper)
    {
        int difference = upper - lower - 1;
        double number = NextDouble();

        return Mathf.RoundToInt(lower + difference * (float) number);
    }


    /// <summary>
    /// Returns an integer between the inclusive variables "lower" and "upper".
    /// </summary>
    /// <param name="lower">The inclusive lower bound of the integer</param>
    /// <param name="upper">The inclusive upper bound of the integer</param>
    /// <returns></returns>
    public static int FromTo(int lower, int upper)
    {
        int difference = upper - lower;
        double number = NextDouble();

        return Mathf.RoundToInt(lower + difference * (float) number);
    }

    /// <summary>
    /// Returns a number between the inclusive variables "lower" and "upper".
    /// </summary>
    /// <param name="lower">The inclusive lower bound of the number</param>
    /// <param name="upper">The inclusive upper bound of the number</param>
    /// <returns></returns>
    public static float Range(float lower, float upper)
    {
        float difference = upper - lower;
        double number = NextDouble();

        return lower + difference * (float) number;
    }

    /*
    /// <summary>
    /// Returns a number between the min and max of a "Range" object
    /// </summary>
    /// <param name="range"></param>
    /// <returns></returns>
    public static float Range(Range range)
    {
        float difference = range.max - range.min;
        return range.min + difference * NextFloat();
    }*/


    public static float RangePosNeg(float posBound)
    {
        return Range(-posBound, posBound);
    }

    /// <summary>
    /// Returns a number between the inclusive variables "lower" and "upper", and rounds it to "decimal" decimal places.
    /// </summary>
    /// <param name="lower">The inclusive lower bound of the number</param>
    /// <param name="upper">The inclusive upper bound of the number</param>
    /// <param name="decimals">The number of decimal places to round the number to</param>
    /// <returns></returns>
    public static float RoundRange(float lower, float upper, int decimals)
    {
        return (float)System.Math.Round(Range(lower, upper), decimals);
    }


    /// <summary>
    /// Returns a number between the two values in the Vector2 "values", and rounds it if specified to "decimals" decimal places.
    /// </summary>
    /// <param name="vector2">The Vector2 that holds the inclusive lower and upper bounds</param>
    /// <param name="decimals">The amount of decimal places to round the value to</param>
    /// <returns></returns>
    public static float RangeBetweenVector2(Vector2 values, int decimals = 100)
    {
        if (decimals == 100)
        {
            return Range(values.x, values.y);
        }
        else
        {
            return RoundRange(values.x, values.y, decimals);
        }

    }


    /// <summary>
    /// Returns a boolean dependent on the probabilities of variables "trueChance" and "falseChance".
    /// </summary>
    /// <param name="trueChance">The probability of returning true</param>
    /// <param name="falseChance">The probability of returning false</param>
    /// <returns></returns>
    public static bool ChanceBoolean(float trueChance, float falseChance)
    {
        float total = trueChance + falseChance;

        if (Range(0f, total) <= trueChance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }



    public static int ChanceFromList(float[] chancesForIndex)
    {
        float sum = 0f; foreach (float i in chancesForIndex) sum += i;
        float rngNum = Range(0f, sum);

        float tempTotal = 0f;

        for (int i = 0; i < chancesForIndex.Length; i++)
        {
            tempTotal += chancesForIndex[i];
            if (rngNum < tempTotal)
            {
                return i;
            }
        }

        return chancesForIndex.Length - 1;
    }

    /// <summary>
    /// Returns a float between 0 and 1.
    /// </summary>
    /// <returns></returns>
    public static float NextFloat()
    {
        return (float) NextDouble();
    }

    /// <summary>
    /// Returns a Vector3 with x and y parameters of a random float from the respective negative extent to the respective positive extent relative to the transform.
    /// </summary>
    /// <param name="transform">Relative transform</param>
    /// <param name="xExtent">Positive extent of the x parameter</param>
    /// <param name="yExtent">Positive extent of the y parameter</param>
    /// <returns></returns>
    public static Vector3 RandomSpread(Transform transform, float xExtent, float yExtent)
    {
        return transform.right * Range(-xExtent, xExtent) + transform.up * Range(-yExtent, yExtent);
    }


    /// <summary>
    /// Returns a boolean with a random value of true or false.
    /// </summary>
    /// <returns></returns>
    public static bool RandomBoolean()
    {
        return NextDouble() % 2 == 0;
    }

    /// <summary>
    /// Returns a Vector3 that can be between -1 and 1 on each axis.
    /// </summary>
    /// <param name="x">Setting this to false will make it so the Vector3's x value is 0.</param>
    /// <param name="y">Setting this to false will make it so the Vector3's y value is 0.</param>
    /// <param name="z">Setting this to false will make it so the Vector3's z value is 0.</param>
    /// <returns></returns>
    public static Vector3 RandomVector3(bool x = true, bool y = true, bool z = true)
    {
        float _x = Range(-1f, 1f); if (!x) { _x = 0f; }
        float _y = Range(-1f, 1f); if (!y) { _y = 0f; }
        float _z = Range(-1f, 1f); if (!z) { _z = 0f; }

        return new Vector3(_x, _y, _z);
    }

    /// <summary>
    /// Returns a Vector3 that can be between -1 and 1 on each axis.
    /// </summary>
    /// <param name="x">Multipier of the Vector3's x value.</param>
    /// <param name="y">Multipier of the Vector3's y value.</param>
    /// <param name="z">Multipier of the Vector3's z value.</param>
    /// <returns></returns>
    public static Vector3 RandomVector3(float x, float y, float z)
    {
        return new Vector3(Range(-1f, 1f) * x, Range(-1f, 1f) * y, Range(-1f, 1f) * z);
    }

    /// <summary>
    /// Returns a random object from a list of object.
    /// </summary>
    /// <param name="objects">The list of objects to choose from</param>
    /// <returns></returns>
    public static T RandomObject<T>(T[] objects)
    {
        return objects[Next(0, objects.Length)];
    }
}