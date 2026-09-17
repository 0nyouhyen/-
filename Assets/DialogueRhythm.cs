using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueRhythm : MonoBehaviour
{
    [Header("대사를 표시할 텍스트")]
    public TMP_Text customerText;
    public TMP_Text playerText;

    [Header("손님 대사 박자 간격")]
    public float beatInterval = 0.75f;

    string[] customerDialogue =
    {
        "안녕하세요.",
        "전..",
        "오늘",
        "너무 힘든 일이 있었어요."
    };

    string[] playerDialogue =
    {
        "반갑습니다.",
        "오늘",
        "어떤 하루였고,",
        "무슨 일이 있었나요?"
    };

    int currentBeat = 0;
    int playerBeat = 0;

    bool playerTurn = false;

    void Start()
    {
        customerText.text = "";
        playerText.text = "";

        StartCoroutine(ShowCustomerDialogue());
    }

    IEnumerator ShowCustomerDialogue()
    {
        while (currentBeat < customerDialogue.Length)
        {
            customerText.text += customerDialogue[currentBeat] + " ";

            currentBeat++;

            yield return new WaitForSeconds(beatInterval);
        }

        playerTurn = true;

        Debug.Log("플레이어의 차례!");
    }

    void Update()
    {
        if (!playerTurn)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            PlayerInput();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            PlayerInput();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            PlayerInput();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            PlayerInput();
        }
    }

    void PlayerInput()
    {
        if (playerBeat >= playerDialogue.Length)
        {
            return;
        }

        // 한 단어(대사 한 요소)를 한 번에 출력
        playerText.text += playerDialogue[playerBeat] + " ";

        playerBeat++;

        if (playerBeat >= playerDialogue.Length)
        {
            playerTurn = false;

            Debug.Log("플레이어의 대사가 모두 끝났습니다.");
        }
    }
}