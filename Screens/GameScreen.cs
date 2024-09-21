using System.Runtime.CompilerServices;
using Balance.Ui;
using SadConsole.Input;

namespace Balance.Screens;

public class GameScreen
{

    private Animation? _currentAnimation { get; set; }
    private Queue<Animation> _animationsQueue { get; set; }
    public GameScreen()
    {
        _animationsQueue = new Queue<Animation>();
        _animationsQueue.Enqueue(new GameScreenIntroAnimation());
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
            Program.Engine.Draw();
        }

    }
}