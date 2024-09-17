using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using SadConsole;

public static class AudioManager
{
    public static Song? IntroMusic { get; set; }
    public static Song? MenuMusic { get; set; }
    public static Song? CombatInteractionMusic { get; set; }
    public static Song? LeavingHome { get; set; }

    public static SoundEffect? HitSFX { get; set; }
    public static SoundEffect? HealSFX { get; set; }
    public static SoundEffect? MalfunctionSFX { get; set; }

    public static SoundEffect? DoorOpenSFX { get; set; }
    public static SoundEffect? DoorCloseSFX { get; set; }

    public static SoundEffect? SpaceEngineSFX { get; set; }

    private static ContentManager? contentManager;

    // Initialize the ContentManager using the correct MonoGame services
    public static void Initialize()
    {
        System.Console.WriteLine("Current Directory: " + Environment.CurrentDirectory);
        System.Console.WriteLine("Content Path: " + Path.Combine(Environment.CurrentDirectory, "net8.0/Content"));

        // Initialize the ContentManager with MonoGame services accessed via SadConsole's Game instance
        contentManager = new ContentManager(Game.Instance.MonoGameInstance.Services, "Content/Audio");
        LoadContent();
    }

    private static void LoadContent()
    {
        try
        {
            IntroMusic = contentManager?.Load<Song>("Xnb/intro");
            MenuMusic = contentManager?.Load<Song>("Xnb/menu");
            CombatInteractionMusic = contentManager?.Load<Song>("Xnb/combatinteraction");
            LeavingHome = contentManager?.Load<Song>("Xnb/Leaving Home");

            HitSFX = contentManager?.Load<SoundEffect>("Xnb/hit");
            HealSFX = contentManager?.Load<SoundEffect>("Xnb/heal");
            MalfunctionSFX = contentManager?.Load<SoundEffect>("Xnb/malfunction");

            DoorOpenSFX = contentManager?.Load<SoundEffect>("Xnb/doorOpen");
            DoorCloseSFX = contentManager?.Load<SoundEffect>("Xnb/doorClose");

            SpaceEngineSFX = contentManager?.Load<SoundEffect>("Xnb/space_engine");

        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Error loading audio file: {ex}");
            throw; // Rethrow the exception to see the stack trace in the console
        }

    }
}