using GXPEngine;
using System;

//the tank body
class Tank : Sprite
{

    public bool wordy = false;

    // public fields & properties:
    public Vec2 Position
    {
        get
        {
            return _position;
        }
    }

    Vec2 velocity;

    public Vec2 Velocity
    {
        get
        {
            return velocity;
        }
    }

    // private fields:
    Vec2 _position;
    Vec2 _oldPosition;
    Barrel _barrel;

    public Barrel theBarrel
    {
        get
        {
            return _barrel;
        }
    }

    Vec2 acceleration;

    public Vec2 Acceleration
    {
        get
        {
            return acceleration;
        }
    }


    float _accelerationStrength = 0.1f;
    float _friction = 0.5f;

    float maxVelocity = 8f;

    public Tank(float px, float py) : base("assets/bodies/t34.png")
    {
        SetOrigin(width / 2, height / 2);
        _position.x = px;
        _position.y = py;
        _barrel = new Barrel(new Vec2(-20, 0), new Vec2(80, 0));
        AddChild(_barrel);
    }

    void Controls()
    {
        //has both accelerate forward and backward intentions so don't accelerate
        if (Input.GetKey(Key.K) == true && Input.GetKey(Key.I) == true)
        {
            acceleration = new Vec2(0, 0);
            acceleration += velocity * -_friction;
            return;
        }

        //press only up accelerate forward
        if (Input.GetKey(Key.I) && !Input.GetKey(Key.K))
        {
            acceleration.SetAngleDegree(rotation, _accelerationStrength);
        }

        //press only down accelerate backward
        if (Input.GetKey(Key.K) && !Input.GetKey(Key.I))
        {

            acceleration.SetAngleDegree(rotation + 180, _accelerationStrength);
        }

        //no player move control so no acceleration
        if (Input.GetKey(Key.K) == false && Input.GetKey(Key.I) == false)
        {
            acceleration = new Vec2(0, 0);
            acceleration += velocity * -_friction;
        }

        CheckTurn();

    }

    void CheckTurn()
    {
        //velocity.Length() > 0.3f
        if (true)
        {
            if (Input.GetKey(Key.J))
            {
                rotation--;
                rotation = rotation % 360;
                if (rotation < 0)
                {
                    rotation += 360;
                }
            }
            if (Input.GetKey(Key.L))
            {
                rotation++;
                rotation = rotation % 360;
                if (rotation < 0)
                {
                    rotation += 360;
                }
            }
        }
    }

    void Shoot()
    {
        if (Input.GetKeyDown(Key.F))
        {
            _barrel.Shoot(rotation);
        }
    }

    void UpdateScreenPosition()
    {
        x = _position.x;
        y = _position.y;
    }

    void DetectAndSolveLineCollision()
    {
        MyGame myGame = (MyGame)game;

        float radius = width / 2;

        //checking the lines
        for (int i = 0; i < myGame.GetNumberOfLines(); i++)
        {
            if (wordy)
            {
                Console.WriteLine("line collision detected");
            }

            LineSegment theLineSegment = myGame.GetLine(i);
            float ballDistance;
            Vec2 differenceVec = new Vec2(Position.x - theLineSegment.start.x, Position.y - theLineSegment.start.y);
            Vec2 normalVec = new Vec2(theLineSegment.end.x - theLineSegment.start.x, theLineSegment.end.y - theLineSegment.start.y);
            normalVec = normalVec.Normal();
            ballDistance = (differenceVec.Dot(normalVec));

            if (Math.Abs(ballDistance) < radius)
            {


                Vec2 desiredPoint = Position; //for a line calculation
                desiredPoint.SetAngleDegree(normalVec.GetAngleDegrees(), ballDistance - radius); //set the difference vector to the angle of the normal to the find desired point
                desiredPoint = Position - desiredPoint;
                Vec2 a = _oldPosition - desiredPoint; //for a line calculation
                float theA = a.Dot(normalVec);
                float theB = -velocity.Dot(normalVec);
                float theTimeOfImpact;

                if (wordy)
                {
                    Console.WriteLine("line b: " + theB);
                }

                if (theB <= 0) //if true, means ball already started moving away
                {
                    continue;
                }

                if (wordy)
                {
                    Console.WriteLine("line A: " + theA);
                }

                if (theA >= 0) //went past the segment if A <0
                {
                    theTimeOfImpact = theA / theB;
                }

                else if (theA >= -radius) //for solving ball at rest.
                {
                    theTimeOfImpact = 0;
                }

                else //if a< -radius, ball is past the line, so keep moving
                {
                    continue;
                }

                if (theTimeOfImpact <= 1)
                {

                    //segment line logic
                    //finding the distance along the segment

                    Vec2 POITest = Position + theTimeOfImpact * velocity;
                    Vec2 diffDistanceAlongAline = new Vec2(POITest.x - theLineSegment.start.x, POITest.y - theLineSegment.start.y);
                    Vec2 normalVeccc = normalVec;
                    normalVeccc.RotateDegrees(-90);
                    ballDistance = diffDistanceAlongAline.Dot(normalVeccc);
                    differenceVec = new Vec2(theLineSegment.end.x - theLineSegment.start.x, theLineSegment.end.y - theLineSegment.start.y);

                    //if the POI is within the segment
                    if (ballDistance >= 0 && ballDistance <= differenceVec.Length())
                    {
                        _position = _oldPosition + theTimeOfImpact * velocity;
                    }
                }
            }
        }

    }


    public void Update()
    {
        _oldPosition = Position;
        Controls();
        //semi-implict Euler integration:
        velocity += acceleration;

        if (velocity.Length() > maxVelocity)
        {
            velocity -= acceleration;
        }

        _position += velocity;
        Shoot();
        _barrel.ChangePostion(_position, new Vec2(0, 0));

        DetectAndSolveLineCollision();

        UpdateScreenPosition();

    }
}
