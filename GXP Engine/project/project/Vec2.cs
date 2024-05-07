using System;
using System.Diagnostics.SymbolStore;
using System.Runtime.CompilerServices;
using System.Xml.Schema;
using GXPEngine; // Allows using Mathf functions
using GXPEngine.Core;

public struct Vec2
{
    public float x;
    public float y;

    public Vec2(float pX = 0, float pY = 0)
    {
        x = pX;
        y = pY;
    }

    //returns the result of adding two vectors(without modifying either of them)
    public static Vec2 operator +(Vec2 left, Vec2 right)
    {
        return new Vec2(left.x + right.x, left.y + right.y);
    }

    //print these if the vec2 is to be printed
    public override string ToString()
    {
        return String.Format("(x: {0} , y: {1})", x, y);
    }

    /*
     * 
     * FIRST EXPANSION
     * 
     */

    //returns the result of subtracting two vectors (without modifying either of them)
    public static Vec2 operator -(Vec2 left, Vec2 right)
    {
        return new Vec2(left.x - right.x, left.y - right.y);
    }


    //returns the result of scaling a vector (input vecter first)
    public static Vec2 operator *(Vec2 theVector, float theScale)
    {
        return new Vec2(theVector.x * theScale, theVector.y * theScale);
    }

    //returns the result of scaling a vector (input scaler first)
    public static Vec2 operator *(float theScale, Vec2 theVector)
    {
        return new Vec2(theVector.x * theScale, theVector.y * theScale);
    }

    //returns the result of dividing a vector (input vecter first)
    public static Vec2 operator / (Vec2 theVector, float theScale)
    {
        return new Vec2(theVector.x / theScale, theVector.y / theScale);
    }

    //returns the result of dividing a vector (input scaler first)
    public static Vec2 operator / (float theScale, Vec2 theVector)
    {
        return new Vec2(theVector.x / theScale, theVector.y / theScale);
    }

    //returns the length of the current vector
    public float Length()
    {
        return (float)Math.Sqrt((x * x) + (y * y));
    }

    //normalizes the current vector
    public void Normalize()
    {
        float theLength = Length(); //this keep the vector's orginal length so calculating the y would be accurate

        if (theLength == 0) //that means the Vector is (0,0), normalization is not possible
        {
            return;
        }

        x = x / theLength;
        y = y / theLength;
    }


    //returns a normalized version of the current vector without modifying it
    public static Vec2 Normalized(Vec2 theVector)
    {
        float theLength = theVector.Length();

        if (theLength == 0) //that means the Vector is (0,0), normalization is not possible
        {
            return new Vec2(0, 0);
        }
        return new Vec2(theVector.x / theLength, theVector.y / theLength);
    }

