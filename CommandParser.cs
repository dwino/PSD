using System;
using Microsoft.Xna.Framework.Media;

namespace Balance;

public static class CommandParser
{
    public static void Parse(string command)
    {
        var splitCommand = command.Split(':');

        if (splitCommand[0] == "play_sound")
        {
            PlaySoundCommandParser.Parse(splitCommand[1]);
        }

    }

}

public static class PlaySoundCommandParser
{
    public static void Parse(string command)
    {
        MediaPlayer.Pause();

        var sfxInstance = AudioManager.SFXDict[command].CreateInstance();
        sfxInstance.Play();
        while (sfxInstance.State == Microsoft.Xna.Framework.Audio.SoundState.Playing)
        {

        }

        MediaPlayer.Resume();

    }
}