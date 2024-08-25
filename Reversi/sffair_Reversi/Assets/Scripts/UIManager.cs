using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/**
 * Game built with help from OttoBotCode of YouTube
 */
public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI topText;

    [SerializeField]
    private TextMeshProUGUI blackScoreText;

    [SerializeField]
    private TextMeshProUGUI whiteScoreText;

    [SerializeField]
    private TextMeshProUGUI winnerText;

    [SerializeField]
    private Image blackOverlay;

    [SerializeField]
    private RectTransform playAgainButton;

    [SerializeField]
    private TextMeshProUGUI title;

    [SerializeField]
    private TextMeshProUGUI selectDifficulty;

    [SerializeField]
    private TextMeshProUGUI difficulty;

    [SerializeField]
    private TextMeshProUGUI OR;

    [SerializeField]
    private RectTransform pveButton;

    [SerializeField]
    private RectTransform pvpButton;

    [SerializeField]
    private RectTransform plusDiffButton;

    [SerializeField]
    private RectTransform minusDiffButton;

    public void SetPlayerText(Player currentPlayer)
    {
        if(currentPlayer == Player.Black)
        {
            topText.text = "Black's Turn <sprite name=DiscBlackUp>";
        }
        else if(currentPlayer == Player.White)
        {
            topText.text = "White's Turn <sprite name=DiscWhiteUp>";
        }
    }

    public void SetSkippedText(Player skippedPlayer)
    {
        if(skippedPlayer == Player.Black)
        {
            topText.text = "Black Cannot Move! <sprite name=DiscBlackUp>";
        }
        else if(skippedPlayer == Player.White)
        {
            topText.text = "White Cannot Move! <sprite name=DiscWhiteUp>";
        }
    }

    public void SetTopText(string message)
    {
        topText.text = message;
    }

    public IEnumerator AnimateTopText()
    {
        topText.transform.LeanScale(Vector3.one * 1.2f, 0.25f).setLoopPingPong(4);
        yield return new WaitForSeconds(2);
    }

    private IEnumerator ScaleDown(RectTransform rect)
    {
        rect.LeanScale(Vector3.zero, 0.2f);
        yield return new WaitForSeconds(0.2f);
        rect.gameObject.SetActive(false);
    }

    private IEnumerator ScaleUp(RectTransform rect)
    {
        rect.gameObject.SetActive(true);
        rect.localScale = Vector3.zero;
        rect.LeanScale(Vector3.one, 0.2f);
        yield return new WaitForSeconds(0.2f);
    }

    public IEnumerator ShowScoreText()
    {
        yield return ScaleDown(topText.rectTransform);
        yield return ScaleUp(blackScoreText.rectTransform);
        yield return ScaleUp(whiteScoreText.rectTransform);
    }

    public void SetBlackScoreText(int score)
    {
        blackScoreText.text = $"<sprite name=DiscBlackUp> {score}";
    }    

    public void SetWhiteScoreText(int score)
    {
        whiteScoreText.text = $"<sprite name=DiscWhiteUp> {score}";
    }

    private IEnumerator ShowOverlay()
    {
        blackOverlay.gameObject.SetActive(true);
        blackOverlay.color = Color.clear;
        blackOverlay.rectTransform.LeanAlpha(0.8f, 1);
        yield return new WaitForSeconds(1);
    }

    private IEnumerator HideOverlay()
    {
        blackOverlay.rectTransform.LeanAlpha(0, 1);
        yield return new WaitForSeconds(1);
        blackOverlay.gameObject.SetActive(false);
    }

    private IEnumerator MoveScoresDown()
    {
        blackScoreText.rectTransform.LeanMoveY(0, 0.5f);
        whiteScoreText.rectTransform.LeanMoveY(0, 0.5f);
        yield return new WaitForSeconds(0.5f);
    }

    public void SetWinnerText(Player winner)
    {
        switch (winner)
        {
            case Player.Black:
                winnerText.text = "Black Wins!";
                break;
            case Player.White:
                winnerText.text = "White Wins!";
                break;
            case Player.None:
                winnerText.text = "It's a Tie!";
                break;
        }
    }

    public int getDifficulty()
    {
        return int.Parse(difficulty.text);
    }

    public void IncrementDiffuculty()
    {
        int curDiff = int.Parse(difficulty.text);
        curDiff++;

        if(curDiff > 7)
        {
            curDiff = 1;
        }

        difficulty.SetText(curDiff.ToString());
    }

    public void DecrementDifficulty()
    {
        int curDiff = int.Parse(difficulty.text);
        curDiff--;

        if(curDiff < 1)
        {
            curDiff = 7;
        }

        difficulty.SetText(curDiff.ToString());
    }

    public IEnumerator ShowEndScreen()
    {
        yield return ShowOverlay();
        yield return MoveScoresDown();
        yield return ScaleUp(winnerText.rectTransform);
        yield return ScaleUp(playAgainButton);
    }

    public IEnumerator HideEndScreen()
    {
        StartCoroutine(ScaleDown(winnerText.rectTransform));
        StartCoroutine(ScaleDown(blackScoreText.rectTransform));
        StartCoroutine(ScaleDown(whiteScoreText.rectTransform));
        StartCoroutine(ScaleDown(playAgainButton));

        yield return new WaitForSeconds(0.5f);
        yield return HideOverlay();
    }

    public IEnumerator ShowStartScreen()
    {
        yield return ShowOverlay();
        yield return ScaleUp(title.rectTransform);
        yield return ScaleUp(selectDifficulty.rectTransform);
        yield return ScaleUp(difficulty.rectTransform);
        yield return ScaleUp(minusDiffButton);
        yield return ScaleUp(plusDiffButton);
        yield return ScaleUp(pveButton);
        yield return ScaleUp(OR.rectTransform);
        yield return ScaleUp(pvpButton);
    }

    public IEnumerator HideStartScreen()
    {
        StartCoroutine(ScaleDown(title.rectTransform));
        StartCoroutine(ScaleDown(selectDifficulty.rectTransform));
        StartCoroutine(ScaleDown(difficulty.rectTransform));
        StartCoroutine(ScaleDown(minusDiffButton));
        StartCoroutine(ScaleDown(plusDiffButton));
        StartCoroutine(ScaleDown(pveButton));
        StartCoroutine(ScaleDown(OR.rectTransform));
        StartCoroutine(ScaleDown(pvpButton));

        yield return new WaitForSeconds(0.5f);
        yield return HideOverlay();
    }
}