    //sets the x & y of the current vector to the given two floats
    public void SetXY(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    /*
     * 
     * SECOND EXPANSION
     * 
     */

    // converts the given degrees to radians
    public static float DegreeToRad(float theDegree)
    {
        return (float)(Math.PI / 180) * theDegree;
    }

    //converts the given radians to degrees
    public static int RadToDegree(float rad)
    {
        return (int)((180 / Math.PI) * rad);
    }

    //returns a unit vector pointing in the given direction in degrees
    public static Vec2 GetUnitVectorDegrees(float angle)
    {
        Vec2 result = new Vec2(Mathf.Cos(DegreeToRad(angle)), Mathf.Sin(DegreeToRad(angle)));
        result.Normalize();
        return result;
    }

    //set vector angle to the given direction in degrees (length doesn’t change)
    public void SetAngleDegrees(float angle, float theSpeed)
    {
        Vec2 result = GetUnitVectorDegrees(angle) * theSpeed;
        this = result;
    }


    //returns a unit vector pointing in the given direction in radians
    public static Vec2 GetUnitVectorRad(float angle)
    {
        Vec2 result = new Vec2(Mathf.Cos(angle), Mathf.Sin(angle));
        result.Normalize();
        return result;
    }


    //returns a unit vector pointing in a random direction (all angles should
    //have the same probability(density)).
    public static Vec2 RandomUnitVector()
    {
        float theAngle = Utils.Random(1f, 360.001f);
        return GetUnitVectorDegrees(theAngle);
    }


    //set vector angle to the given direction in degrees (length doesn’t change)
    public void SetAngleDegree(float angle, float theSpeed)
    {
        Vec2 result = GetUnitVectorDegrees(angle) * theSpeed;
        this = result;
    }

    //set vector angle to the given direction in radians (length doesn’t change)
    public void SetAngleRadians(float angle)
    {
        Vec2 result = GetUnitVectorRad(angle) * Length();
        this = result;
    }

    public void SetAngleRadians(float angle, float theSpeed)
    {
        Vec2 result = GetUnitVectorRad(angle) * theSpeed;
        this = result;
    }

    //gets the vector angle in radians
    public float GetAngleRadians()
    {
        return Mathf.Atan2(y, x);
    }

    //gets the vector angle in degrees
    public float GetAngleDegrees()
    {
        return RadToDegree(Mathf.Atan2(y, x));
    }

    //rotate the vector over the given angle in radians
    public void RotateRadians(float angle)
    {
        Vec2 vecTemp = this;
        x = (x * Mathf.Cos(angle) - y * Mathf.Sin(angle));
        y = (vecTemp.x * Mathf.Sin(angle) + vecTemp.y * Mathf.Cos(angle));
    }

    //rotate the vector over the given angle in degrees
    public void RotateDegrees(float angle)
    {
        float theAngle = DegreeToRad(angle);
        Vec2 vecTemp = this;
        x = (x * Mathf.Cos(theAngle) - y * Mathf.Sin(theAngle));
        y = (vecTemp.x * Mathf.Sin(theAngle) + vecTemp.y * Mathf.Cos(theAngle));
    }

    //rotate the vector around the given point over the given angle in degrees
    public void RotateAroundDegrees(float theX, float theY, float theAngle)
    {
        x -= theX;
        y -= theY;

        RotateDegrees(theAngle);

        x += theX;
        y += theY;
    }

    // rotate the vector around the given point over the given angle in radians
    public void RotateAroundRadian(float theX, float theY, float theAngle)
    {
        x -= theX;
        y -= theY;

        RotateRadians(theAngle);

        x += theX;
        y += theY;
    }

    /*
     * 
     * THRID EXPANSION
     * 
     */

    //returns the dot product between the current and the given vector
    public float Dot(Vec2 other)
    {
        return x * other.x + y * other.y;
    }

    // returns a new Vec2 representing the unit normal for the current vector
    public Vec2 Normal()
    {
        return Normalized(new Vec2(-y, x));
    }

    public void Reflect(Vec2 normal, float bounciness = 1)
    {
        this = this - (1 + bounciness) * this.Dot(normal) * normal;
    }

    /*
     * 
     * TEST METHOD FUNCTION
     * 
     */

    public static void testMethods()
    {
        /*
         * 
         * Week 1 methods tests
         * 
         */

        //Vector length test
        /*
        float result = myVec.Length();
        Console.WriteLine(Math.Round(result, 2) == 0.55);
        */


        //degree to rad test
        /*
        float result = Vec2.DegreeToRad(208);
        Console.WriteLine(Math.Round(result, 2) == 3.63);
        */

        //rad to degree test
        /*
        int result = Vec2.RadToDegree(1.33f);
        Console.WriteLine(result);
        Console.WriteLine(result == 76);
        */

        /*
         * 
         * Week 2 methods tests
         * 
         */

        //GetUnitVectorDeg test
        /*
        Vec2 result = Vec2.GetUnitVectorDeg(127f);
        Console.WriteLine(result);
        Console.WriteLine(result.Length());
        */
        //    Console.WriteLine(Math.Round(result.x, 2) == 1 && Math.Round(result.y, 2) == 0.08);


        //GetUnitVectorRad test
        /*
        Vec2 result = Vec2.GetUnitVectorRad(0.0849f);
        Console.WriteLine(Math.Round(result.x, 2) == 1 && Math.Round(result.y, 2) == 0.08);
        */

        //SetAngleDegree test
        /*
        Vec2 result = new Vec2(8,7);
        result.SetAngleDegree (310);
        Console.WriteLine(result);
        Console.WriteLine(Math.Round(result.x, 2) == 6.83 && Math.Round(result.y, 2) == -8.14);
        */


        //SetAngleRadians test
        /*
        Vec2 result = new Vec2(8,7);
        result.SetAngleRadians (5.41052f);
        Console.WriteLine(Math.Round(result.x, 2) == 6.83 && Math.Round(result.y, 2) == -8.14);
        */

        //Get angle rad test
        /*
        Vec2 result = new Vec2(1,7.4f);
        Console.WriteLine(Math.Round(result.GetAngleRadians(), 2) == 1.44);
        */

        //Get angle degree test
        /*
        Vec2 result = new Vec2(1, 7.4f);
        Console.WriteLine(Math.Round(result.GetAngleDegrees(), 2) == 82);
        */

        //rotate degree test
        /*
        Vec2 result = new Vec2(2,5);
        result.RotateDegrees(90);
        Console.WriteLine(result);
        Console.WriteLine(result.x == -5f);
        */

        //rotate rad test
        /*
        Vec2 result = new Vec2(2, 5);
        result.RotateRadians(1.5708f);
        Console.WriteLine(Math.Round(result.x, 1) == -5f);
        */

        /*
         * 
         * Week 4 methods tests
         * 
         */

        //dot product test
        /*
        Vec2 v1 = new Vec2(1, 2);
        Vec2 v2 = new Vec2(4, 5);
        Console.WriteLine(v1.Dot(v2) == 14);
        */

        //vector normal test
        /*
        Vec2 v1 = new Vec2(1, 2);
        Vec2 v2 = new Vec2(-2, 1);
        v1 = v1.Normal();
        v2.Normalize();
        Console.WriteLine(v1.x == v2.x && v1.y == v2.y);
        */

        //Reflection test
        /*
        Vec2 v1 = new Vec2(1, 2);
        Vec2 v2 = new Vec2(2, 3);
        v1.Reflect(v2, 0.5f);
        Console.WriteLine(v1.x == -23 && v1.y == -34);
        */
    }
}