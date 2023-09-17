namespace Lib.ProblemSolving;

public static class Challenge3
{
    public static int FindLessCostPath(int[][] board)
    {
        int i = 0;
        int j = 0;
        var rows = board.GetLength(0);
        var columns = board[0].GetLength(0);
        bool[,] visited = new bool[rows, columns];
        int[,] distances = new int[rows, columns];

        List<(int Row, int Col, int Distance)> partialDistances = new List<(int Row, int Col, int Distance)>();
        partialDistances.Add((Row: 0, Col: 0, Distance: board[0][0]));
        while (partialDistances.Count > 0)
        {
            var current = partialDistances[0];
            partialDistances.RemoveAt(0);

            i = current.Row;
            j = current.Col;

            distances[i, j] = current.Distance;
            int right = int.MaxValue;
            int down = int.MaxValue;
            int up = int.MaxValue;
            int left = int.MaxValue;


            visited[i, j] = true;

            if (IsAbleToGoRight(j, board[i].GetLength(0)) && visited[i, j + 1] == false)
            {
                right = board[i][j + 1];
                partialDistances.Add((Row: i, Col: j + 1, Distance: current.Distance + right));
                visited[i, j + 1] = true;
            }

            if (IsAbleToGoDown(i, board.GetLength(0)) && visited[i + 1, j] == false)
            {
                down = board[i + 1][j];
                partialDistances.Add((Row: i + 1, Col: j, Distance: current.Distance + down));
                visited[i + 1, j] = true;
            }

            if (IsAbleToGoUp(i, board.GetLength(0)) && visited[i - 1, j] == false)
            {
                up = board[i - 1][j];
                partialDistances.Add((Row: i - 1, Col: j, Distance: current.Distance + up));
                visited[i - 1, j] = true;
            }

            if (IsAbleToGoLeft(j, board[i].GetLength(0)) && visited[i, j - 1] == false)
            {
                left = board[i][j - 1];
                partialDistances.Add((Row: i, Col: j - 1, Distance: current.Distance + left));
                visited[i, j - 1] = true;
            }

            partialDistances = partialDistances.OrderBy(x => x.Distance).ToList();
        }
        return distances[rows - 1, columns - 1] - board[rows - 1][columns - 1];
    }

    private static bool IsAbleToGoLeft(int j, int length)
    {
        return j - 1 >= 0;
    }

    private static bool IsAbleToGoRight(int j, int length)
    {
        return j + 1 < length;
    }

    private static bool IsAbleToGoUp(int i, int length)
    {
        return i - 1 >= 0;
    }

    private static bool IsAbleToGoDown(int i, int length)
    {
        return i + 1 < length;
    }

}