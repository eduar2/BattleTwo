namespace Lib.ProblemSolving;

public static class Challenge1
{
    public static Challenge1Result FractionsCalculator(int[] numbers)
    {
        var total = numbers.Length;

        var zerosCount = numbers.Count(x => x == 0);
        var positivesCount = numbers.Count(x => x > 0);
        var negativesCount = numbers.Count(x => x < 0);

        return new Challenge1Result()
        {
            Positives = Math.Round(decimal.Divide(positivesCount, total), 6, MidpointRounding.AwayFromZero),
            Negatives = Math.Round(decimal.Divide(negativesCount, total), 6, MidpointRounding.AwayFromZero),
            Zeros = Math.Round(decimal.Divide(zerosCount, total), 6, MidpointRounding.AwayFromZero)
        };
    }
}

public class Challenge1Result
{
    public decimal Positives { get; set; }
    public decimal Negatives { get; set; }
    public decimal Zeros { get; set; }
}