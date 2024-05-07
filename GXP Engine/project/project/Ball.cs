using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Security.AccessControl;
using System.Threading;
using GXPEngine;
using GXPEngine.Core;
using Microsoft.Win32;

public class Ball : EasyDraw
{
	public static bool drawDebugLine = false; //enable debug lines
    int health = 10; //health of the brick

    public int Health
    {
        get
        {
            return health;
        }

        set
        {
            health = value;
        }
    }

	Arrow _velocityIndicator; //for dispalying ball velocity (for debugging)
    bool isBrick = false; //if true, then the ball is a circle shaped brick
    public bool IsBrick
    {
        get
        {
            return isBrick;
        }

        set
        {
            isBrick = value;
        }
    }

    ColliderCircle ballCollision; //handles the ball's physics
    public ColliderCircle BallCollision
    {
        get
        {
            return ballCollision;
        }
    }

    bool moving;

    public bool Moving
    {
        get
        {
            return moving;
        }
    }

    public Ball(int pRadius, Vec2 pPosition, Vec2 pVelocity = new Vec2(), bool moving = true) : base(pRadius * 2 + 1, pRadius * 2 + 1)
    {
        this.moving = moving;
        ballCollision = new ColliderCircle(this, pPosition, pVelocity, pRadius, moving);
        UpdateScreenPosition();
        SetOrigin(ballCollision.Radius, ballCollision.Radius);
        Draw(230, 200, 0); //drawing thhe circle
        _velocityIndicator = new Arrow(ballCollision.Position, new Vec2(0, 0), 10); //for displaying velocity vector
        AddChild(_velocityIndicator);
    }
    public void ChangeColor(int red, int green, int blue)
    {
        Draw((byte)red, (byte)green, (byte)blue);
    }

    //drawing the circle shape with color
    void Draw(byte red, byte green, byte blue) {
		Fill (red, green, blue);
		Stroke (red, green, blue);
		Ellipse (ballCollision.Radius, ballCollision.Radius, 2*ballCollision.Radius, 2*ballCollision.Radius);
	}

	void UpdateScreenPosition() {
		x = ballCollision.Position.x;
		y = ballCollision.Position.y;
	}

    //ball moving logic
	public void Step () {
        ballCollision.Step();
        UpdateScreenPosition();
		ShowDebugInfo();
	}

    void Update()
    {
        //if the ball is a brick. because brick breaks, if the health is too low, ball will destroy itself
        if (health < 0 && isBrick == true) 
        {
            MyGame myGame = (MyGame)game;
            myGame.RemoveMover(this);
            Destroy();
            return;
        }
    }

	void ShowDebugInfo() {
        
		if (drawDebugLine) {
			((MyGame)game).DrawLine (ballCollision.OldPosition, ballCollision.Position);
		}
        //uncomment these below will display velocity vector of the ball
        /*
		_velocityIndicator.startPoint = position;
		_velocityIndicator.vector = velocity;
        */
	}
}

