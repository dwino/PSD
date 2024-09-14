

namespace Balance;

public interface IEvent
{
    bool IsAvailable(GameEngine _game);

    void WhenAvailable();
}
