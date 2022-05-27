using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class OpeningsBook
{
    private List<Opening> openingList = new List<Opening>();
    private readonly string openingsJsonPath = Application.dataPath + "/Stockfish/openings.json";

    public OpeningsBook ()
    {
        LoadOpenings();
    }

    private void LoadOpenings ()
    {
        using (StreamReader r = new StreamReader(openingsJsonPath))
        {
            string json = r.ReadToEnd();
            openingList = JsonConvert.DeserializeObject<List<Opening>>(json);
        }
    }

    public string GetOpeningName (string moves)
    {
        foreach (Opening opening in openingList)
        {
            if (opening.Moves == moves)
                return opening.Name;
        }

        return null;
    }

    public int GetOpeningsAmount ()
    {
        return openingList.Count;
    }

    private class Opening
    {
        public string ECO { get; }
        public string Name { get; }
        public string Moves { get; }

        public Opening (string eco, string name, string moves)
        {
            ECO = eco;
            Name = name;
            Moves = moves;
        }
    }
}
