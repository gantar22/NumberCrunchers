public class Square
{
    public int fillNum;
    public int solNum;
    public int ownedBy;
    public bool spawningPwrUp;
    public int row; 
    public int col;
    public float xMinLim;
    public float xMaxLim;
    public float zMinLim;
    public float zMaxLim;

    public Square(int fillNum, int solNum, int ownedBy, bool spawningPwrUp, int row, int col, float xMinLim, float xMaxLim, float zMinLim, float zMaxLim)
    {
        this.fillNum = fillNum;
        this.solNum = solNum;
        this.ownedBy = ownedBy;
        this.spawningPwrUp = spawningPwrUp;
        this.row = row;
        this.col = col;
        this.xMinLim = xMinLim;
        this.xMaxLim = xMaxLim;
        this.zMinLim = zMinLim;
        this.zMaxLim = zMaxLim;
    }
}