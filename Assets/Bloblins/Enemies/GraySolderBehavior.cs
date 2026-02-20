using System;
using System.Collections.Generic;
using System.Linq;

public class GraySolderBehavior : IEnemyBehavior
{
    private readonly Random random;

    public GraySolderBehavior()
    {
        random = new Random();
    }

    public GraySolderBehavior(Random random)
    {
        this.random = random ?? new Random();
    }

    public CellPosition DecideNextMove(IEnemy enemy, GameState state)
    {
        var bloblins = GetBloblins(state);
        
        if (bloblins.Count == 0)
        {
            return enemy.Position;
        }

        var targetBloblin = ChooseRandomBloblin(bloblins);
        var desiredMove = CalculateMoveTowards(enemy.Position, targetBloblin.Position, enemy.MoveRange);
        var validMove = FindValidMove(enemy, desiredMove, state);

        return validMove;
    }

    private List<IBloblin> GetBloblins(GameState state)
    {
        var bloblins = new List<IBloblin>();
        foreach (var creature in state.Field.Creatures)
        {
            if (creature is IBloblin bloblin)
            {
                bloblins.Add(bloblin);
            }
        }
        return bloblins;
    }

    private IBloblin ChooseRandomBloblin(List<IBloblin> bloblins)
    {
        var index = random.Next(bloblins.Count);
        return bloblins[index];
    }

    private CellPosition CalculateMoveTowards(CellPosition from, CellPosition target, int moveRange)
    {
        var dx = target.X - from.X;
        var dy = target.Y - from.Y;

        var stepX = Math.Sign(dx);
        var stepY = Math.Sign(dy);

        var newX = from.X;
        var newY = from.Y;

        for (int i = 0; i < moveRange; i++)
        {
            if (Math.Abs(newX + stepX - target.X) + Math.Abs(newY - target.Y) < 
                Math.Abs(newX - target.X) + Math.Abs(newY + stepY - target.Y))
            {
                newX += stepX;
            }
            else if (stepY != 0)
            {
                newY += stepY;
            }
            else if (stepX != 0)
            {
                newX += stepX;
            }
        }

        return new CellPosition(newX, newY);
    }

    private CellPosition FindValidMove(IEnemy enemy, CellPosition desiredPosition, GameState state)
    {
        if (IsValidMove(enemy, desiredPosition, state))
        {
            return desiredPosition;
        }

        var alternatives = GetAlternativeMoves(enemy, state);
        if (alternatives.Count > 0)
        {
            return alternatives[0];
        }

        return enemy.Position;
    }

    private bool IsValidMove(IEnemy enemy, CellPosition position, GameState state)
    {
        if (!enemy.CanMoveTo(enemy.Position, position))
        {
            return false;
        }

        if (!state.Field.CellTypes.ContainsKey(position))
        {
            return false;
        }

        if (!state.Field.CellTypes[position].IsWalkable())
        {
            return false;
        }

        if (state.Field.EnvironmentObjects.ContainsKey(position))
        {
            return false;
        }

        return true;
    }

    private List<CellPosition> GetAlternativeMoves(IEnemy enemy, GameState state)
    {
        var alternatives = new List<CellPosition>();

        for (int dx = -enemy.MoveRange; dx <= enemy.MoveRange; dx++)
        {
            for (int dy = -enemy.MoveRange; dy <= enemy.MoveRange; dy++)
            {
                if (Math.Abs(dx) + Math.Abs(dy) > enemy.MoveRange)
                    continue;

                var newPosition = new CellPosition(enemy.Position.X + dx, enemy.Position.Y + dy);
                
                if (newPosition.X == enemy.Position.X && newPosition.Y == enemy.Position.Y)
                    continue;

                if (IsValidMove(enemy, newPosition, state))
                {
                    alternatives.Add(newPosition);
                }
            }
        }

        return alternatives;
    }
}
