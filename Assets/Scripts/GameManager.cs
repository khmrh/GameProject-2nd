using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("게임 상태")]
    public int playerScore = 0;
    public int itemsColledted = 0;

    [Header("UI 참조")]
    public Text scoreText;
    public Text tiemCountText;
    public Text gmaeStatusText;

    public static GameManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Collectitem()
    {
        itemsColledted++;
        Debug.Log($"아이템 수집! (총 : {itemsColledted} 개");
    }

    public void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "점수 : " + playerScore;
        }

        if (tiemCountText != null)
        {
            tiemCountText.text = "아이템 : " + tiemCountText + "개";
        }
    }
}
