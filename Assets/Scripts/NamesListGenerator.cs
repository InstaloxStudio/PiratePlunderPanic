using System.Collections.Generic;
using UnityEngine;

public class NamesListGenerator : MonoBehaviour
{

    public string SelectedName;

    public List<string> creatureNames = new List<string>()
    {
        // Cute flying creatures
        "Flutterwing",
        "Skywhisp",
        "Breezeflutter",
        "Featherdancer",
        "Suncatcher",
        "Starling",
        "Cloudskimmer",
        "Petalwing",
        "Dawnbreeze",
        "Moonbeam",
        "Songbird",
        "Twinklefeather",
        "Skyglider",
        "Zephyrwing",
        "Lumina",
        "Whispertail",
        "Sunbeam",
        "Silkfeather",
        "Glimmerwing",
        "Dewdrop",
        "Windchime",
        "Rosewing",
        "Sunshower",
        "Fluffcloud",
        "Radiantfly",
        "Daydreamer",
        "Whisperflap",
        "Springwing",
        "Stardust",
        "Celestial",

        // Scary Dr. Seuss-style names
        "Shiverbeast",
        "Grimsnarl",
        "Ghoulgrove",
        "Dreadmuncher",
        "Snickertooth",
        "Wraithwobble",
        "Spindlecrawler",
        "Creepspore",
        "Gloomgobbler",
        "Shriekling",
        "Nightmaregnasher",
        "Wobblewhisper",
        "Fangfrenzy",
        "Twitchtangle",
        "Howlthorn",
        "Jittersquirm",
        "Slithershade",
        "Chillsnapper",
        "Driftbone",
        "Gobblegloom",
        "Quiverclaw",
        "Skittershade",
        "Rumblegrin",
        "Wretchwraith",
        "Squealshriek",
        "Grumblegloom",
        "Hushmaw",
        "Snickerwretch",
        "Whiskerwisp",
        "Gruesomeeker",
        "Gnashgloom",
        "WidowStalker"
    };


    public string GetRandomName()
    {
        return creatureNames[Random.Range(0, creatureNames.Count)];
    }

    public void PickName()
    {
        SelectedName = GetRandomName();
    }
}
