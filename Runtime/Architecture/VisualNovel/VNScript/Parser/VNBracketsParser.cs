using System.Collections.Generic;
using JLGA.Architecture.VisualNovel.VNScript.Data;

namespace JLGA.Architecture.VisualNovel.VNScript.Parser
{
    public class VNBracketsParser
    {
        private readonly SortedDictionary<char, VNBracketPair> _bracketPairs;
        private readonly SortedSet<char> _charactersToIgnore;
        private readonly char _endParseCharacter;

        public enum ResultType
        {
            EndParse,
            BracketPair,
            Error
        }

        public readonly struct Result
        {
            public ResultType Type { get; }
            public VNIndex? EndIndex { get; }
            public bool EndParse { get; }
            public VNBracketPair? BracketPair { get; }
            public VNString? BracketPairContents { get; }

            private Result(ResultType type, VNIndex? endIndex, bool endParse, VNBracketPair? bracketPair, VNString? bracketPairContents)
            {
                Type = type;
                EndIndex = endIndex;
                EndParse = endParse;
                BracketPair = bracketPair;
                BracketPairContents = bracketPairContents;
            }

            public static Result TypeEndParse(VNIndex? endIndex)
            {
                return new Result(ResultType.EndParse, endIndex, true, null, null);
            }

            public static Result TypeBracketPair(VNIndex? endIndex, VNBracketPair bracketPair, VNString bracketPairString)
            {
                return new Result(ResultType.BracketPair, endIndex, false, bracketPair, bracketPairString);
            }

            public static Result TypeError()
            {
                return new Result(ResultType.Error, null, true, null, null);
            }
        }

        #region VNBracketsParser Public

        public VNBracketsParser(char endParseCharacter, string charactersToIgnore)
        {
            _bracketPairs = new SortedDictionary<char, VNBracketPair>();
            _charactersToIgnore = new SortedSet<char>(charactersToIgnore);
            _endParseCharacter = endParseCharacter;
        }

        public void AddBracketPair(VNBracketPair bracketPair)
        {
            _bracketPairs.Add(bracketPair.Left, bracketPair);
        }

        /// <summary>
        /// Parses until the end parse character, or until a left bracket. If a left bracket is reached, parses until the corresponding right bracket.
        /// </summary>
        /// <param name="script">The visual novel script to parse.</param>
        /// <param name="errors">The error accumulator to add to if there are any errors.</param>
        /// <param name="start">The index to begin parsing from.</param>
        /// <returns>The final result of the parse, either being a 'Result.EndParse', 'Result.BracketPair' or 'Result.Error'.</returns>
        public Result Parse(Data.VNScript script, VNErrorAccumulator errors, VNIndex start)
        {
            // Parse until left bracket
            (VNBracketPair? brackets, VNIndex? leftBracketIndex) = _ParseUntilLeftBracket(script, errors, start);
            if (leftBracketIndex == null)
            {
                return Result.TypeError();
            }
            if (brackets == null)
            {
                return Result.TypeEndParse(script.Next(leftBracketIndex.Value));
            }

            // Parse until right bracket
            VNIndex? rightBracketIndex = _ParseUntilRightBracket(script, errors, brackets.Value, leftBracketIndex.Value);
            if (rightBracketIndex == null)
            {
                return Result.TypeError();
            }

            // Form final result
            VNString bracketsContents = new VNString(script.Next(leftBracketIndex.Value).Value, rightBracketIndex.Value);
            return Result.TypeBracketPair(script.Next(rightBracketIndex.Value), brackets.Value, bracketsContents);
        }

        #endregion

        #region VNBracketsParser Private

