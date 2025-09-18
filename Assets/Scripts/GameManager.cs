using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("���� ����")]
    public int playerScore = 0;
    public int itemsColledted = 0;

    [Header("UI ����")]
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
        Debug.Log($"������ ����! (�� : {itemsColledted} ��");
    }

    public void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "���� : " + playerScore;
        }

        if (tiemCountText != null)
        {
            tiemCountText.text = "������ : " + tiemCountText + "��";
        }
    }
}
