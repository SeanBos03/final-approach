using System;
using GXPEngine;
using System.Drawing;
using System.Collections.Generic;
using TiledMapParser;

//git update test
public class MyGame : Game
{	
	bool _stepped = false;
	bool _paused = false;
	int _stepIndex = 0;
	int _startSceneNumber = 0;

	Canvas _lineContainer = null;

    List<Ball> _movers;
	List<LineSegment> _lines;
	List<Brick> _bricks;

    EasyDraw _text;
    EasyDraw _textCrood;
    EasyDraw _textAccelertion;
    EasyDraw _textRotation;
    Tank theTank;

    public int GetNumberOfLines() {
		return _lines.Count;
	}
    public int GetNumberOfBricks()
    {
        return _bricks.Count;
    }

    public Brick GetBrick(int index)
    {
        if (index >= 0 && index < _bricks.Count)
        {
            return _bricks[index];
        }
        return null;
    }

    public LineSegment GetLine(int index) {
		if (index >= 0 && index < _lines.Count) {
			return _lines [index];
		}
		return null;	
	}

	public int GetNumberOfMovers() {
		return _movers.Count;
	}

	public void RemoveMover(Ball theBall)
	{
		_movers.Remove(theBall);
	}

    public void RemoveBrick(Brick theBrick)
    {
        _bricks.Remove(theBrick);
    }

    public void AddMover(Ball theMover)
    {
		_movers.Add(theMover);
    }

    public Ball GetMover(int index) {
		if (index >= 0 && index < _movers.Count) {
			return _movers [index];
		}
		return null;
	}
    public void DrawLine(Vec2 start, Vec2 end) {
		_lineContainer.graphics.DrawLine(Pens.White, start.x, start.y, end.x, end.y);
	}

	public MyGame () : base(800, 600, false, false)
	{
		//for the lines (border)
		_lineContainer = new Canvas(width, height);
		AddChild(_lineContainer);

		targetFps = 60;

		//initialize the lists
		_movers = new List<Ball>();
		_lines = new List<LineSegment>();
		_bricks = new List<Brick>();

		//load scene
		LoadScene(_startSceneNumber);
        PrintInfo();

		//adding debug display texts
        _text = new EasyDraw(200, 25);
        _text.TextAlign(CenterMode.Min, CenterMode.Min);
        _textCrood = new EasyDraw(1000, 25);
        _textCrood.TextAlign(CenterMode.Min, CenterMode.Min);
        _textAccelertion = new EasyDraw(1000, 25);
        _textAccelertion.TextAlign(CenterMode.Min, CenterMode.Min);
        _textRotation = new EasyDraw(1000, 25);
        _textRotation.TextAlign(CenterMode.Min, CenterMode.Min);
        AddChild(_text);
        AddChild(_textCrood);
        AddChild(_textAccelertion);
        AddChild(_textRotation);

        //add tank
        theTank = new Tank(width / 2, height / 2);
        AddChild(theTank);

    }
	
	void AddLine (Vec2 start, Vec2 end) {
		LineSegment line = new LineSegment (start, end, 0xff00ff00, 4);
		AddChild (line);
		_lines.Add (line);
	}

