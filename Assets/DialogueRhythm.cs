using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueRhythm : MonoBehaviour
{
    [Header("대사를 표시할 텍스트")]
    public TMP_Text customerText;
    public TMP_Text playerText;

    [Header("Customer 노트 Image (D, F, J, K)")]
    public Image[] customerNotes = new Image[4];

    [Header("Player 노트 Image (D, F, J, K)")]
    public Image[] playerNotes = new Image[4];

    [Header("노트 색상 설정")]
    public Color normalColor = Color.white;
    public Color activeColor = Color.yellow;
    public Color hitColor = Color.green;
    public Color missColor = Color.red;

    [Header("손님 대사 박자 간격")]
    public float beatInterval = 0.75f;

    [Header("플레이어 노트 판정 시간")]
    public float noteTimeLimit = 0.8f;

    [Header("MISS 표시 시간")]
    public float missDisplayTime = 0.5f;


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

    string[] playerMissDialogue =
    {
    "반갑ㅅㅂ니다.",
    "오눌",
    "어던 하류였고,",
    "뮤슨 일이 잇엇나요?"
    };

    int currentBeat = 0;
    int playerBeat = 0;

    bool playerTurn = false;

    // 현재 플레이어 노트가 나온 후 지난 시간
    float noteTimer = 0f;

    // MISS 처리 중인지 확인
    bool processingMiss = false;


    void Start()
    {
        customerText.text = "";
        playerText.text = "";

        ResetNoteColors();

        StartCoroutine(ShowCustomerDialogue());
    }


    // =========================================================
    // Customer 대사
    // =========================================================

    IEnumerator ShowCustomerDialogue()
    {
        while (currentBeat < customerDialogue.Length)
        {
            // 현재 Customer 노트를 노란색으로 표시
            SetNoteColor(
                customerNotes,
                currentBeat,
                activeColor
            );

            // Customer 대사 출력
            customerText.text +=
                customerDialogue[currentBeat] + " ";

            // 노트를 초록색으로 잠깐 표시
            StartCoroutine(
                FlashNoteColor(
                    customerNotes,
                    currentBeat,
                    hitColor,
                    0.5f
                )
            );

            currentBeat++;

            // 다음 박자까지 기다림
            yield return new WaitForSeconds(beatInterval);
        }

        // Customer 대사가 끝나면 Player 턴
        playerTurn = true;

        // 첫 번째 노트(D)를 노란색으로 표시
        HighlightCurrentPlayerNote();

        // 타이머 초기화
        noteTimer = 0f;

        Debug.Log("플레이어의 차례!");
    }


    // =========================================================
    // Player 입력
    // =========================================================

    void Update()
    {
        // Player 턴이 아니거나 MISS 처리 중이면 입력하지 않음
        if (!playerTurn || processingMiss)
        {
            return;
        }

        // 현재 노트의 시간 증가
        noteTimer += Time.deltaTime;


        // 제한 시간이 지나면 MISS
        if (noteTimer >= noteTimeLimit)
        {
            MissNote();
            return;
        }


        // -----------------------------------------
        // D
        // -----------------------------------------

        if (playerBeat == 0 &&
            Input.GetKeyDown(KeyCode.D))
        {
            PlayerInput();
        }


        // -----------------------------------------
        // F
        // -----------------------------------------

        else if (playerBeat == 1 &&
                 Input.GetKeyDown(KeyCode.F))
        {
            PlayerInput();
        }


        // -----------------------------------------
        // J
        // -----------------------------------------

        else if (playerBeat == 2 &&
                 Input.GetKeyDown(KeyCode.J))
        {
            PlayerInput();
        }


        // -----------------------------------------
        // K
        // -----------------------------------------

        else if (playerBeat == 3 &&
                 Input.GetKeyDown(KeyCode.K))
        {
            PlayerInput();
        }
    }


    // =========================================================
    // Player가 올바른 키를 눌렀을 때
    // =========================================================

    void PlayerInput()
    {
        if (playerBeat >= playerDialogue.Length)
        {
            return;
        }


        // 현재 노트를 초록색으로 표시
        StartCoroutine(
            FlashNoteColor(
                playerNotes,
                playerBeat,
                hitColor,
                0.5f
            )
        );


        // 해당 Player 대사를 출력
        playerText.text +=
            playerDialogue[playerBeat] + " ";


        // 다음 노트로 이동
        playerBeat++;


        // 다음 노트의 타이머 초기화
        noteTimer = 0f;


        // 아직 남은 노트가 있다면
        if (playerBeat < playerDialogue.Length)
        {
            // 다음 노트를 노란색으로 표시
            HighlightCurrentPlayerNote();
        }


        // 모든 노트를 끝냈다면
        else
        {
            playerTurn = false;

            Debug.Log("플레이어의 대사가 모두 끝났습니다.");
        }
    }


    // =========================================================
    // Player가 노트를 놓쳤을 때
    // =========================================================

    void MissNote()
    {
        // MISS 처리 시작
        processingMiss = true;

        StartCoroutine(ProcessMiss());
    }


    IEnumerator ProcessMiss()
    {
        Debug.Log("MISS!");

        // 현재 노트를 빨간색으로 변경
        if (playerBeat < playerNotes.Length)
        {
            playerNotes[playerBeat].color = missColor;
        }

        // MISS된 경우 오타가 난 대사를 출력
        if (playerBeat < playerMissDialogue.Length)
        {
            playerText.text +=
                playerMissDialogue[playerBeat] + " ";
        }

        // 빨간색을 0.5초 동안 보여줌
        yield return new WaitForSeconds(missDisplayTime);

        // 현재 노트를 다시 흰색으로 변경
        if (playerBeat < playerNotes.Length)
        {
            playerNotes[playerBeat].color = normalColor;
        }

        // 다음 노트로 이동
        playerBeat++;

        // 다음 노트의 시간 측정 시작
        noteTimer = 0f;

        // 다음 노트가 남아있다면
        if (playerBeat < playerDialogue.Length)
        {
            HighlightCurrentPlayerNote();
        }
        // 모든 노트를 끝냈다면
        else
        {
            playerTurn = false;

            Debug.Log("플레이어의 대사가 모두 끝났습니다.");
        }

        // MISS 처리 종료
        processingMiss = false;
    }


    // =========================================================
    // 현재 Player 노트 노란색 표시
    // =========================================================

    void HighlightCurrentPlayerNote()
    {
        if (playerBeat < playerNotes.Length)
        {
            SetNoteColor(
                playerNotes,
                playerBeat,
                activeColor
            );
        }
    }


    // =========================================================
    // 노트 색상 잠깐 변경
    // =========================================================

    IEnumerator FlashNoteColor(
        Image[] notes,
        int index,
        Color targetColor,
        float duration)
    {
        if (index >= 0 &&
            index < notes.Length)
        {
            if (notes[index] == null)
            {
                yield break;
            }


            // 지정한 색상으로 변경
            notes[index].color = targetColor;


            // 지정된 시간 동안 유지
            yield return new WaitForSeconds(duration);


            // 다시 기본 색상으로 변경
            notes[index].color = normalColor;
        }
    }


    // =========================================================
    // 노트 색상 설정
    // =========================================================

    void SetNoteColor(
        Image[] notes,
        int index,
        Color color)
    {
        if (index >= 0 &&
            index < notes.Length)
        {
            if (notes[index] != null)
            {
                notes[index].color = color;
            }
        }
    }


    // =========================================================
    // 모든 노트를 기본 색상으로 초기화
    // =========================================================

    void ResetNoteColors()
    {
        for (int i = 0; i < 4; i++)
        {
            if (customerNotes[i] != null)
            {
                customerNotes[i].color = normalColor;
            }

            if (playerNotes[i] != null)
            {
                playerNotes[i].color = normalColor;
            }
        }
    }
}
