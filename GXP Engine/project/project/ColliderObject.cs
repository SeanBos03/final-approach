using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//physcis for possibly all gameobjects
public class ColliderObject : GameObject
{
    // For ease of testing / changing, we assume every ball has the same acceleration (gravity) and bounciness
    public static Vec2 acceleration = new Vec2(0, 0);
    public static float bounciness = 0.98f;
    public static bool wordy = false; //if true, enable debug messages

    public static bool wordy1 = false; //if true, enable debug messages 1

    protected Vec2 position;
    protected Vec2 _oldPosition;
    protected Vec2 velocity;
    protected int radius;
    protected bool moving; //if the ball is moving or not (only collision detect and resolve should be applied)
    protected bool firstTime;
    protected List<CollisionInfo> _collisionList = new List<CollisionInfo>();
    protected float _density = 1;
    protected float mass;

    public Vec2 Position
    {
        get
        {
            return position;
        }
    }

    public Vec2 OldPosition
    {
        get
        {
            return _oldPosition;
        }
    }

    public float Mass
    {
        get
        {
            return mass;
        }
    }

    public float Radius
    {
        get
        {
            return radius;
        }
    }

    public Vec2 Velocity
    {
        get
        {
            return velocity;
        }

        set 
        {
            velocity = value;
        }
    }

    public ColliderObject(Vec2 pPosition, Vec2 pVelocity, int pRadius, bool pMoving, float pDensity=1) 
    {
        position = pPosition;
        velocity = pVelocity;
        radius = pRadius;
        moving = pMoving;
        _density = pDensity;
        // Mass = density * volume.
        // In 2D, we assume volume = area (=all objects are assumed to have the same "depth")
        mass = 4 * radius * radius * _density;
    }

    //AABB collision detection (collision with ball and a  
    protected void CheckCollisionBricks(MyGame myGame)
    {
        //checking the bricks
        for (int i = 0; i < myGame.GetNumberOfBricks(); i++)
        {
            Brick brick = myGame.GetBrick(i);

            if (Math.Abs(position.x - brick.x) <= (radius) + (brick.radius) &&
            Math.Abs(position.y - brick.y) <= (radius) + (brick.radius))
            {
                if (wordy)
                {
                    Console.WriteLine("brick collision detected");
                }

                float timeOfImpact;

                //we determine the direction the collision comes. since up and down & left and right directions have different ways of calculation
                if (position.y < brick.y - brick.radius)
                {
                    timeOfImpact = Math.Abs(_oldPosition.y - (brick.y - brick.radius - radius)) / Math.Abs(_oldPosition.y - position.y);

                    if (timeOfImpact <= 1 && timeOfImpact >= 0)
                    {
                        brick.Damage();
                        _collisionList.Add(new CollisionInfo(new Vec2(0, 0), brick, timeOfImpact));
                    }
                }

                else if (position.y > brick.y + brick.radius)
                {
                    timeOfImpact = Math.Abs(_oldPosition.y - (brick.y + brick.radius + radius)) / Math.Abs(_oldPosition.y - position.y);

                    if (timeOfImpact <= 1 && timeOfImpact >= 0)
                    {
                        if (timeOfImpact <= 1 && timeOfImpact >= 0)
                        {
                            brick.Damage();
                            _collisionList.Add(new CollisionInfo(new Vec2(0, 0), brick, timeOfImpact));
                        }
                    }
                }

                else if (position.x < brick.x - brick.radius)
                {
                    timeOfImpact = Math.Abs(_oldPosition.x - (brick.x - brick.radius - radius)) / Math.Abs(position.x - _oldPosition.x);
                    if (timeOfImpact <= 1 && timeOfImpact >= 0)
                    {
                        brick.Damage();
                        _collisionList.Add(new CollisionInfo(new Vec2(0, 0), brick, timeOfImpact));
                    }
                }

                else if (position.x > brick.x + brick.radius)
                {
                    timeOfImpact = Math.Abs(_oldPosition.x - (brick.x + brick.radius + radius)) / Math.Abs(position.x - _oldPosition.x);
                    if (timeOfImpact <= 1 && timeOfImpact >= 0)
                    {
                        brick.Damage();
                        _collisionList.Add(new CollisionInfo(new Vec2(0, 0), brick, timeOfImpact));
                    }
                }
            }
        }
    }

    //return the collision with smallest value of TOI in collision list
    protected CollisionInfo FindLowestTOICollision()
    {
        float TOI = 0;
        CollisionInfo theCollsion = null;

        for (int i = 0; i < _collisionList.Count; i++)
        {
            if (_collisionList[i].other is null)
            {
                continue;
            }

            if (wordy)
            {
                //debug messages
                if (_collisionList[i].other is Ball)
                {
                    Console.WriteLine("Type: Ball");
                }

                if (_collisionList[i].other is LineSegment)
                {
                    Console.WriteLine("Type: line");
                }

                if (_collisionList[i].other is Brick)
                {
                    Console.WriteLine("Type: brick");
                }

                //debug messages
                Console.WriteLine("TOI: " + _collisionList[i].timeOfImpact);
            }

            if (i == 0)
            {
                theCollsion = _collisionList[i];
                TOI = _collisionList[i].timeOfImpact;
                continue;
            }

            if (_collisionList[i].timeOfImpact < TOI)
            {
                theCollsion = _collisionList[i];
                TOI = _collisionList[i].timeOfImpact;
            }
        }
        return theCollsion; //return null if collision not found
    }

    protected virtual void ResolveCollision(CollisionInfo col)
    {
    }

    //Move the object 
    protected void MoveAndDetectAndResolveCollision()
    {
        position += velocity;

        CollisionInfo earilestCollision = FindEarliestCollision();

        if (earilestCollision != null)
        {
            ResolveCollision(earilestCollision);

            if (wordy)
            {
                Console.WriteLine("time: " + earilestCollision.timeOfImpact);
            }

            //for sliding behavior for gravity + multiple objects
            if (Math.Round(earilestCollision.timeOfImpact, 1) == 0 && firstTime)
            {
                firstTime = false;
                _collisionList.Clear();

                if (wordy)
                {
                    Console.WriteLine("doing calculation again");
                }

                MoveAndDetectAndResolveCollision(); //this would only be called once every frame as firstTime got set to false before this method call.
            }
        }
    }

    public void Step()
    {

        if (moving == false)
        {
            return;
        }

        firstTime = true;
        _collisionList.Clear();
        _oldPosition = position;
        velocity += acceleration;

        MoveAndDetectAndResolveCollision();
    }

    protected virtual CollisionInfo FindEarliestCollision()
    {
        MyGame myGame = (MyGame)game;

        CheckCollisionBricks(myGame);

        return FindLowestTOICollision();
    }
}