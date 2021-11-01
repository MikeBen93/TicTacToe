using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSpace : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Text _buttonText;
    public void SetSymbol()
    {
        if(GameController.firstPlayerTurn) _buttonText.text = GameController.firstPlayerSymbol;
        else _buttonText.text = GameController.secondPlayerSymbol;

        _button.interactable = false;

        string currentStatus = GameController.CheckBoard(GameController.gridSpacesText);

        if (currentStatus != "NOT FINISHED") GameController.GameOver(currentStatus);
        else GameController.ChangeSide();

    }
}
