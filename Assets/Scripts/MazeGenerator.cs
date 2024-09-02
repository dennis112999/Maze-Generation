using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace AlgorithmMaze
{
    /// <summary>
    /// Maze Generator
    /// </summary>
    public class MazeGenerator : MonoBehaviour
    {
        [SerializeField] MazeCell _mazeCellPrefab;

        [SerializeField] private int _mazeWidth;
        [SerializeField] private int _mazeDepth;

        private MazeCell[,] _mazeGrid;

        private static readonly Vector2Int[] Directions =
{
            new Vector2Int(1, 0), // Right
            new Vector2Int(-1, 0), // Left
            new Vector2Int(0, 1), // Front
            new Vector2Int(0, -1) // Back
        };

        #region MonoBehaviour

        IEnumerator Start()
        {
            GenerateMazeGrid();

            yield return GenerateMaze(null, _mazeGrid[0, 0]);
        }

        #endregion MonoBehaviour

        /// <summary>
        /// Generate Maze Grid
        /// </summary>
        private void GenerateMazeGrid()
        {
            if (_mazeWidth <= 0 || _mazeDepth <= 0)
            {
#if UNITY_EDITOR
                Debug.LogError("Maze width and depth must be greater than 0.");
#endif
                return;
            }

            _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];

            for (int x = 0; x < _mazeWidth; x++)
            {
                for (int z = 0; z < _mazeDepth; z++)
                {
                    _mazeGrid[x, z] = Instantiate(_mazeCellPrefab, new Vector3(x, 0, z), Quaternion.identity);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="preiousCell"></param>
        /// <param name="currentCell"></param>
        /// <returns></returns>
        private IEnumerator GenerateMaze(MazeCell preiousCell, MazeCell currentCell)
        {
            currentCell.Visit();
            ClearWalls(preiousCell, currentCell);

            yield return new WaitForSeconds(0.05f);

            MazeCell nextCell;

            while ((nextCell = GetNextUnvisitedCell(currentCell)) != null)
            {
                yield return GenerateMaze(currentCell, nextCell);
            }

        }

        /// <summary>
        /// Find Next Unvisited Cell
        /// </summary>
        /// <param name="currentCell"></param>
        /// <returns>Next Unvisited Cell</returns>
        private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
        {
            var unvisitedCells = GetUnvisitedCells(currentCell).ToList();

            if (unvisitedCells.Count == 0) return null;

            int randomIndex = Random.Range(0, unvisitedCells.Count);
            return unvisitedCells[randomIndex];
        }

        private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
        {
            int x = (int)currentCell.transform.position.x;
            int z = (int)currentCell.transform.position.z;

            foreach (var direction in Directions)
            {
                int newX = x + direction.x;
                int newZ = z + direction.y;

                if (newX >= 0 && newX < _mazeWidth && newZ >= 0 && newZ < _mazeDepth)
                {
                    var neighborCell = _mazeGrid[newX, newZ];
                    if (!neighborCell.IsVisited)
                    {
                        yield return neighborCell;
                    }
                }
            }
        }

        private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
        {
            if (previousCell == null) return;

            Vector2Int direction = new Vector2Int(
                (int)(currentCell.transform.position.x - previousCell.transform.position.x),
                (int)(currentCell.transform.position.z - previousCell.transform.position.z)
            );

            if (direction == Vector2Int.right)
            {
                previousCell.ClearWall(Direction.Right);
                currentCell.ClearWall(Direction.Left);
                return;
            }
            else if (direction == Vector2Int.left)
            {
                previousCell.ClearWall(Direction.Left);
                currentCell.ClearWall(Direction.Right);
                return;
            }
            else if (direction == Vector2Int.up)
            {
                previousCell.ClearWall(Direction.Front);
                currentCell.ClearWall(Direction.Back);
                return;
            }
            else if (direction == Vector2Int.down)
            {
                previousCell.ClearWall(Direction.Back);
                currentCell.ClearWall(Direction.Front);
                return;
            }
        }
    }

}