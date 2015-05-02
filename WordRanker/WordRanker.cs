using System;
using System.Collections.Generic;
using System.Linq;

namespace WordRanker
{
    public class WordRanker
    {
        #region Private Variables

        private string _originalInputWord = null;

        #endregion

        #region Constructor

        public WordRanker( string wordToRank )
        {
            _originalInputWord = wordToRank;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Ranks a given word according to the rules defined in Readme.txt.
        /// </summary>
        /// <returns>The rank for the given word.</returns>
        public float RankWord()
        {
            // The total number of entries to skip in the permutation list for the original word.
            float totalEntriesToSkip = 0;

            // Iterate of the word with this formula moving forward one character at a time.
            for ( int i = 0; i < _originalInputWord.Length; i++ )
            {
                // Get the word for the current iteration.
                string currentWord = _originalInputWord.Substring( i, _originalInputWord.Length - i );

                // Get the unique characters in the word for the current iteration and the frequncies with which they occur.
                Dictionary<char, int> currentWordCharFrequencies = GetCharFrequencies( currentWord );

                // 1) Look at the first character in the word for the current iteration.
                // 2) Find the other characters in that word that alphabetically precede the first character.
                // 3) Of the characters that alphabetically precede the first one, 
                //      take a sum of the frequencies of those characters.
                int j = 0;
                {                   
                    foreach ( char c in currentWordCharFrequencies.Keys )
                    {
                        if ( c != currentWord[ 0 ] )
                            j += currentWordCharFrequencies[ c ];
                        else
                            break;
                    }
                }

                // Get the permutation list size for the word for the current iteration.
                long currentWordPermutationListSize = GetPermutationListSize( currentWord );

                // Use the formula:
                // [currentWordPermutationListSize] * ( [ total sum of characters alphabetically preceding the first character in the word for the current iteration ] / [ the length of the word for the current iteration ]
                // Then add that to the tally of the entries in the permutation list for the original word that we want to skip.

                totalEntriesToSkip += ( currentWordPermutationListSize * ( j / ( float )currentWord.Length ) );
            }

            // The word rank will be 1 higher than the total number of entries
            // skipped for the original word's permutation list.
            return totalEntriesToSkip + 1;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// A method to calculate a factorial for a given integer.
        /// </summary>
        /// <param name="i">The integer for which to calulate the factorial.</param>
        /// <returns>The factorial calculation for the given integer.</returns>
        private long CalcFactorial( int i )
        {
            long result = 1;

            if ( i <= 1 )
                return result;
            else
            {
                for ( int j = 1; j <= i; j++ )
                {
                    result *= j;
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the size of the permutation list for a given word.
        /// </summary>
        /// <param name="inputWord"></param>
        /// <returns></returns>
        private long GetPermutationListSize( string inputWord )
        {
            // The general algorithm to calculate the permutation list size for a given word is
            // as follows, where charFrequencies[i] is the number of times each character
            // appears in the given word:

            // (inputWordLength!)/(charFrequencies[0]! * charFrequencies[1]! * charFrequencies[2]!...)

            // Get the numerator for the above formula.
            long numerator = CalcFactorial( inputWord.Length );

            // Get the denominator for the above formula.
            Dictionary<char, int> charFrequencies = GetCharFrequencies( inputWord );
            long denominator = 1;

            foreach ( int i in charFrequencies.Values )
            {
                denominator *= CalcFactorial( i );
            }

            // Divide the numerator and denominator. This is the permutation list size for the word.
            return numerator / denominator;
        }

        /// <summary>
        /// For a given string (word) returns a Dictionary containing a Key
        /// for each character in the word, and a Value indicating the number
        /// of times the character appears in the word.
        /// 
        /// Important note: Ordered by Key (character).
        /// 
        /// </summary>
        /// <param name="inputWord">The word to evaluate.</param>
        /// <returns>
        /// A Dictionary containing the characters in the word as Keys and
        /// the number of times each character appears in the word as a
        /// corresponding Value for each Key.
        /// </returns>
        private Dictionary<char, int> GetCharFrequencies( string inputWord )
        {
            // TODO: this is actually called twice per iteration.  Once directly from the loop
            // and once indirectly via GetPermutationListSize().  This could be tightened up a little...
            char[] inputWordChars = inputWord.ToCharArray();

            Dictionary<char, int> charFrequencies = new Dictionary<char, int>();

            for ( int i = 0; i < inputWordChars.Length; i++ )
            {
                if ( charFrequencies.ContainsKey( inputWordChars[ i ] ) )
                {
                    charFrequencies[ inputWordChars[ i ] ] = charFrequencies[ inputWordChars[ i ] ] + 1;
                }
                else
                {
                    charFrequencies.Add( inputWordChars[ i ], 1 );
                }
            }

            // order by character
            charFrequencies = charFrequencies.OrderBy( x => x.Key ).ToDictionary( x => x.Key, y => y.Value );

            return charFrequencies;
        }

        #endregion
    }
}
