using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

public static class AudioManager
{
    public static Dictionary<string, SoundEffect?> SFXDict = new Dictionary<string, SoundEffect?>();
    public static Dictionary<string, Song?> SongDict = new Dictionary<string, Song?>();

    private static ContentManager? contentManager;

    public static void Initialize()
    {
        contentManager = new ContentManager(Game.Instance.MonoGameInstance.Services, "Content/Audio/Xnb");
        LoadContent();

    }

    public static void PlaySong(string song, bool isRepeating = true)
    {
        MediaPlayer.Stop();
        MediaPlayer.Play(SongDict[song]);
        MediaPlayer.IsRepeating = isRepeating;
    }

    public static void PlaySFX(string sfx, bool isLooped = false)
    {
        var sfxInstance = SFXDict[sfx].CreateInstance();
        sfxInstance.Play();
        sfxInstance.IsLooped = isLooped;
    }

    public static SoundEffectInstance GetSFX(string sfx)
    {
        return SFXDict[sfx].CreateInstance();
    }

    private static void LoadContent()
    {
        string pathSong = Path.Combine("Content", "Audio", "Xnb");
        ProcessDirectory(pathSong);
    }

    // Process all files in the directory passed in, recurse on any directories
    // that are found, and process the files they contain.
    public static void ProcessDirectory(string targetDirectory)
    {
        // Process the list of files found in the directory.
        string[] fileEntries = Directory.GetFiles(targetDirectory);
        foreach (string fileName in fileEntries)
            ProcessFile(fileName);

        // Recurse into subdirectories of this directory.
        string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
        foreach (string subdirectory in subdirectoryEntries)
            ProcessDirectory(subdirectory);
    }

    // Insert logic for processing found files here.
    public static void ProcessFile(string path)
    {
        // System.Console.WriteLine("Processed file '{0}'.", path);
        // System.Console.WriteLine(Path.GetFileName(path));

        if (path.Contains("SFX"))
        {
            var fileName = Path.GetFileName(path).Split('.')[0];
            var audiomangerPath = Path.Combine("SFX", fileName);

            var sFX = contentManager?.Load<SoundEffect>(audiomangerPath);
            SFXDict.Add(fileName, sFX);
        }
        else if (path.Contains("Song"))
        {
            var file = Path.GetFileName(path);
            var extension = file.Split('.')[1];
            if (extension == "xnb")
            {
                var fileName = file.Split('.')[0];
                var audiomangerPath = Path.Combine("Song", fileName);

                var song = contentManager?.Load<Song>(audiomangerPath);
                SongDict.Add(fileName, song);
            }
        }


    }
}