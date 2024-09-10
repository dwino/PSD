using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using SadConsole;

public static class AudioManager
{
    public static Song? IntroMusic { get; set; }
    public static Song? MenuMusic { get; set; }
    public static Song? CombatInteractionMusic { get; set; }

    public static SoundEffect? HitSFX { get; set; }
    public static SoundEffect? HealSFX { get; set; }




    private static ContentManager? contentManager;

    // Initialize the ContentManager using the correct MonoGame services
    public static void Initialize()
    {
        System.Console.WriteLine("Current Directory: " + Environment.CurrentDirectory);
        System.Console.WriteLine("Content Path: " + Path.Combine(Environment.CurrentDirectory, "net8.0/Content"));

        // Initialize the ContentManager with MonoGame services accessed via SadConsole's Game instance
        contentManager = new ContentManager(Game.Instance.MonoGameInstance.Services, "Content");
        LoadContent();
    }

    private static void LoadContent()
    {
        try
        {
            IntroMusic = contentManager?.Load<Song>("intro");
            MenuMusic = contentManager?.Load<Song>("menu");
            CombatInteractionMusic = contentManager?.Load<Song>("combatinteraction");

            HitSFX = contentManager?.Load<SoundEffect>("hit");
            HealSFX = contentManager?.Load<SoundEffect>("heal");



        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Error loading audio file: {ex}");
            throw; // Rethrow the exception to see the stack trace in the console
        }

    }
}