using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    public AudioSource sound;
    public AudioClip matchF, matchT;
    public bool lost = false, match = false, newGame = false;
    public GameObject gameOver, description, frame, block;
    public List<GameObject> tiles = new List<GameObject>();
    public int dimension, highScore, score;
    public List<(int, int, int)> positions = new List<(int, int, int)>();
    public Object[] blocks;
    public TextMeshProUGUI scores;
    public Transform wireFrame, blockSet, cam;

    private void Start()
    {
        BuildGraph();
        if (newGame)    LoadNewGame();
        else            LoadPlayer();
    }

    public void BuildGraph()
    {
        wireFrame.transform.localPosition = Vector3.one * (dimension % 2 != 0 ? (-dimension / 2) : (0.5f - dimension / 2));
        blockSet.transform.localPosition = Vector3.one * (dimension % 2 != 0 ? (-dimension / 2) : (0.5f - dimension / 2));
        for (int i = 0; i < dimension; i++)
            for (int j = 0; j < dimension; j++)
                for (int k = 0; k < dimension; k++)
                {
                    GameObject obj = Instantiate(block, wireFrame.position, wireFrame.rotation);
                    obj.transform.SetParent(wireFrame);
                    obj.transform.localPosition = new Vector3(i, j, k);
                }
        cam.position = new Vector3(0, -dimension * 0.45f, -dimension * 3f);
    }

    // Load Game.
    public void LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.ColoredAdvance";
        if (File.Exists(path))
        {
            PlayerData data = SaveSystem.LoadPlayer();
            if (!data.lost)
            {
                highScore = data.highScore[data.dimension - 3];
                Scoring(data.score);
                for (int i = 0; i < data.positions.Count; i++)
                {
                    GameObject objB = Instantiate((GameObject)blocks[data.positions.ElementAt(i).Value], transform.position, transform.rotation);
                    objB.transform.parent = blockSet;
                    objB.transform.localRotation = new Quaternion(0, 0, 0, 1);
                    objB.transform.localPosition = new Vector3(data.positions.ElementAt(i).Key[0], data.positions.ElementAt(i).Key[1], data.positions.ElementAt(i).Key[2]);
                    tiles.Add(objB);
                }
                Setup(true);
            }
        }
        else
            LoadNewGame();
    }

    // Start a new Game.
    public void LoadNewGame()
    {
        string path = Application.persistentDataPath + "/player.ColoredAdvance";
        if (File.Exists(path))
        {
            PlayerData data = SaveSystem.LoadPlayer();
            highScore = data.highScore[dimension - 3];
        }
        Setup(false);
    }

    // Set up scene. Instantiate 3D graph, frame, and boxes.
    public void Setup(bool loading)
    {
        // Spawn starting blocks.
        if (!loading)
            for (int i = 0; i < dimension; i++)
                StartBlocks(i);

        Scoring(0);
        SaveSystem.SavePlayer(this);
    }

    public void Guide()         =>  description.SetActive(!description.activeSelf);

    public void ReturnToGame() => SceneManager.LoadScene(dimension + "x" + dimension);

    public void ReturnToMain() => SceneManager.LoadScene("Menu");

    // Spawn starting blocks.
    public void StartBlocks(int i)
    {
        int y = Mathf.FloorToInt(Random.Range(0, dimension));
        int z = Mathf.FloorToInt(Random.Range(0, dimension));
        int block = Mathf.FloorToInt(Random.Range(0, 1.99f));

        GameObject objB = Instantiate((GameObject)blocks[block], transform.position, transform.rotation);
        objB.transform.parent = blockSet;
        objB.transform.localPosition = new Vector3(i, y, z);
        tiles.Add(objB);
    }

    public void Scoring(int add)
    {
        score += add;
        if (highScore < score) highScore = score;
        scores.text = "Score: " + score + "\nHigh Score: " + highScore;
    }

    public void Shift(string side)
    {
        scores.text = "Score: " + score + "\nHigh Score: " + highScore;
        //if (moves % 100 == 0 && Advertisement.IsReady("video")) Advertisement.Show();
        List<List<GameObject>> col = new List<List<GameObject>>();

        // Make a list per column. Then add blocks of each column to the respective list. Then add all lists to col list.
        for (int i = 0; i < dimension; i++)
        {
            for (int j = 0; j < dimension; j++)
            {
                // Make a list per column.
                List<GameObject> coll = new List<GameObject>();
                for (int k = 0; k < tiles.Count; k++)
                {
                    switch (side)
                    {
                        case "UP":
                        case "DOWN":
                            if (tiles[k].transform.localPosition.x == i && tiles[k].transform.localPosition.z == j) coll.Add(tiles[k]);
                            break;
                        case "LEFT":
                        case "RIGHT":
                            if (tiles[k].transform.localPosition.y == i && tiles[k].transform.localPosition.z == j) coll.Add(tiles[k]);
                            break;
                        case "FORWARD":
                        case "BACKWARD":
                            if (tiles[k].transform.localPosition.x == i && tiles[k].transform.localPosition.y == j) coll.Add(tiles[k]);
                            break;
                        default:
                            print("Error in clicking side.");
                            break;
                    }
                }
                // Add lists to col list.
                col.Add(coll);
            }
        }

        for (int i = 0; i < col.Count; i++)
        {
            // Sort the lists based on closest to farthest from side that is clicked.
            switch (side)
            {
                case "UP":
                    col[i].Sort(delegate (GameObject a, GameObject b) { return (a.transform.localPosition.y).CompareTo(b.transform.localPosition.y); });
                    break;
                case "DOWN":
                    col[i].Sort(delegate (GameObject a, GameObject b) { return (a.transform.localPosition.y).CompareTo(b.transform.localPosition.y); });
                    break;
                case "LEFT":
                    col[i].Sort(delegate (GameObject a, GameObject b) { return (a.transform.localPosition.x).CompareTo(b.transform.localPosition.x); });
                    break;
                case "RIGHT":
                    col[i].Sort(delegate (GameObject a, GameObject b) { return (a.transform.localPosition.x).CompareTo(b.transform.localPosition.x); });
                    break;
                case "FORWARD":
                    col[i].Sort(delegate (GameObject a, GameObject b) { return (a.transform.localPosition.z).CompareTo(b.transform.localPosition.z); });
                    break;
                case "BACKWARD":
                    col[i].Sort(delegate (GameObject a, GameObject b) { return (a.transform.localPosition.z).CompareTo(b.transform.localPosition.z); });
                    break;
                default:
                    print("Error in sorting blocks.");
                    break;
            }

            // Check for matching blocks in columns side-by-side IF there are 2 or more blocks in the column.
            if (col[i].Count > 1)
            {
                List<char> colorSeq = new List<char>();
                List<int> indexes = new List<int>();

                for (int j = 0; j < col[i].Count; j++)
                    colorSeq.Add(col[i][j].name[0]);

                // Go forward or backward.
                if (side == "DOWN" || side == "LEFT" || side == "FORWARD")
                {
                    // Order of least to greatest.
                    for (int j = 0; j < colorSeq.Count - 1; j++)
                        if (colorSeq[j] == colorSeq[j + 1])
                        {
                            indexes.Add(j);
                            j++;
                        }
                }
                else
                {
                    // Order of greatest to least.
                    for (int j = colorSeq.Count - 1; j > 0; j--)
                        if (colorSeq[j] == colorSeq[j - 1])
                        {
                            indexes.Add(j);
                            j--;
                        }
                }

                if (indexes.Count > 0)
                {
                    for (int j = 0; j < indexes.Count; j++)
                    {
                        match = true;
                        GameObject die1 = col[i][indexes[j]];
                        GameObject die2;
                        if (side == "DOWN" || side == "LEFT" || side == "FORWARD") die2 = col[i][indexes[j] + 1];
                        else die2 = col[i][indexes[j] - 1];
                        int block = int.Parse(die1.name[0].ToString());
                        Scoring(block != 7 ? block : 10);

                        // Instantiate new block.
                        if (block != 7)
                        {
                            GameObject objB = Instantiate((GameObject)blocks[block], transform.position, transform.rotation);
                            objB.transform.parent = blockSet;
                            objB.transform.localPosition = side == "DOWN" || side == "LEFT" || side == "FORWARD" ? die1.transform.localPosition : die2.transform.localPosition;
                            tiles.Add(objB);
                            col[i].Add(objB);
                        }
                    }
                    if (side == "DOWN" || side == "LEFT" || side == "FORWARD")
                    {
                        for (int j = indexes.Count - 1; j >= 0; j--)
                        {
                            GameObject die1 = col[i][indexes[j]];
                            GameObject die2 = col[i][indexes[j] + 1];

                            tiles.Remove(die2);
                            col[i].Remove(die2);
                            Destroy(die2);
                            tiles.Remove(die1);
                            col[i].Remove(die1);
                            Destroy(die1);
                        }
                    }
                    else
                    {
                        for (int j = 0; j < indexes.Count; j++)
                        {
                            GameObject die1 = col[i][indexes[j]];
                            GameObject die2 = col[i][indexes[j] - 1];

                            tiles.Remove(die1);
                            col[i].Remove(die1);
                            Destroy(die1);
                            tiles.Remove(die2);
                            col[i].Remove(die2);
                            Destroy(die2);
                        }
                    }
                }
            }

            // Shift blocks.
            switch (side)
            {
                case "UP":
                    col[i].Sort(delegate (GameObject a, GameObject b) { return (a.transform.localPosition.y).CompareTo(b.transform.localPosition.y); });
                    for (int j = 0; j < col[i].Count; j++)
                        col[i][col[i].Count - 1 - j].transform.localPosition = new Vector3(col[i][j].transform.localPosition.x, dimension - 1 - j, col[i][j].transform.localPosition.z);
                    break;
                case "DOWN":
                    col[i].Sort(delegate (GameObject a, GameObject b) { return (a.transform.localPosition.y).CompareTo(b.transform.localPosition.y); });
                    for (int j = 0; j < col[i].Count; j++)
                        col[i][j].transform.localPosition = new Vector3(col[i][j].transform.localPosition.x, j, col[i][j].transform.localPosition.z);
                    break;
                case "LEFT":
                    col[i].Sort(delegate (GameObject a, GameObject b) { return (a.transform.localPosition.x).CompareTo(b.transform.localPosition.x); });
                    for (int j = 0; j < col[i].Count; j++)
                        col[i][j].transform.localPosition = new Vector3(j, col[i][j].transform.localPosition.y, col[i][j].transform.localPosition.z);
                    break;
                case "RIGHT":
                    col[i].Sort(delegate (GameObject a, GameObject b) { return (a.transform.localPosition.x).CompareTo(b.transform.localPosition.x); });
                    for (int j = 0; j < col[i].Count; j++)
                        col[i][col[i].Count - 1 - j].transform.localPosition = new Vector3(dimension - 1 - j, col[i][j].transform.localPosition.y, col[i][j].transform.localPosition.z);
                    break;
                case "FORWARD":
                    col[i].Sort(delegate (GameObject a, GameObject b) { return (a.transform.localPosition.z).CompareTo(b.transform.localPosition.z); });
                    for (int j = 0; j < col[i].Count; j++)
                        col[i][j].transform.localPosition = new Vector3(col[i][j].transform.localPosition.x, col[i][j].transform.localPosition.y, j);
                    break;
                case "BACKWARD":
                    col[i].Sort(delegate (GameObject a, GameObject b) { return (a.transform.localPosition.z).CompareTo(b.transform.localPosition.z); });
                    for (int j = 0; j < col[i].Count; j++)
                        col[i][col[i].Count - 1 - j].transform.localPosition = new Vector3(col[i][j].transform.localPosition.x, col[i][j].transform.localPosition.y, dimension - 1 - j);
                    break;
                default:
                    print("Error in shifting blocks.");
                    break;
            }
        }
        if (tiles.Count >= dimension * dimension * dimension)
        {
            // GAME OVER.
            Debug.Log("OVER 1");
            lost = true;
            gameOver.SetActive(true);
            Debug.Log("OVER 2");
            sound.clip = match ? matchT : matchF;
            sound.Play();
            match = false;
            // Save game after each move.
            Debug.Log("OVER 3");
            SaveSystem.SavePlayer(this);
            Debug.Log("OVER 4");
        }
        else
        {
            Spawn();
            sound.clip = match ? matchT : matchF;
            sound.Play();
            match = false;
            // Save game after each move.
            SaveSystem.SavePlayer(this);
        }
    }

    // Spawn new block.
    public void Spawn()
    {
        List<Vector3> pos = new List<Vector3>();
        int x = Mathf.FloorToInt(Random.Range(0, dimension));
        int y = Mathf.FloorToInt(Random.Range(0, dimension));
        int z = Mathf.FloorToInt(Random.Range(0, dimension));
        int block = Mathf.FloorToInt(Random.Range(0, 1.99f));

        for (int i = 0; i < tiles.Count; i++)
            pos.Add(tiles[i].transform.localPosition);

        while(pos.Contains(new Vector3(x, y, z)))
        {
            x = Mathf.FloorToInt(Random.Range(0, dimension));
            y = Mathf.FloorToInt(Random.Range(0, dimension));
            z = Mathf.FloorToInt(Random.Range(0, dimension));
        }

        GameObject objB = Instantiate((GameObject)blocks[block], transform.position, transform.rotation);
        objB.transform.parent = blockSet;
        objB.transform.localPosition = new Vector3(x, y, z);
        tiles.Add(objB);
    }
}
