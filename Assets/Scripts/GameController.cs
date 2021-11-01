using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static bool firstPlayerTurn = true;
    public const string firstPlayerSymbol = "X";
    public const string secondPlayerSymbol = "O";

    [SerializeField] private GameObject _board3x3;
    [SerializeField] private GameObject _board5x5;
    [SerializeField] private Enemy _computerEnemy;
    private GameObject _chosenBoard;

    private static bool _gameIsOn = false;
    private string _firstPlayerType;
    private string _secondPlayerType;
    private static GameObject[,] _gridSpaces;
    public static Text[,] gridSpacesText;
    private int gridSize;


    // Update is called once per frame
    private void Update()
    {
        // if computer goes first
        if(_gameIsOn && firstPlayerTurn && _firstPlayerType == "computer" )
        {
            StartCoroutine(XPlayer());
        }
        // if computer goes second
        else if (_gameIsOn && !firstPlayerTurn && _secondPlayerType == "computer" )
        {
            StartCoroutine(OPlayer());
        }
    }

    private IEnumerator XPlayer()
    {
        _gameIsOn = false;

        string gameStatus = _computerEnemy.BestMoveXPlayer(gridSpacesText);
        yield return new WaitForSeconds(1f);
        _gameIsOn = !(gameStatus != "NOT FINISHED");
    }

    private IEnumerator OPlayer()
    {
        _gameIsOn = false;

        string gameStatus = _computerEnemy.BestMoveOPlayer(gridSpacesText);
        yield return new WaitForSeconds(1f);
        _gameIsOn = !(gameStatus != "NOT FINISHED");
    }

    public void StartGame(int boardSize, string firstPlayer, string secondPlayer)
    {
        firstPlayerTurn = true;
        ChooseBoard(boardSize);

        _firstPlayerType = firstPlayer;
        _secondPlayerType = secondPlayer;
        
        ActivateChoosenBoard();

        _gameIsOn = true;
    }

    private void ChooseBoard(int boardSize)
    {
        if (boardSize == 3)
        {
            _chosenBoard = _board3x3;
            gridSize = boardSize;
        }
        else if (boardSize == 5)
        {
            _chosenBoard = _board5x5;
            gridSize = boardSize;
        }

        int childAmount = _chosenBoard.transform.GetChild(1).childCount;
        _gridSpaces = new GameObject[gridSize, gridSize];
        gridSpacesText = new Text[gridSize, gridSize];

        for (int i = 0; i < childAmount; i++)
        {
            _gridSpaces[i / gridSize, i % gridSize] = _chosenBoard.transform.GetChild(1).GetChild(i).gameObject;
            gridSpacesText[i / gridSize, i % gridSize] = _gridSpaces[i / gridSize, i % gridSize].transform.GetChild(0).GetComponent<Text>();
        }
    }

    public void ActivateChoosenBoard() => _chosenBoard.SetActive(true);
    public void DeactivateChoosenBoard() => _chosenBoard.SetActive(false);

    public void ResetChoosenBoard()
    {
        _gameIsOn = false;
        int rowsAmount = gridSpacesText.GetLength(0);
        int columnsAmount = gridSpacesText.GetLength(1);

        for (int i = 0; i < rowsAmount; i++)
        {
            for (int j = 0; j < columnsAmount; j++)
            {
                gridSpacesText[i, j].text = "";
                _gridSpaces[i, j].GetComponent<Button>().interactable = true;
            }
        }
    }


    /// <summary>
    /// Function to change side of the game
    /// </summary>
    public static void ChangeSide() => firstPlayerTurn = !firstPlayerTurn;

    /// <summary>
    /// Function to check board for win or tie result
    /// </summary>
    /// <param name="boardToCheck"></param>
    /// <returns></returns>
    public static string CheckBoard(Text[,] boardToCheck)
    {
        int rowsAmount = boardToCheck.GetLength(0);
        int columnsAmount = boardToCheck.GetLength(1);
        string result = "NOT FINISHED";
        bool hasOpenSpots = false;
        bool hasWinner = false;
        //1.Check if same symbols in any row
        for (int i = 0; i < rowsAmount; i++)
        {
            for (int j = 1; j < columnsAmount; j++)
            {
                bool isEqualNeighbors = boardToCheck[i, j - 1].text == boardToCheck[i, j].text;
                bool isNotEmptyField = boardToCheck[i, j - 1].text != "" || boardToCheck[i, j].text != "";
                hasWinner = isEqualNeighbors && isNotEmptyField ? true : false;

                if (!hasWinner) break;
            }
            if (hasWinner) return boardToCheck[i, columnsAmount - 1].text;
        }
        //2.Check if same symbols in any column
        for (int j = 0; j < columnsAmount; j++)
        {
            for (int i = 1; i < rowsAmount; i++)
            {
                bool isEqualNeighbors = boardToCheck[i - 1, j].text == boardToCheck[i, j].text;
                bool isNotEmptyField = boardToCheck[i - 1, j].text != "" || boardToCheck[i, j].text != "";
                hasWinner = isEqualNeighbors && isNotEmptyField ? true : false;

                if (!hasWinner) break;
            }
            if (hasWinner) return boardToCheck[rowsAmount - 1, j].text;
        }
        //3.Check if same symbols in main diagonal
        for (int i = 1; i < rowsAmount; i++)
        {
            bool isEqualNeighbors = boardToCheck[i - 1, i - 1].text == boardToCheck[i, i].text;
            bool isNotEmptyField = boardToCheck[i - 1, i - 1].text != "" || boardToCheck[i, i].text != "";
            hasWinner = isEqualNeighbors && isNotEmptyField ? true : false;

            if (!hasWinner) break;
        }
        if(hasWinner) return boardToCheck[rowsAmount - 1, rowsAmount - 1].text;
        //4.Check if same symbols in antidiagonal
        for (int i = 1; i < rowsAmount; i++)
        {
            bool isEqualNeighbors = boardToCheck[i - 1, columnsAmount - i].text == boardToCheck[i, columnsAmount - i - 1].text;
            bool isNotEmptyField = boardToCheck[i - 1, columnsAmount - i].text != "" || boardToCheck[i, columnsAmount - i - 1].text != "";
            hasWinner = isEqualNeighbors && isNotEmptyField ? true : false;

            if (!hasWinner) break;
        }
        if (hasWinner) return boardToCheck[rowsAmount - 1, 0].text;
        //5.Check if it's tie
        for (int i = 0; i < columnsAmount; i++)
        {
            for (int j = 0; j < rowsAmount; j++)
            {
                if (boardToCheck[i, j].text == "")
                {
                    hasOpenSpots = true;
                }
            }
        }

        if (!hasWinner && !hasOpenSpots) return "TIE";
        
        return result;
    }


    /// <summary>
    /// Function to stop game after 
    /// </summary>
    /// <param name="firstPlayerWin"></param>
    public static void GameOver(string result)
    {
        _gameIsOn = false;
        Debug.Log("Game result: " + result);

        foreach (GameObject gridSpace in _gridSpaces)
        {
            gridSpace.GetComponent<Button>().interactable = false;
        }
    }
}
