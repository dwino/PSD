using System.Diagnostics;
using System.Runtime.CompilerServices;
using Balance.Ui;
using SadConsole.Configuration;
using SadConsole.Input;

namespace Balance.Screens;

public class GameScreen
{

    private Animation? _currentAnimation;
    private Queue<Animation> _animationsQueue;
    private BackgroundAnimation _backgroundAnimation;
    public GameScreen()
    {
        _animationsQueue = new Queue<Animation>();
        _animationsQueue.Enqueue(new GameScreenIntroAnimation());
        _backgroundAnimation = new BackgroundAnimation();
    }

    public void AddAnimation(Animation animation)
    {
        if (_currentAnimation == null || !_currentAnimation.IsRunning)
        {
            _currentAnimation = animation;
        }
        else
        {
            _animationsQueue.Enqueue(animation);
        }
    }

    public void ProcessKeyboard(Keyboard keyboard)
    {
        if (_currentAnimation != null && _currentAnimation.IsRunning)
        {
            _currentAnimation.ProcessKeyboard(keyboard);
        }
        else
        {
            Program.Engine.ProcessKeyboard(keyboard);
        }
    }

    public void Update()
    {
        if (_currentAnimation == null || (_currentAnimation != null && !_currentAnimation.IsRunning))
        {
            _currentAnimation = null;
            if (_animationsQueue.Count > 0)
            {
                _currentAnimation = _animationsQueue.Dequeue();
                _currentAnimation.IsRunning = true;
            }
        }
    }

    public void DrawGameScreen()
    {

        if (_currentAnimation != null && _currentAnimation.IsRunning)
        {
            _currentAnimation.Play();
        }
        else
        {
            Program.Ui.Clear();
            _backgroundAnimation.Play();
            Program.Engine.Draw();
        }

    }
}

public class BackgroundAnimation
{
    private List<Star> _stars;
    public BackgroundAnimation()
    {
        _stars = new List<Star>();
    }

    public void Play()
    {
        if (_stars.Count < 35)
        {
            _stars.Add(new Star());
        }
        int i = 0;
        while (i < _stars.Count)
        {
            if (_stars[i].Life <= 0)
            {
                _stars.RemoveAt(i);
            }
            else
            {
                i++;
            }
        }
        foreach (var star in _stars)
        {
            Program.Ui.Print(star.X, star.Y, ((char)249).ToString(), star.GetColor(), Color.Black);
            star.AdvanceLife();
        }
    }
}

public class Star
{
    public Star()
    {
        Point = new Point(Helper.Rnd.Next(1, GameSettings.GAME_WIDTH), Helper.Rnd.Next(1, GameSettings.GAME_HEIGHT));
        Life = Helper.Rnd.NextSingle();
        Speed = Helper.Rnd.NextSingle() / 200;
    }

    public Point Point { get; set; }

    public int X => Point.X; //- Program.Engine.Player.Position.X;
    public int Y => Point.Y; //- Program.Engine.Player.Position.Y;
    public float Life { get; set; }
    public float Speed { get; set; }

    public void AdvanceLife()
    {
        Life -= Speed;
    }

    public Color GetColor()
    {
        return new Color(Life, Life, Life);
    }
}

// public class BackgroundAnimation
// {
//     private List<PointFloat> stars;
//     public BackgroundAnimation()
//     {
//         stars = new List<PointFloat>();
//         for (int i = 0; i < 20; i++)
//         {
//             if (i % 2 == 0)
//             {
//                 stars.Add(new PointFloat(Helper.Rnd.NextSingle() * GameSettings.GAME_WIDTH - 1, 1, Helper.Rnd.NextSingle() / 2 * -1));
//             }
//             else
//             {
//                 stars.Add(new PointFloat(GameSettings.GAME_WIDTH - 1, Helper.Rnd.NextSingle() * GameSettings.GAME_HEIGHT - 1, Helper.Rnd.NextSingle() / 2 * -1));
//             }
//         }
//     }

//     public void Play()
//     {
//         foreach (var star in stars)
//         {
//             while (!star.InBounds())
//             {
//                 if (Helper.Rnd.Next(2) == 0)
//                 {
//                     star.X = Helper.Rnd.NextSingle() * GameSettings.GAME_WIDTH - 1;
//                     star.Y = 1;
//                 }
//                 else
//                 {
//                     star.X = GameSettings.GAME_WIDTH - 1;
//                     star.Y = Helper.Rnd.NextSingle() * GameSettings.GAME_HEIGHT - 1;
//                 }
//             }

//             var pointToDraw = star.GetPoint();

//             Program.Ui.Print(pointToDraw.X, pointToDraw.Y, ".", Color.White, Color.Black);
//             star.Increase();
//         }
//     }
// }

// public class PointFloat(float x, float y, float inc)
// {
//     public float X { get; set; } = x;
//     public float Y { get; set; } = y;
//     public float Inc { get; set; } = inc;
//     public Point GetPoint()
//     {
//         return new Point((int)X, (int)Y);
//     }

//     public bool InBounds()
//     {
//         return (X >= 0 && X <= GameSettings.GAME_WIDTH) && (Y >= 0 && Y <= GameSettings.GAME_HEIGHT);
//     }

//     public void Increase()
//     {
//         X += Inc;
//         Y -= Inc;
//     }
// }
