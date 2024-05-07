using GXPEngine;
using System;

class Bullet : Sprite
{
    // public fields & properties:
    public Vec2 position
    {
        get
        {
            return _position;
        }
    }
    public Vec2 velocity;

    Vec2 _position;
    Vec2 _positionOffset;
    public Bullet(Vec2 pPosition, Vec2 positionOffset, Vec2 pVelocity, float rotation, float tankRotation) : base("assets/bullet.png")
    {
        _position = pPosition;
        _positionOffset = positionOffset;
        velocity = pVelocity;
        SetOrigin(width / 2, height / 2);
        this.rotation = tankRotation + rotation;
        //     Console.WriteLine("tank: " + tankRotation + " | " + "barrel: " + rotation + " | " + "all: " + (rotation + _tankRotation));
    }

    void UpdateScreenPosition()
    {
        Vec2 actualPositionOffset = _positionOffset;
        actualPositionOffset.SetAngleDegree(rotation, actualPositionOffset.Length());
        x = _position.x + actualPositionOffset.x;
        y = _position.y + actualPositionOffset.y;
    }

    public void Update()
    {
        velocity.SetAngleDegree(rotation, velocity.Length()); //make the bullet move in the direction of it's rotation
        _position += velocity;
        UpdateScreenPosition();
    }
}
