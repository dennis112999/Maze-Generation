using UnityEngine;

namespace AlgorithmMaze
{
    public enum Direction
    {
        Left,
        Right,
        Front,
        Back
    }

    public class MazeCell : MonoBehaviour
    {
        [Header("Wall GameObject")]
        [SerializeField] private GameObject _leftWall;
        [SerializeField] private GameObject _rightWall;
        [SerializeField] private GameObject _frontWall;
        [SerializeField] private GameObject _backWall;

        [Header("Block GameObject")]
        [SerializeField] private GameObject _unisitedBlock;

        public bool IsVisited { get; private set; }

        public void Visit()
        {
            if(IsVisited)
            {
#if UNITY_EDITOR
                Debug.LogWarning("This Cell is already visited!");
#endif
                return;
            }

            IsVisited = true;
            _unisitedBlock.SetActive(false);
        }

        public void ClearWall(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    _leftWall.SetActive(false);
                    break;
                case Direction.Right:
                    _rightWall.SetActive(false);
                    break;
                case Direction.Front:
                    _frontWall.SetActive(false);
                    break;
                case Direction.Back:
                    _backWall.SetActive(false);
                    break;
                default:
#if UNITY_EDITOR
                    Debug.LogWarning("Invalid direction specified.");
#endif
                    break;
            }
        }
    }
}
