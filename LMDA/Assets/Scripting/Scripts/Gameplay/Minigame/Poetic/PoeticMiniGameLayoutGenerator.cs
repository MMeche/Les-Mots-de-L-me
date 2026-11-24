using System;
using System.Collections.Generic;
using System.Data;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.VFX;

public class PoeticMiniGameLayoutGenerator : MonoBehaviour
{
    public float ObstacleSpeed =0.1f;
    public float ObstacleChance = 0.5f;
    public int GapBetweenObstacles = 2;
    public int ObstacleNumber = 10;
    public int OrbNumber = 4;

    [SerializeField] GameObject _obstacles;
    [SerializeField] GameObject _orbs;
    [SerializeField] GameObject _lane;
    [SerializeField] GameObject _endTrigger;
    [SerializeField] float _xOffset = 2.5f;
    [SerializeField] float _yOffset = 2.5f;
    List<int>[] _path;  
    

    int[] _nextPossibleObstacle = new int[4];


    void OnEnable()
    {

        _path = new List<int>[4];
        _nextPossibleObstacle = new int[4];

        CreateLayout();
        VisualLayout();
        //PrintGrid();
    }


    void CreateLayout()
    {
        int obstaclesPlaced = 0;
        int row = 0;
        for (int i = 0; i < 4; i++)
        {
            _path[i] = new List<int>();
            _nextPossibleObstacle[i] = 0;
        }
        List<int[]> emptyPositions = new();
        while (ObstacleNumber - obstaclesPlaced > 0)
        {
            for (int i = 0; i < 4; i++)
            {
                int tmp = PlaceObstacles(i, row);
                obstaclesPlaced += tmp;
                if (tmp == 0)
                {
                    emptyPositions.Add(new int[] { i, row });
                }
        
            }
            row++;
        }
        for (int i = 0; i < OrbNumber; i++)
        {
            int k = UnityEngine.Random.Range(0, emptyPositions.Count);
            PlaceOrbs(emptyPositions[k][0], emptyPositions[k][1]);
        }
    }

    int PlaceObstacles(int i, int row)
    {
        float rdObstacle = UnityEngine.Random.Range(0f, 1f);

        if (_nextPossibleObstacle[i] <= row && rdObstacle < ObstacleChance)
        {
            if (CanStillReachEnd(_path,i,row))
            {
                _path[i].Add(1);
                _nextPossibleObstacle[i] = row + GapBetweenObstacles + 1;
                return 1;
            }
        }

        _path[i].Add(0);
        return 0;
    }

    int PlaceOrbs(int i, int row)
    {
        _path[i][row] = 2;
        return 0;
    }

    void PrintGrid()
    {
        int rows = _path[0].Count; // on suppose que toutes les voies ont la même longueur
        Debug.Log("=== Layout généré ===");
        for (int r = 0; r < rows; r++)
        {
            string line = "";
            for (int lane = 0; lane < 4; lane++)
            {
                line += _path[lane][r] == 1 ? "X " : ". ";
            }
            Debug.Log(line);
        }
    }

    void VisualLayout()
    {
        //Placing the obstacles on the layout
        for (int lane = 0; lane < 4; lane++)
        {
            for (int row = 0; row < _path[0].Count; row++)
            {
                if (_path[lane][row] == 1)
                {
                    GameObject newObstacle = Instantiate(_obstacles);
                    newObstacle.transform.SetParent(this.transform);
                    newObstacle.transform.position = new Vector3(_xOffset * row , 1.5f, _yOffset * (lane - 2) + 3.5f);
                }
                if (_path[lane][row] == 2)
                {
                    GameObject newOrb = Instantiate(_orbs);
                    newOrb.transform.SetParent(this.transform);
                    newOrb.transform.position = new Vector3(_xOffset * row , 1.5f, _yOffset * (lane-2)+3.5f);
                }
            }
            
             
             GameObject newLaneIndicator = Instantiate(_lane,transform);
             newLaneIndicator.transform.localScale = new Vector3(_path[0].Count * _xOffset, 0.1f, 1);
             newLaneIndicator.transform.position = new Vector3(0,1,_yOffset * (lane-2.5f)+3.5f);
            
        }
        GameObject firstLaneIndicator = Instantiate(_lane, transform);
        firstLaneIndicator.transform.localScale = new Vector3(_path[0].Count * _xOffset, 0.1f, 1);
        firstLaneIndicator.transform.position = new Vector3(0, 1, 4.75f+3.5f);

        // Placing the end trigger
        GameObject endTrigger = Instantiate(_endTrigger);
        endTrigger.transform.SetParent(this.transform);
        endTrigger.transform.position = new Vector3(_xOffset * _path[0].Count+1, 2.5f * _yOffset,  3.5f);
        endTrigger.transform.localScale = new Vector3(1, 10, 10); //Arbitrary values
    }

    bool CanStillReachEnd(List<int>[] path, int testLine, int testColumn)
    {
        int lines = path.Length;
        int columns = path[0].Count;

        // Cloner le path et remplir jusqu'à testColumn
        var simulatedPath = new List<int>[lines];
        for (int l = 0; l < lines; l++)
        {
            simulatedPath[l] = new List<int>(path[l]);
            while (simulatedPath[l].Count <= testColumn)
            {
                simulatedPath[l].Add(0);
            }
        }

        simulatedPath[testLine][testColumn] = 1; // obstacle simulé

        int maxCols = simulatedPath[0].Count;
        Queue<(int l, int c)> queue = new Queue<(int l, int c)>();
        bool[,] visited = new bool[lines, maxCols];

        // Ajouter comme départ toutes les lignes libres de la première colonne
        for (int l = 0; l < lines; l++)
        {
            if (simulatedPath[l][0] == 0)
            {
                queue.Enqueue((l, 0));
                visited[l, 0] = true;
            }
        }

        int[] dLine = { -1, 1, 0, 0 };
        int[] dCol  = {  0, 0, -1, 1 };

        while (queue.Count > 0)
        {
            var (l, c) = queue.Dequeue();

            // Si on atteint la dernière colonne existante, passage trouvé
            if (c == maxCols - 1) return true;

            for (int k = 0; k < 4; k++)
            {
                int nl = l + dLine[k];
                int nc = c + dCol[k];

                if (nl >= 0 && nl < lines && nc >= 0 && nc < maxCols)
                {
                    if (!visited[nl, nc] && simulatedPath[nl][nc] == 0)
                    {
                        visited[nl, nc] = true;
                        queue.Enqueue((nl, nc));
                    }
                }
            }
        }

        return false; // aucun chemin de la première à la dernière colonne
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += new Vector3(-ObstacleSpeed, 0, 0);
    }

    private void OnDisable()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
