public static class CellTypeExtensions
{
    public static bool CanMoveTo(this CellType cellType)
    {
        switch (cellType)
        {
            case CellType.Ground:
                return true;
            case CellType.Water:
                return false;
            case CellType.Void:
                return false;
        }
        throw new System.NotImplementedException();
    }
}
