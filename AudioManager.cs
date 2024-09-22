using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

public static class AudioManager
{
    public static Dictionary<string, SoundEffect?> SFXDict = new Dictionary<string, SoundEffect?>();
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

    public static void Initialize()
    {
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

            SFXDict.Add("faucet_running", HealSFX);

        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Error loading audio file: {ex}");
            throw;
        }

    }
}