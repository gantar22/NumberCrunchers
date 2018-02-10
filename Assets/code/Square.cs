public class Square
{
    public int prefillNum;
    public int solNum;
    public int ownedBy;
    public bool spawningPwrUp;

    public Square(int prefillNum, int solNum, int ownedBy, bool spawningPwrUp)
    {
        this.prefillNum = prefillNum;
        this.solNum = solNum;
        this.ownedBy = ownedBy;
        this.spawningPwrUp = spawningPwrUp;
    }
}