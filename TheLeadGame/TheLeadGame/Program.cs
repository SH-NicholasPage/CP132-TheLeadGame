using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TheLeadGame
{
    public class Program
    {
        private const int MAX_SCORE = 10;

        public static void Main()
        {
            if(File.Exists("inputs.txt") == false)
            {
                Console.Error.WriteLine("ERROR: Unable to find input file. Show this error to the instructor ASAP.");
                return;
            }

            String[] lines = File.ReadAllLines("inputs.txt");

            int scenarios = 0;
            int correct = 0;

            int lr = 0;
            int bestDif = 0;
            int winner = 0;
            List<Int32> p1Scores = new List<Int32>();
            List<Int32> p2Scores = new List<Int32>();
            Tuple<Int32, Int32> returnedWinner;

            foreach (String line in lines)
            {
                if (String.IsNullOrWhiteSpace(line)) continue;

                if(lr == 0)
                {
                    scenarios++;

                    if(bestDif != 0)
                    {
                        returnedWinner = Source.WhoWon(p1Scores.ToArray(), p2Scores.ToArray());

                        correct += ((IsCorrect(winner, bestDif, returnedWinner) == true) ? 1 : 0);
                    }

                    lr = Convert.ToInt32(line.Trim());
                    p1Scores.Clear();
                    p2Scores.Clear();
                    bestDif = 0;
                    winner = 0;
                }
                else
                {
                    lr--;
                    p1Scores.Add(Convert.ToInt32(line.Split(" ")[0].Trim()));
                    p2Scores.Add(Convert.ToInt32(line.Split(" ")[1].Trim()));

                    if(Math.Abs(p1Scores.Last() - p2Scores.Last()) > Math.Abs(bestDif))
                    {
                        bestDif = Math.Abs(p1Scores.Last() - p2Scores.Last());
                        winner = (p1Scores.Last() - p2Scores.Last() > 0) ? 1 : 2;
                    }
                }
            }

            returnedWinner = Source.WhoWon(p1Scores.ToArray(), p2Scores.ToArray());
            correct += ((IsCorrect(winner, bestDif, returnedWinner) == true) ? 1 : 0);

            float score = (float)Math.Round(((float)correct / scenarios) * MAX_SCORE, 2);
            Console.WriteLine("***Score: " + score + "/" + MAX_SCORE);
        }

        private static bool IsCorrect(int winner, int bestDif, Tuple<Int32, Int32> returnedWinner)
        {
            if (returnedWinner.Item1 == winner && returnedWinner.Item2 == bestDif)
            {
                return true;
            }
            else
            {
                Console.WriteLine("Incorrect values returned. Expected winner to be player " + winner + " and the lead to be " + bestDif + ".");
                Console.WriteLine("\tInstead received player " + returnedWinner.Item1 + " as the winner and the lead was " + returnedWinner.Item2 + ".");
                return false;
            }
        }
    }
}