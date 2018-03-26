using System.Collections.Generic;

public interface ICellRange
{
    List<Cell> GetCellsInRange(Cell center, int range);
    int GetDistance(Cell cell1, Cell cell2);
}