	void LoadScene(int sceneNumber) {
		_startSceneNumber = sceneNumber;
		// remove previous scene:
		foreach (Ball mover in _movers) {
			mover.Destroy();
		}
		_movers.Clear();
		foreach (LineSegment line in _lines) {
			line.Destroy();
		}
		_lines.Clear();

        foreach (Brick brick in _bricks)
        {
            brick.Destroy();
        }
        _bricks.Clear();

		
		//add borders all scene will have
        AddLine(new Vec2(50, height - 20), new Vec2(200, 60)); //left
        AddLine(new Vec2(200, 60), new Vec2(50, height - 20));

        AddLine (new Vec2 (200, 60), new Vec2 (width-20, 50)); //top
        AddLine(new Vec2(width - 20, 50), new Vec2(200, 60));

        AddLine (new Vec2 (width-20, 50), new Vec2 (width-60, height-110));  //right
        AddLine(new Vec2(width - 60, height - 110), new Vec2(width - 20, 50));

        switch (sceneNumber) {
			// BALL / BALL COLLISION SCENES:
			case 1: // one moving ball (medium speed), one fixed ball.
                ColliderObject.acceleration.SetXY(0, 0);
				_movers.Add(new Ball(30, new Vec2(200, 300), new Vec2(38, 0)));
				_movers.Add(new Ball(30, new Vec2(400, 340)));
				break;				
			case 2: // one moving ball (high speed), one fixed ball.
                ColliderObject.acceleration.SetXY(0, 0);
				_movers.Add(new Ball(30, new Vec2(200, 300), new Vec2(72, 0)));
				_movers.Add(new Ball(30, new Vec2(400, 340)));
				break;
			case 3: // many balls:
                ColliderObject.acceleration.SetXY(0, 0);

				_movers.Add(new Ball(30, new Vec2(200, 300), new Vec2(3,4)));
				_movers.Add(new Ball(50, new Vec2(600, 300), new Vec2(5,4)));
				_movers.Add(new Ball(40, new Vec2(400, 300), new Vec2(-3,4)));
				_movers.Add(new Ball(15, new Vec2(500, 200), new Vec2(7,4)));
				_movers.Add(new Ball(20, new Vec2(300, 400), new Vec2(-3,4)));
				_movers.Add(new Ball(30, new Vec2(200, 200), new Vec2(3,4)));
				_movers.Add(new Ball(50, new Vec2(600, 200), new Vec2(5,4)));
				_movers.Add(new Ball(40, new Vec2(300, 200), new Vec2(-3,4)));
				_movers.Add(new Ball(15, new Vec2(400, 100), new Vec2(7,4)));
				_movers.Add(new Ball(20, new Vec2(500, 300), new Vec2(-3,4)));
				break;
			case 4: // one moving ball bouncing on some fixed balls:
                ColliderObject.acceleration.SetXY(0, 1);
				_movers.Add(new Ball(30, new Vec2(200, 470), moving: false));
				_movers.Add(new Ball(30, new Vec2(260, 500), moving: false));
				_movers.Add(new Ball(30, new Vec2(320, 500), moving: false));
				_movers.Add(new Ball(30, new Vec2(380, 470), moving: false));
				_movers.Add(new Ball(30, new Vec2(400, 302), new Vec2(0, 0)));
				break;
			// LINE SEGMENT SCENES:
			case 5: // line segment:
                ColliderObject.acceleration.SetXY(0, 0);
				_movers.Add(new Ball(30, new Vec2(200, 300), new Vec2(20, 0)));
				AddLine(new Vec2(290, 250), new Vec2(455, 350));
                AddLine(new Vec2(455, 350), new Vec2(290, 250));
                _movers.Add(new Ball(0, new Vec2(290 - 1, 250 - 1), new Vec2(0, 0), false));
                _movers.Add(new Ball(0, new Vec2(455 + 1, 350 + 1), new Vec2(0, 0), false));
                break;
			case 6: // polygon:
                ColliderObject.acceleration.SetXY(0, 1);
				_movers.Add(new Ball(30, new Vec2(400, 180), new Vec2(0, 0)));
				AddLine(new Vec2(290, 250), new Vec2(455, 350));
				AddLine(new Vec2(455, 350), new Vec2(600, 250));
				AddLine(new Vec2(600, 250), new Vec2(450, 300));
				AddLine(new Vec2(450, 300), new Vec2(290, 250));
				break;	
			default:


                //adding some brick squares and circles

                ColliderObject.acceleration.SetXY(0, 0);

				Ball newBall1 = new Ball(30, new Vec2(250, 450), new Vec2(0, 0), false);
                newBall1.IsBrick = true;
                newBall1.ChangeColor(252, 11, 3);
                _movers.Add(newBall1);

                _bricks.Add(new Brick(40, new Vec2(200, 340)));


                _bricks.Add(new Brick(10, new Vec2(200, 540)));

                _bricks.Add(new Brick(30, new Vec2(550, 200)));


                AddLine(new Vec2(50, height - 20), new Vec2(1000, 800));
                AddLine(new Vec2(1000, 800), new Vec2(50, height - 20));
                _movers.Add(new Ball(0, new Vec2(1000, 800), new Vec2(0, 0), false));

                AddLine(new Vec2(800, 300), new Vec2(1500, 900));
                AddLine(new Vec2(1500, 900), new Vec2(800, 300));
                _movers.Add(new Ball(0, new Vec2(800, 300), new Vec2(0, 0), false));
                _movers.Add(new Ball(0, new Vec2(1500, 900), new Vec2(0, 0), false));

                Ball newBall3 = new Ball(40, new Vec2(1000, 550), new Vec2(0, 0), false);
                newBall3.IsBrick = true;
                newBall3.ChangeColor(252, 11, 3);
                _movers.Add(newBall3);

                _movers.Add(new Ball(0, new Vec2(width - 60, height - 110), new Vec2(0, 0), false));

                Ball newBall2 = new Ball(40, new Vec2(620, 330), new Vec2(0, 0), false);
                newBall2.IsBrick = true;
                newBall2.ChangeColor(252, 11, 3);
                _movers.Add(newBall2);

                break;
		}		
		_stepIndex = -1;
		foreach (Ball b in _movers) {
			AddChild(b);
		}

        foreach (Brick b in _bricks)
        {
            AddChild(b);
        }
    }

	/****************************************************************************************/

