using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
// Save ALL data related to the game itself.
public class PlayerData
{
    public bool lost = false;
    public int score, dimension;
    public int[] highScore = new int[3];
    public Dictionary<int[], int> positions = new Dictionary<int[], int>();

    public PlayerData(GameMaster master)
    {
        score = master.score;
        dimension = master.dimension;
        highScore[master.dimension - 3] = master.highScore;
        lost = master.lost;
        for (int i = 0; i < master.tiles.Count; i++)
        {
            int[] position = new int[3] { (int)master.tiles[i].transform.localPosition.x, (int)master.tiles[i].transform.localPosition.y, (int)master.tiles[i].transform.localPosition.z };
            switch(master.tiles[i].name[0])
            {
                case '1':
                    positions.Add(position, 0);
                    break;
                case '2':
                    positions.Add(position, 1);
                    break;
                case '3':
                    positions.Add(position, 2);
                    break;
                case '4':
                    positions.Add(position, 3);
                    break;
                case '5':
                    positions.Add(position, 4);
                    break;
                case '6':
                    positions.Add(position, 5);
                    break;
                case '7':
                    positions.Add(position, 6);
                    break;
            }
        }
    }
}
