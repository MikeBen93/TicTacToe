using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    private bool _boardHasBeenChoosen = false;
    private bool _playersHasBeenChoosen = false;
    private bool _startTurnHasBeenChoosen = false;

    private string _firstPlayer;
    private string _secondPlayer;

    private int _choosenBoard = 3;

    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameController _gameController;

    [SerializeField] private Button _button3X3;
    [SerializeField] private Button _button5X5;

    [SerializeField] private Button _pvPButton;
    [SerializeField] private Button _pvCButton;
    [SerializeField] private Button _cvCButton;

    [SerializeField] private Text _turnChooseText;
    [SerializeField] private Button _playeTurnButton;
    [SerializeField] private Button _computerTurnButton;

    [SerializeField] private Button _resetButton;
    [SerializeField] private Button _tryAgainButton;

    private void Update()
    {
        if(_boardHasBeenChoosen && _playersHasBeenChoosen && _startTurnHasBeenChoosen)
        {
            StartGame();
        }
    }

    public void ChooseBoardButton(int boardSize)
    {
        _choosenBoard = boardSize;
        
        _button3X3.interactable = false;
        _button5X5.interactable = false;

        _boardHasBeenChoosen = true;
    }

    public void PlayersTypeButton(string gameType)
    {
        if(gameType == "PvP")
        {
            _firstPlayer = "player";
            _secondPlayer = "player";
            _playersHasBeenChoosen = true;
            _startTurnHasBeenChoosen = true;
        }
        else if (gameType == "CvC")
        {
            _firstPlayer = "computer";
            _secondPlayer = "computer";
            _playersHasBeenChoosen = true;
            _startTurnHasBeenChoosen = true;
        }
        else if (gameType == "PvC")
        {
            TurnChooseStart();
        }

        _pvPButton.interactable = false;
        _pvCButton.interactable = false;
        _cvCButton.interactable = false;
    }

    private void TurnChooseStart()
    {
        _turnChooseText.gameObject.SetActive(true);
        _playeTurnButton.gameObject.SetActive(true);
        _computerTurnButton.gameObject.SetActive(true);
    }

    public void FirstTurnChoose(string playerType)
    {
        if (playerType == "player")
        {
            _firstPlayer = "player";
            _secondPlayer = "computer";
        }
        else if (playerType == "computer")
        {
            _firstPlayer = "computer";
            _secondPlayer = "player";
        }

        _playeTurnButton.interactable = false;
        _computerTurnButton.interactable = false;

        _playersHasBeenChoosen = true;
        _startTurnHasBeenChoosen = true;
    }
    private void StartGame()
    {
        _mainMenu.SetActive(false);
        _gameController.StartGame(_choosenBoard, _firstPlayer, _secondPlayer);
        _resetButton.gameObject.SetActive(true);
        _tryAgainButton.gameObject.SetActive(true);
    }

    public void ResetParams()
    {
        _boardHasBeenChoosen = false;
        _playersHasBeenChoosen = false;
        _startTurnHasBeenChoosen = false;

        _button3X3.interactable = true;
        _button5X5.interactable = true;
        _pvPButton.interactable = true;
        _pvCButton.interactable = true;
        _cvCButton.interactable = true;

        _turnChooseText.gameObject.SetActive(false);
        _playeTurnButton.gameObject.SetActive(false);
        _computerTurnButton.gameObject.SetActive(false);

        _playeTurnButton.interactable = true;
        _computerTurnButton.interactable = true;

        _gameController.DeactivateChoosenBoard();
        _gameController.ResetChoosenBoard();
        _mainMenu.SetActive(true);
        _resetButton.gameObject.SetActive(false);
        _tryAgainButton.gameObject.SetActive(false);
    }

    public void TryAgain()
    {
        _gameController.RestartChoosenBoard();
    }
}
