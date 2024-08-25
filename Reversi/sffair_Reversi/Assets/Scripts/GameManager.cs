using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Game built with help from OttoBotCode of YouTube
 */
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private Disc discBlackUp;

    [SerializeField]
    private Disc discWhiteUp;

    [SerializeField]
    private GameObject highlightPrefab;

    [SerializeField]
    private UIManager uiManager;

    private Dictionary<Player, Disc> discPrefabs = new Dictionary<Player, Disc>();
    private GameState gameState = new GameState();
    private Disc[,] discs = new Disc[8, 8];
    private List<GameObject> highlights = new List<GameObject>();

    private int difficulty;
    private bool started = false;
    private bool isPlayingAI = false;

    private void Start()
    {
        discPrefabs[Player.Black] = discBlackUp;
        discPrefabs[Player.White] = discWhiteUp;

        AddStartDiscs();
        ShowLegalMoves();
        uiManager.ShowStartScreen();
        uiManager.SetPlayerText(gameState.CurrentPlayer);
    }
    
    private void Update()
    {
        if(started)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hitInfo))
                {
                    Vector3 impact = hitInfo.point;
                    Position boardPos = SceneToBoardPos(impact);
                    OnBoardClicked(boardPos);
                }
            }
        }
    }

    private void ShowLegalMoves()
    {
        foreach(Position boardPos in gameState.LegalMoves.Keys)
        {
            Vector3 scenePos = BoardToScenePos(boardPos) + Vector3.up * 0.01f;
            GameObject highlight = Instantiate(highlightPrefab, scenePos, Quaternion.identity);
            highlights.Add(highlight);
        }
    }

    private void HideLegalMoves()
    {
        highlights.ForEach(Destroy);
        highlights.Clear();

    }

    private void OnBoardClicked(Position boardPos)
    {
        if(gameState.MakeMove(boardPos, out MoveInfo moveInfo))
        {
            StartCoroutine(OnMoveMade(moveInfo));
        }
    }

    private IEnumerator OnMoveMade(MoveInfo moveInfo)
    {
        HideLegalMoves();
        yield return ShowMove(moveInfo);
        yield return ShowTurnOutcome(moveInfo);
        yield return new WaitForSeconds(0.5f);
        ShowLegalMoves();

        if (!gameState.GameOver && gameState.CurrentPlayer == Player.Black && isPlayingAI)
        {
            StartCoroutine(AITurn());
        }
    }

    private Position SceneToBoardPos(Vector3 scenePos)
    {
        int col = (int)(scenePos.x - 0.25f);
        int row = 7 - (int)(scenePos.z - 0.25f);
        return new Position(row, col);
    }

    private Vector3 BoardToScenePos(Position boardPos)
    {
        return new Vector3(boardPos.Col + 0.75f, 0, 7 - boardPos.Row + 0.75f);
    }

    private void SpawnDisc(Disc prefab, Position boardPos)
    {
        Vector3 scenePos = BoardToScenePos(boardPos) + Vector3.up * 0.1f;
        discs[boardPos.Row, boardPos.Col] = Instantiate(prefab, scenePos, Quaternion.identity);
    }

    private void AddStartDiscs()
    {
        foreach(Position boardPos in gameState.OccupiedPositions())
        {
            Player player = gameState.Board[boardPos.Row, boardPos.Col];
            SpawnDisc(discPrefabs[player], boardPos);
        }
    }

    private void FlipDiscs(List<Position> positions)
    {
        foreach(Position boardPos in positions)
        {
            discs[boardPos.Row, boardPos.Col].Flip();
        }
    }

    private IEnumerator ShowMove(MoveInfo moveInfo)
    {
        SpawnDisc(discPrefabs[moveInfo.Player], moveInfo.Position);
        yield return new WaitForSeconds(0.33f);
        FlipDiscs(moveInfo.OutFlanked);
        yield return new WaitForSeconds(0.83f);
    }

    private IEnumerator ShowTurnSkipepd(Player skippedPlayer)
    {
        uiManager.SetSkippedText(skippedPlayer);
        yield return uiManager.AnimateTopText();
    }

    private IEnumerator ShowGameOver(Player winner)
    {
        uiManager.SetTopText("Neither Player Can Move");
        yield return uiManager.AnimateTopText();

        yield return uiManager.ShowScoreText();
        yield return new WaitForSeconds(0.5f);

        yield return ShowCounting();

        uiManager.SetWinnerText(winner);
        yield return uiManager.ShowEndScreen();
    }

    private IEnumerator ShowTurnOutcome(MoveInfo moveInfo)
    {
        if(gameState.GameOver)
        {
            yield return ShowGameOver(gameState.Winner);
            yield break;
        }

        Player currentPlayer = gameState.CurrentPlayer;

        if(currentPlayer == moveInfo.Player)
        {
            yield return ShowTurnSkipepd(currentPlayer.Opponent());
        }

        uiManager.SetPlayerText(currentPlayer);
    }

    private IEnumerator ShowCounting()
    {
        int black = 0, white = 0;

        foreach(Position pos in gameState.OccupiedPositions())
        {
            Player player = gameState.Board[pos.Row, pos.Col];

            if(player == Player.Black)
            {
                black++;
                uiManager.SetBlackScoreText(black);
            }
            else
            {
                white++;
                uiManager.SetWhiteScoreText(white);
            }

            discs[pos.Row, pos.Col].Twitch();
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator RestartGame()
    {
        yield return uiManager.HideEndScreen();
        yield return uiManager.ShowStartScreen();
        Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.name);
    }

    public void OnPlayAgainClicked()
    {
        StartCoroutine(RestartGame());
    }

    public void OnPlusDiffButtonClicked()
    {
        uiManager.IncrementDiffuculty();
    }

    public void OnMinusDiffButtonClicked()
    {
        uiManager.DecrementDifficulty();
    }

    public void OnPvEButtonClicked()
    {
        started = true;
        difficulty = uiManager.getDifficulty();
        isPlayingAI = true;
        StartCoroutine(uiManager.HideStartScreen());
    }

    public void OnPvPButtonClicked()
    {
        started = true;
        difficulty = uiManager.getDifficulty();
        StartCoroutine(uiManager.HideStartScreen());
    }

    private IEnumerator AITurn()
    {
        yield return new WaitForSeconds(1f);

        Position bestPosition = GetBestMove(gameState, Player.Black);

        if(bestPosition != null)
        {
            gameState.MakeMove(bestPosition, out MoveInfo moveInfo);
            StartCoroutine(OnMoveMade(moveInfo));
        }
        else
        {
            yield return ShowTurnSkipepd(Player.Black);

            if (!gameState.GameOver && gameState.CurrentPlayer != Player.Black)
            {
                ShowLegalMoves();
            }
        }
    }

    private Position GetBestMove(GameState state, Player player)
    {
        Position bestPosition = null;
        int bestScore = int.MinValue;

        foreach(Position move in state.LegalMoves.Keys)
        {
            GameState newState = state;

            if(newState.CheckForMove(move))
            {
                int score = MiniMax(newState, difficulty, true, player.Opponent());

                if(score > bestScore)
                {
                    bestScore = score;
                    bestPosition = move;
                }
            }
        }

        return bestPosition;
    }

    private int MiniMax(GameState state, int depth, bool maximizingPlayer, Player player)
    {
        if (depth == 0 || state.GameOver)
        {
            return Evaluate(state, Player.Black);
        }

        if(maximizingPlayer)
        {
            int maxEval = int.MinValue;

            foreach(Position move in state.LegalMoves.Keys)
            {
                GameState newState = state;

                if(newState.CheckForMove(move))
                {
                    int eval = MiniMax(newState, depth - 1, false, player);
                    maxEval = Mathf.Max(maxEval, eval);
                }
            }

            return maxEval;
        }
        else
        {
            int minEval = int.MaxValue;

            foreach(Position move in state.LegalMoves.Keys)
            {
                GameState newState = state;

                if(newState.CheckForMove(move))
                {
                    int eval = MiniMax(newState, depth - 1, true, player);
                    minEval = Mathf.Min(minEval , eval);
                }
            }

            return minEval;
        }
    }

    private int Evaluate(GameState state, Player player)
    {
        const int cornerWeight = 100;
        const int edgeWeight = 50;
        const int pieceWeight = 1;

        int playerScore = 0;
        int opponentScore = 0;

        foreach(Position pos in state.OccupiedPositions())
        {
            Player occupent = state.Board[pos.Row, pos.Col];

            if(occupent == player)
            {
                playerScore += pieceWeight;

                if(IsCorner(pos))
                {
                    playerScore += cornerWeight;
                }
                else if(IsEdge(pos))
                {
                    playerScore += edgeWeight;
                }
            }
            else
            {
                opponentScore += pieceWeight;

                if(IsCorner(pos))
                {
                    opponentScore += cornerWeight;
                }
                else if(IsEdge(pos))
                {
                    opponentScore += edgeWeight;
                }
            }
        }

        return playerScore - opponentScore;
    }

    private bool IsCorner(Position pos)
    {
        return (pos.Row == 0 || pos.Row == 7) && (pos.Col == 0 || pos.Col == 7);
    }

    private bool IsEdge(Position pos)
    {
        return pos.Row == 0 || pos.Row == 7 || pos.Col == 0 || pos.Col == 7;
    }
}
