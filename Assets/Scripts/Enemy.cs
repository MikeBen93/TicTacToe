using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public string BestMoveXPlayer(Text[,] boardGridText)
    {
        int rowAmount = boardGridText.GetLength(0);
        int columnAmount = boardGridText.GetLength(1);

        float bestScore = -Mathf.Infinity;
        float alpha = -Mathf.Infinity;
        float beta = Mathf.Infinity;
        int rowMove = 0;
        int columnMove = 0;
        for (int i = 0; i < rowAmount; i++)
        {
            for (int j = 0; j < columnAmount; j++)
            {
                //if the spot available
                if(boardGridText[i,j].text == "")
                {
                    // trying putting in this spot
                    boardGridText[i, j].text = GameController.firstPlayerSymbol;
                    float score = Minimax(boardGridText, 0, false, alpha, beta);
                    boardGridText[i, j].text = "";
                    //check if this board position is the best
                    if (score > bestScore)
                    {
                        bestScore = score;
                        rowMove = i;
                        columnMove = j;
                    }
                    alpha = Mathf.Max(alpha, bestScore);
                    if (beta <= alpha) break;
                }
            }
        }
        boardGridText[rowMove, columnMove].text = GameController.firstPlayerSymbol;
        string currentStatus = GameController.CheckBoard(boardGridText);

        if (currentStatus != "NOT FINISHED") GameController.GameOver(currentStatus);
        else GameController.ChangeSide();

        return currentStatus;
    }

    public string BestMoveOPlayer(Text[,] boardGridText)
    {
        int rowAmount = boardGridText.GetLength(0);
        int columnAmount = boardGridText.GetLength(1);

        float bestScore = Mathf.Infinity;
        float alpha = -Mathf.Infinity;
        float beta = Mathf.Infinity;
        int rowMove = 0;
        int columnMove = 0;
        for (int i = 0; i < rowAmount; i++)
        {
            for (int j = 0; j < columnAmount; j++)
            {
                //if the spot available
                if (boardGridText[i, j].text == "")
                {
                    // trying putting in this spot
                    boardGridText[i, j].text = GameController.secondPlayerSymbol;
                    float score = Minimax(boardGridText, 0, true, alpha, beta);
                    boardGridText[i, j].text = "";
                    //check if this board position is the best
                    if (score < bestScore)
                    {
                        bestScore = score;
                        rowMove = i;
                        columnMove = j;
                    }
                    beta = Mathf.Min(beta, bestScore);
                    if (beta <= alpha) break;
                }
            }
        }

        boardGridText[rowMove, columnMove].text = GameController.secondPlayerSymbol;
        string currentStatus = GameController.CheckBoard(boardGridText);

        if (currentStatus != "NOT FINISHED") GameController.GameOver(currentStatus);
        else GameController.ChangeSide();

        return currentStatus;
    }


    private static float Minimax(Text[,] boardGridText, int depth, bool isMaximizing, float alpha, float beta)
    {
        if (depth >= 6) return 0;
        int rowAmount = boardGridText.GetLength(0);
        int columnAmount = boardGridText.GetLength(1);

        switch (GameController.CheckBoard(boardGridText))
        {
            case GameController.firstPlayerSymbol:
                return 1;
            case GameController.secondPlayerSymbol:
                return -1;
            case "TIE":
                return 0;
        }

        if (isMaximizing)
        {
            float bestScore = -Mathf.Infinity;
            for (int i = 0; i < rowAmount; i++)
            {
                for (int j = 0; j < columnAmount; j++)
                {
                    if (boardGridText[i, j].text == "")
                    {
                        boardGridText[i, j].text = GameController.firstPlayerSymbol;
                        float score = Minimax(boardGridText, depth + 1, false, alpha, beta);
                        boardGridText[i, j].text = "";

                        bestScore = Mathf.Max(bestScore, score);
                        alpha = Mathf.Max(alpha, bestScore);
                        if (beta <= alpha) break;
                    }
                }
            }
            return bestScore;
        } 
        else
        {
            float bestScore = Mathf.Infinity;
            for (int i = 0; i < rowAmount; i++)
            {
                for (int j = 0; j < columnAmount; j++)
                {
                    if (boardGridText[i, j].text == "")
                    {
                        boardGridText[i, j].text = GameController.secondPlayerSymbol;
                        float score = Minimax(boardGridText, depth + 1, true, alpha, beta);
                        boardGridText[i, j].text = "";

                        bestScore = Mathf.Min(bestScore, score);
                        beta = Mathf.Min(beta, bestScore);
                        if (beta <= alpha) break;
                    }
                }
            }
            return bestScore;
        }
    }
}
