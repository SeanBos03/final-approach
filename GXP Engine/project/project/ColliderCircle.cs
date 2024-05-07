using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//physcis for a ball object
public class ColliderCircle : ColliderObject
{
    Ball thisBallObject;
    public ColliderCircle(Ball pBallObject, Vec2 pPosition, Vec2 pVelocity, int pRadius, bool pMoving, float pDensity=1) : base (pPosition, pVelocity, pRadius, pMoving, pDensity)
    {
        thisBallObject = pBallObject;
    }

    //collision ball / line segment
    void CheckCollisionLines(MyGame myGame)
    {
        //checking the lines
        for (int i = 0; i < myGame.GetNumberOfLines(); i++)
        {


            LineSegment theLineSegment = myGame.GetLine(i);
            float ballDistance;
            Vec2 differenceVec = new Vec2(position.x - theLineSegment.start.x, position.y - theLineSegment.start.y);
            Vec2 normalVec = new Vec2(theLineSegment.end.x - theLineSegment.start.x, theLineSegment.end.y - theLineSegment.start.y);
            normalVec = normalVec.Normal();
            ballDistance = (differenceVec.Dot(normalVec));

            if (Math.Abs(ballDistance) < radius)
            {
                if (wordy)
                {
                    Console.WriteLine("line collision detected");
                }

                Vec2 desiredPoint = position; //for a line calculation
                desiredPoint.SetAngleDegree(normalVec.GetAngleDegrees(), ballDistance - radius); //set the difference vector to the angle of the normal to the find desired point
                desiredPoint = position - desiredPoint;
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

                    Vec2 POITest = position + theTimeOfImpact * velocity;
                    Vec2 diffDistanceAlongAline = new Vec2(POITest.x - theLineSegment.start.x, POITest.y - theLineSegment.start.y);
                    Vec2 normalVeccc = normalVec;
                    normalVeccc.RotateDegrees(-90);
                    ballDistance = diffDistanceAlongAline.Dot(normalVeccc);
                    differenceVec = new Vec2(theLineSegment.end.x - theLineSegment.start.x, theLineSegment.end.y - theLineSegment.start.y);

                    //if the POI is within the segment
                    if (ballDistance >= 0 && ballDistance <= differenceVec.Length())
                    {
                        _collisionList.Add(new CollisionInfo(normalVec, theLineSegment, theTimeOfImpact));
                    }
                }
            }
        }
    }

    //continous circle/circle collision
    void CheckCollisionBalls(MyGame myGame)
    {
        // Checking the balls
        for (int i = 0; i < myGame.GetNumberOfMovers(); i++)
        {
            Ball mover = myGame.GetMover(i);

            if (mover != thisBallObject)
            {
                Vec2 relativePosition = position - mover.BallCollision.Position;
                Vec2 relativeVelocity = velocity - mover.BallCollision.Velocity;

                if (relativePosition.Length() < radius + mover.BallCollision.Radius)
                {

                    Vec2 relativePosNormalized = Vec2.Normalized(relativePosition);
                    Vec2 relativeVelocityNormalized = Vec2.Normalized(relativeVelocity);

                    float collisionAngle = relativePosNormalized.Dot(relativeVelocityNormalized);

                    collisionAngle = Vec2.RadToDegree(collisionAngle);

                    //if angle bewteen the ball collision is less than 90 degress, not a real collision
                    if (collisionAngle > 90)
                    {
                        continue;
                    }

                    if (wordy)
                    {
                        Console.WriteLine("possible ball collision detected");
                    }

                    //using the quadratic formula to detect collision and find TOI
                    Vec2 normal = new Vec2(position.x - mover.x, position.y - mover.y);
                    normal.Normalize();
                    float a = (float)Math.Pow(velocity.Length(), 2);
                    Vec2 relativePositionVecMul2 = new Vec2(relativePosition.x * 2, relativePosition.y * 2);
                    float b = relativePositionVecMul2.Dot(relativeVelocity);
                    float c = (float)Math.Pow(relativePosition.Length(), 2) - (float)Math.Pow((radius + mover.BallCollision.Radius), 2);
                    c *= -1;

                    if (wordy)
                    {
                        Console.WriteLine("c: " + c);
                    }

                    if (c < 0) //is true, is overlapping
                    {
                        if (b < 0)
                        {
                            if (wordy)
                            {
                                Console.WriteLine("not moving away");
                            }
                            _collisionList.Add(new CollisionInfo(normal, mover, 0));
                            continue;
                        }

                        else
                        {
                            if (wordy)
                            {
                                Console.WriteLine("is moving away"); //it means ball are alreay moving away from each other, so a false collision
                            }

                            continue;
                        }
                    }

                    if (wordy)
                    {
                        Console.WriteLine("a: " + a);
                    }

                    if (Math.Round(a) == 0) //ball speed is 0
                    {
                        continue;
                    }

                    float d = (float)Math.Sqrt(Math.Pow(b, 2) + 4 * a * c);

                    if (wordy)
                    {
                        Console.WriteLine("d: " + a);
                    }

                    if (d < 0) //collision should have positive solution
                    {
                        continue;
                    }

                    float timeOfImpact = (-b - (float)Math.Sqrt(Math.Pow(b, 2) + 4 * a * c)) / (2 * a); //only care about the min solution (minus sign only)
                    timeOfImpact *= -1;

                    if (wordy)
                    {
                        Console.WriteLine("TOI: " + timeOfImpact);
                    }

                    if (0 <= timeOfImpact && timeOfImpact < 1)
                    {
                        if (mover.IsBrick)
                        {
                            mover.Health--;
                        }

                        _collisionList.Add(new CollisionInfo(normal, mover, timeOfImpact));
                    }

                    else //invalid TOI (should be btw 0 to 1)
                    {
                        if (wordy1)
                        {
                            Console.WriteLine("Anomaly TOI: " + timeOfImpact);
                        }
                    }
                }
            }
        }
    }

    protected override void ResolveCollision(CollisionInfo col)
    {
        if (col == null)
        {
            return;
        }

        //ball resolve logic
        if (col.other is Ball)
        {
            //POI calculation and apply Newton law
            Ball other = (Ball)col.other;
            Vec2 centerOfMass = (mass * velocity + other.BallCollision.Mass * other.BallCollision.Velocity) / (Mass + other.BallCollision.Mass);
            Vec2 relativePosition = position - other.BallCollision.Position;
            Vec2 colNormal = Vec2.Normalized(relativePosition);
            colNormal.Normalize();

            position = _oldPosition + (col.timeOfImpact * velocity);

            velocity = velocity - (1 + bounciness) * (velocity - centerOfMass).Dot(colNormal) * colNormal;
            other.BallCollision.Velocity = other.BallCollision.Velocity - (1 + bounciness) * (other.BallCollision.Velocity - centerOfMass).Dot(colNormal) * colNormal;
        }

        //line resolve logic
        if (col.other is LineSegment)
        {
            position = _oldPosition + col.timeOfImpact * velocity;
            velocity.Reflect(col.normal, bounciness);
        }

        //brick resolve logic
        if (col.other is Brick)
        {
            Brick brick = (Brick)col.other;

            Vec2 centerOfMass = (Mass * velocity + brick.Mass * brick.velocity) / (Mass + brick.Mass);
            Vec2 momentum = centerOfMass - bounciness * (velocity - centerOfMass);
            Vec2 POI = _oldPosition + (col.timeOfImpact * velocity);

            //resolve in different way depends on the direction the collision comes from
            if (position.y < brick.y - brick.radius)
            {
                position.y += POI.y - position.y;
                velocity.y = momentum.y;
            }

            else if (position.y > brick.y + brick.radius)
            {
                position.y += POI.y - position.y;
                velocity.y = momentum.y;
            }

            else if (position.x < brick.x - brick.radius)
            {
                position.x -= position.x - POI.x;
                velocity.x = momentum.x;
            }

            else if (position.x > brick.x + brick.radius)
            {
                position.x -= position.x - POI.x;
                velocity.x = momentum.x;
            }
        }
    }

    protected override CollisionInfo FindEarliestCollision()
    {
        MyGame myGame = (MyGame)game;

        CheckCollisionBalls(myGame);
        CheckCollisionLines(myGame);
        CheckCollisionBricks(myGame);

        return FindLowestTOICollision();
    }
}