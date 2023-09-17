namespace Lib.ProblemSolving;

public static class Challenge2
{
    public static int DiceFacesCalculator(int dice1, int dice2, int dice3)
    {
        if (dice1 >= 1 && dice1 <= 6 && dice2 >= 1 && dice2 <= 6 && dice3 >= 1 && dice3 <= 6)
        {
            if (dice1 == dice2 && dice1 == dice3) return dice1 * 3;
            else if (dice1 == dice2 || dice2 == dice3 || dice1 == dice3) return dice1 * 2;
            else if (dice1 > dice2 && dice1 > dice3) return dice1;
            else if (dice2 > dice1 && dice2 > dice3) return dice2;
            else return dice3;
        }
        throw new Exception("Dice out of number range");
    }
}