        private (VNBracketPair?, VNIndex?) _ParseUntilLeftBracket(Data.VNScript script, VNErrorAccumulator errors, VNIndex leftBracketIndex)
        {
            VNIndex? index = leftBracketIndex;
            VNBracketPair? bracketPair = null;
            VNIndex? leftUnexpectedCharacter = null;
            VNIndex? rightUnexpectedCharacter = null;

            while (index != null)
            {
                VNIndex currentIndex = index.Value;
                char currentCharacter = script.ToCharacter(currentIndex);

                if (currentCharacter == _endParseCharacter)
                {
                    break;
                }

                if (_charactersToIgnore.Contains(currentCharacter)) {
                    index = script.Next(currentIndex);
                    continue;
                }

                if (_bracketPairs.ContainsKey(currentCharacter))
                {
                    bracketPair = _bracketPairs[currentCharacter];
                    break;
                }

                // If reached here, parsed an unexpected character
                (leftUnexpectedCharacter, rightUnexpectedCharacter) = _UpdateUnexpectedCharacterRange(leftUnexpectedCharacter, currentIndex);
                index = script.Next(currentIndex);
            }

            _ParseUntilLeftBracketErrors(errors, leftBracketIndex, index, leftUnexpectedCharacter, rightUnexpectedCharacter);

            return (bracketPair, index);
        }

        private VNIndex? _ParseUntilRightBracket(Data.VNScript script, VNErrorAccumulator errors, VNBracketPair bracketPair, VNIndex start)
        {
            VNIndex? index = script.Next(start); // Starts at the left bracket, can be skipped
            VNBracketPair? finalBracketPair = null;
            
            while (index != null)
            {
                VNIndex currentIndex = index.Value;
                char currentCharacter = script.ToCharacter(currentIndex);

                if (currentCharacter == bracketPair.Right)
                {
                    finalBracketPair = bracketPair;
                    break;
                }

                index = script.Next(currentIndex);
            }

            _ParseUntilRightBracketErrors(errors, bracketPair, start, index);

            return index;
        }

        private (VNIndex, VNIndex) _UpdateUnexpectedCharacterRange(VNIndex? left, VNIndex newRight)
        {
            if (left == null)
            {
                return (newRight, newRight);
            }
            return (left.Value, newRight);
        }

        #endregion

        #region VNBracketsParser Errors

        private void _ParseUntilLeftBracketErrors(VNErrorAccumulator errors, VNIndex startIndex, VNIndex? finalIndex, VNIndex? leftUnexpectedCharacterIndex, VNIndex? rightUnexpectedCharacterIndex)
        {
            if (leftUnexpectedCharacterIndex != null)
            {
                _AddUnexpectedCharacterError(errors, leftUnexpectedCharacterIndex.Value, rightUnexpectedCharacterIndex.Value);
            }

            if (finalIndex == null)
            {
                _AddParseUntilLeftBracketEOFError(errors, startIndex);
            }
        }

        private void _ParseUntilRightBracketErrors(VNErrorAccumulator errors, VNBracketPair bracketPair, VNIndex startIndex, VNIndex? finalIndex)
        {
            if (finalIndex == null)
            {
                _AddParseUntilRightBracketEOFError(errors, bracketPair, startIndex);
            }
        }

        private void _AddUnexpectedCharacterError(VNErrorAccumulator errors, VNIndex left, VNIndex right)
        {
            VNError error = new VNError(
                $"[VisualNovel][Parser][Brackets] Unexpected characters ({left.Line},{left.Character})-({right.Line},{right.Character}).",
                VNError.EStatus.NonFatal,
                left,
                right
            );
            errors.Add(error);
        }

        private void _AddParseUntilLeftBracketEOFError(VNErrorAccumulator errors, VNIndex startIndex)
        {
            VNError error = new VNError(
                $"[VisualNovel][Parser][Brackets] Reached EOF parsing for left bracket ({startIndex.Line},{startIndex.Character}).",
                VNError.EStatus.Fatal,
                startIndex,
                null
            );
            errors.Add(error);
        }

        private void _AddParseUntilRightBracketEOFError(VNErrorAccumulator errors, VNBracketPair bracketPair, VNIndex startIndex)
        {
            VNError error = new VNError(
                $"[VisualNovel][Parser][Brackets] Reached EOF parsing for right bracket of pair {bracketPair.Left}{bracketPair.Right} ({startIndex.Line},{startIndex.Character}).",
                VNError.EStatus.Fatal,
                startIndex,
                null
            );
            errors.Add(error);
        }

        #endregion
    }
}
