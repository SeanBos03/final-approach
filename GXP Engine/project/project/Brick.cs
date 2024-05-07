using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//a brick that's shaped like a square
public  class Brick : EasyDraw
{
    public Vec2 position;
    public Vec2 velocity;
    public float radius; //it's width / 2
    float _density = 1;

    public int health = 10; //health of the brick

    public float Mass
    {
        get
        {
            return 4 * radius * radius * _density;
        }
    }

    public Brick(int pRadius, Vec2 pPosition) : base(pRadius * 2, pRadius * 2)
    {
        SetOrigin(width / 2, height / 2);

        position = pPosition;

        x = position.x;
        y = position.y;

        velocity = new Vec2(0, 0);

        radius = width / 2;

        Fill(200);
        NoStroke();
        ShapeAlign(CenterMode.Min, CenterMode.Min);
        Rect(0, 0, width, height);
    }

    //we want to make the brick 'break' so we give it a health and destroy the brick if health is too low
    public void Damage()
    {
        health--;

        if (health < 0)
        {
            MyGame myGame = (MyGame)game;
            myGame.RemoveBrick(this);
            Destroy();
            return;
        }
    }
}