	void PrintInfo() {
		Console.WriteLine("Hold spacebar to slow down the frame rate.");
		Console.WriteLine("Use arrow keys and backspace to set the gravity.");
		Console.WriteLine("Press S to toggle stepped mode.");
		Console.WriteLine("Press P to toggle pause.");
		Console.WriteLine("Press D to draw debug lines.");
		Console.WriteLine("Press C to clear all debug lines.");
		Console.WriteLine("Press R to reset scene, and numbers to load different scenes");
		Console.WriteLine("Press B to toggle high/low bounciness.");
		Console.WriteLine("Press W to toggle extra output text.");
        Console.WriteLine("Press I and K to move the tank");
        Console.WriteLine("Press J and L to rotate the tank");
        Console.WriteLine("Press T to move the barrel instantly to the cursor");
        Console.WriteLine("Press F to shoot");
    }

	void HandleInput() {
		targetFps = Input.GetKey(Key.SPACE) ? 5 : 60;
		if (Input.GetKeyDown (Key.UP)) {
            ColliderObject.acceleration.SetXY (0, -1);
		}
		if (Input.GetKeyDown (Key.DOWN)) {
            ColliderObject.acceleration.SetXY (0, 1);
		}
		if (Input.GetKeyDown (Key.LEFT)) {
            ColliderObject.acceleration.SetXY (-1, 0);
		}
		if (Input.GetKeyDown (Key.RIGHT)) {
            ColliderObject.acceleration.SetXY (1, 0);
		}
		if (Input.GetKeyDown (Key.BACKSPACE)) {
            ColliderObject.acceleration.SetXY (0, 0);
		}
        if (Input.GetKeyDown (Key.S)) {
			_stepped ^= true;
		}
		if (Input.GetKeyDown (Key.D)) {
			Ball.drawDebugLine ^= true;
		}
		if (Input.GetKeyDown (Key.P)) {
			_paused ^= true;
		}
		if (Input.GetKeyDown (Key.B)) {
            ColliderObject.bounciness = 1.5f - ColliderObject.bounciness;
		}
		if (Input.GetKeyDown(Key.W)) {
			ColliderObject.wordy ^= true;
		}
		if (Input.GetKeyDown (Key.C)) {
			_lineContainer.graphics.Clear (Color.Black);
		}
		if (Input.GetKeyDown (Key.R)) {
			LoadScene (_startSceneNumber);
		}
		for (int i = 0; i< 10; i++) {
			if (Input.GetKeyDown (48 + i)) {
				LoadScene (i);
			}
		}
	}

	void StepThroughMovers() {
		if (_stepped) { // move everything step-by-step: in one frame, only one mover moves
			_stepIndex++;
			if (_stepIndex >= _movers.Count) {
				_stepIndex = 0;
			}
			if (_movers [_stepIndex].Moving) {
				_movers [_stepIndex].Step ();
			}
		} else { // move all movers every frame
			foreach (Ball mover in _movers) {
				if (mover.Moving) {
					mover.Step ();
				}
			}
		}
	}

	void Update () {
		UseCamera();
        HandleInput();
		if (!_paused) {
			StepThroughMovers ();
		}

        _textCrood.Clear(Color.Transparent);
        _textCrood.Text("Crood: " + "(" + Math.Round(theTank.x, 0) + " , " + Math.Round(theTank.y, 0) + ")" 
			+ "(" + Math.Round(theTank.theBarrel.x, 0) + " , " + Math.Round(theTank.theBarrel.y, 0) + ")"
            , 150, 0);

        _textAccelertion.Clear(Color.Transparent);
        _textAccelertion.Text("Speed: " + Math.Round(theTank.Velocity.Length(), 2)
            , 400, 0);

        _textRotation.Clear(Color.Transparent);
        _textRotation.Text("Rotation: " + theTank.rotation + " , " + theTank.theBarrel.rotation
            , 600, 0);

	}

	static void Main() {
		new MyGame().Start();
	}

    void UseCamera()
    {
		float boundaryValueX = game.width / 2;
        float boundaryValueY = game.height / 2;


        //first determine if the camera moves, then determine the max distance the camera can move
        //handling player moving right
        if (theTank.x + x > boundaryValueX && x > -1 * ((game.width * 6) - 800))
        {
			float ogX = x;
            x = boundaryValueX - theTank.x;
			GameData.screenMovedX += ogX - x;
        }

        //handling player moving left
        if (theTank.x + x < game.width - boundaryValueX && x < 0)
        {
            float ogX = x;
            x = game.width - boundaryValueX - theTank.x;
            GameData.screenMovedX += ogX - x;
        }

        //handling player moving up
        if (theTank.y + y < game.height - boundaryValueY && y < 0)
        {
            float ogY = y;
            y = game.height - boundaryValueY - theTank.y;
            GameData.screenMovedY += ogY - y;
        }

        //handling player moving down
        if (theTank.y + y > boundaryValueY && y > -1 - (game.height * 2) - 100)
        {
            float ogY = y;
            y = boundaryValueY - theTank.y;
            GameData.screenMovedY += ogY - y;
        }
    }
}