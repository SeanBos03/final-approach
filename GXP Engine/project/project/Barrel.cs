using GXPEngine;
using System;

//the tank barrel
class Barrel : Sprite
{
    Vec2 _position;
    Vec2 _positionOffset; //offset from the tank's center position (_position)
    Vec2 _bulletOffset; //offset from thetank's center position (_position)
    public Barrel(Vec2 _orginOffset, Vec2 bulletOffset) : base("assets/barrels/t34.png")
    {
        Vec2 origin = new Vec2(width / 2, height / 2);
        origin += _orginOffset;
        SetOrigin(origin.x, origin.y);
        _bulletOffset = bulletOffset;
    }

    //modify the barrel position
    public void ChangePostion(Vec2 position, Vec2 positionOffset)
    {
        _position = position;
        _positionOffset = positionOffset; //becuase barrel is a child of tank, this is the offset from the tank center position
        x = _positionOffset.x;
        y = _positionOffset.y;
    }

    public void Shoot(float tantRotation)
    {
        //we can't make the bullet a child of barrel or tank. it needs to be independent (a child of game)



        //spawn the bullet in front of the barrel (or whatever value -bulletOffset decides)

        //we need to consider the tank and barrel's rotation to calculate the correct position to spawn in front of the barrel
        Vec2 actualPositionOffset = _bulletOffset;
        actualPositionOffset.SetAngleDegree(rotation + parent.rotation, actualPositionOffset.Length());

        //bullet is also depends on the rotaton
        Vec2 bulletVelocity = new Vec2(10, 0);
        bulletVelocity.SetAngleDegree(rotation + parent.rotation, bulletVelocity.Length());

        //make the bullet itself
        MyGame myGame = (MyGame)game;
        Ball newBall = new Ball(10, _position + actualPositionOffset, bulletVelocity);
        myGame.AddMover(newBall);

        game.AddChild(newBall);

    }

    public void Update()
    {
        //the statements below calculate the barrel rotation. trying to rotate the barrel to make the barrel aim at the cursor position

        Vec2 pos = new Vec2(_positionOffset.x + parent.x, _positionOffset.y + parent.y);
        Vec2 mousePos = new Vec2(Input.mouseX + GameData.screenMovedX, Input.mouseY + GameData.screenMovedY); //Input.mouse displays the mouse at the screen position, but we need to mouse's position at the game. so we track the x and y that the camera have moved and adjust
       
        Vec2 direction = new Vec2(mousePos.x - pos.x, mousePos.y - pos.y);

        float directionAngle = direction.GetAngleDegrees();

        //make the direction angle value between 0 to 359
        if (directionAngle >= 360)
        {
            directionAngle = directionAngle % 360;
        }

        float targetRotation = directionAngle - parent.rotation; //the rotation the barrle needs to be in

        //make the targetRotation value between 0 to 359
        if (targetRotation >= 360)
        {
            targetRotation = targetRotation % 360;
        }


        //make the targetRotation value between 0 to 359 while making it remain negative
        if (targetRotation <= -360)
        {
            targetRotation *= -1;
            targetRotation = targetRotation % 360;
            targetRotation *= -1;
        }

        if (Input.GetKey(Key.T))
        {
            rotation = targetRotation;
            return;
        }

        //convert the targetAngle result to 0 - 360 degrees range (-pi - pi to 0 - 360)
        //helps with the calculation to determine the direction with a shorter distance
        if (targetRotation < 0)
        {
            targetRotation += 360;
        }


        //true, if the barrle is in the target rotation
        if (Math.Abs(rotation - targetRotation) < 1f)
        {
            return;
        }

        //determine if rotating clockwise is shorter than counterclockwise
        if ((targetRotation - rotation + 360) % 360 < (rotation - targetRotation + 360) % 360)
        {
            //rotating clockwise has a shorter distance
            rotation++;
        }

        else
        {
            rotation--;
        }

        //check if rotation value goes below the limit
        if (rotation <= 0)
        {
            rotation += 360;
        }

        //check if rotation value goes above the limit
        if (rotation >= 360)
        {
            rotation = rotation % 360;
        }
    }
}
