using System.Collections.Generic;

/**
 * Game built with help from OttoBotCode of YouTube
 */
public class GameState
{
    public const int Rows = 8;
    public const int Cols = 8;

    public Player[,] Board { get; }
    public Dictionary<Player, int> DiscCount { get; }
    public Player CurrentPlayer { get; private set; }
    public bool GameOver {  get; private set; }
    public Player Winner { get; private set; }
    public Dictionary<Position, List<Position>> LegalMoves { get; private set; }

    public GameState()
    {
        Board = new Player[Rows, Cols];
        Board[3, 3] = Player.White;
        Board[4, 4] = Player.White;
        Board[3, 4] = Player.Black;
        Board[4, 3] = Player.Black;

        DiscCount = new Dictionary<Player, int>()
        {
            { Player.Black, 2 },
            { Player.White, 2 }
        };

        CurrentPlayer = Player.White;
        LegalMoves = FindLegalMoves(CurrentPlayer);
    }

    public bool MakeMove(Position pos, out MoveInfo moveInfo)
    {
        if(!LegalMoves.ContainsKey(pos))
        {
            moveInfo = null;
            return false;
        }

        Player movePlayer = CurrentPlayer;
        List<Position> outFlanked = LegalMoves[pos];

        Board[pos.Row, pos.Col] = movePlayer;
        FlipDiscs(outFlanked);
        UpdateDiscCounts(movePlayer, outFlanked.Count);
        PassTurn();

        moveInfo = new MoveInfo { Player = movePlayer, Position = pos, OutFlanked = outFlanked };
        return true;
    }

    public bool CheckForMove(Position pos)
    {
        if(!LegalMoves.ContainsKey(pos))
        {
            return false;
        }

        return true;
    }

    public IEnumerable<Position> OccupiedPositions()
    {
        for(int row = 0; row < Rows; row++)
        {
            for(int col = 0; col < Cols; col++)
            {
                if (Board[row, col] != Player.None)
                {
                    yield return new Position(row, col);
                }
            }
        }
    }

    private void FlipDiscs(List<Position> positions)
    {
        foreach(Position pos in positions)
        {
            Board[pos.Row, pos.Col] = Board[pos.Row, pos.Col].Opponent();
        }
    }

    private void UpdateDiscCounts(Player movePlayer, int outFlankedCount)
    {
        DiscCount[movePlayer] += outFlankedCount + 1;
        DiscCount[movePlayer.Opponent()] -= 1;
    }

    private void ChangePlayer()
    {
        CurrentPlayer = CurrentPlayer.Opponent();
        LegalMoves = FindLegalMoves(CurrentPlayer);
    }

    private Player FindWinner()
    {
        if (DiscCount[Player.Black] > DiscCount[Player.White])
        {
            return Player.Black;
        }
        if (DiscCount[Player.White] > DiscCount[Player.Black])
        {
            return Player.White;
        }

        return Player.None;
    }

    private void PassTurn()
    {
        ChangePlayer();

        if(LegalMoves.Count > 0)
        {
            return;
        }

        ChangePlayer();

        if(LegalMoves.Count == 0)
        {
            CurrentPlayer = Player.None;
            GameOver = true;
            Winner = FindWinner();
        }
    }

    private bool IsInsideBoard(int row, int col)
    {
        return row >= 0 && row < Rows && col >= 0 && col < Cols;
    }

    private List<Position> OutFlankedInDir(Position pos, Player player, int rDelta, int cDelta)
    {
        List<Position> outFlanked = new List<Position>();
        int row = pos.Row + rDelta;
        int col = pos.Col + cDelta;

        while(IsInsideBoard(row, col) && Board[row, col] != Player.None) 
        {
            if (Board[row, col] == player.Opponent())
            {
                outFlanked.Add(new Position(row, col));
                row += rDelta;
                col += cDelta;
            }
            else
            {
                return outFlanked;
            }
        }

        return new List<Position>();
    }

    private List<Position> OutFlanked(Position pos, Player player)
    {
        List<Position> outFlanked = new List<Position>();

        for(int rDelta = -1; rDelta <= 1; rDelta++) 
        {
            for(int cDelta = -1; cDelta <= 1; cDelta++)
            {
                if(rDelta == 0 && cDelta == 0)
                {
                    continue;
                }

                outFlanked.AddRange(OutFlankedInDir(pos, player, rDelta, cDelta));
            }
        }

        return outFlanked;
    }

    private bool IsMoveLegal(Player player, Position pos, out List<Position> outFlanked)
    {
        if (Board[pos.Row, pos.Col] != Player.None)
        {
            outFlanked = null;
            return false;
        }

        outFlanked = OutFlanked(pos, player);
        return outFlanked.Count > 0;
    }

    private Dictionary<Position, List<Position>> FindLegalMoves(Player player)
    {
        Dictionary<Position, List<Position>> legalMoves = new Dictionary<Position, List<Position>>();

        for(int row = 0; row < Rows; row++)
        {
            for(int col = 0; col < Cols; col++)
            {
                Position pos = new Position(row, col);

                if(IsMoveLegal(player, pos, out List<Position> outFlanked))
                {
                    legalMoves[pos] = outFlanked;
                }
            }
        }

        return legalMoves;
    }
}
