using System;
using System.Diagnostics;

namespace WordRanker
{
    public class Program
    {
        static void Main( string[] args )
        {
            // TODO: Validate input.
            string inputWord = Console.ReadLine();

            Stopwatch stopWatch = Stopwatch.StartNew();

            WordRanker ranker = new WordRanker( inputWord );
            float rank = ranker.RankWord();

            stopWatch.Stop();
            long elapsedMs = stopWatch.ElapsedMilliseconds;

            Console.WriteLine( "The rank for word {0} is {1}", inputWord, Convert.ToInt64( rank ) );
            Console.WriteLine( "Elapsed time to calculate rank in milliseconds: {0}", elapsedMs );

            // Leave console open until Escape is pressed.
            while ( Console.ReadKey().Key != ConsoleKey.Escape ) { }
        }
    }
}
