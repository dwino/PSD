using System;

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

        AudioManager.SFXDict[command].CreateInstance().Play();

    }
}