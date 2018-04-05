using System.Collections.Generic;

public interface ICellRange
{
    List<Cell> GetCellsInRange(Cell center, int range, bool includeEnemies);
    List<Cell> GetCellsInLineRange(Cell center);
    int GetDistance(Cell cell1, Cell cell2);
}
