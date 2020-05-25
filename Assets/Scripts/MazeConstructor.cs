
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeConstructor
{
    private float _placementThreshold;
    private int[,] maze;
    private int _rMax = 0;
    private int _cMax = 0;

    public MazeConstructor()
    {
        _placementThreshold = 0.1f;
    }

    public int[,] GenerateNewMaze(int sizeRows, int sizeCols)
    {
        maze = new int[sizeRows, sizeCols];
        _rMax = maze.GetUpperBound(0);
        _cMax = maze.GetUpperBound(1);

        for (int i = 1; i < _rMax; i += 2)
        {
            for (int j = 1; j < _cMax; j += 2)
            {
                if (i == 0 && j == 0)
                {
                    maze[i, j] = 0;
                }
                else
                {
                    if (Random.value > _placementThreshold)
                    {
                        maze[i, j] = 1;

                        int a = Random.value > 0.5f ? 0 : (Random.value < .5 ? -1 : 1);
                        int b = a != 0 ? 0 : (Random.value < .5 ? -1 : 1);

                        maze[i + a, j + b] = 1;
                    }
                }
            }
        }
        maze[_rMax, _cMax] = 0;
 
        return maze;
    }
}


