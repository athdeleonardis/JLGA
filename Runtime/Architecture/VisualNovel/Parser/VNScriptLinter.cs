using JLGA.Architecture.VisualNovel.Data;
using System.Collections.Generic;

namespace JLGA.Architecture.VisualNovel.Parser
{
    /// <summary>
    /// Visual Novel Linter. Represents a linter for VNScript, checking the script against a VNBracketsParser and a VNActionParser.
    /// </summary>
    /// <typeparam name="C">The context type for the action parser.</typeparam>
    /// <typeparam name="R">The return type for the action parser.</typeparam>
    public class VNLinter<C, R>
    {
        private VNBracketsParser _bracketsParser;
        private VNActionParser<C, R> _actionParser;
        private VNBracketPair _actionBracketPair;

        /// <summary>
        /// Create a linter that parses VNScripts with a VNBracketsParser, and for a particular bracket pair type, parses between the bracket pairs with a VNActionParser.
        /// </summary>
        /// <param name="bracketsParser">The brackets parser used to parse brackets and parse-ending characters.</param>
        /// <param name="actionParser">The action parser used to parse actions between the action bracket pairs.</param>
        /// <param name="actionBracketPair">The bracket pair type to parse actions contained within.</param>
        public VNLinter(VNBracketsParser bracketsParser, VNActionParser<C, R> actionParser, VNBracketPair actionBracketPair)
        {
            _bracketsParser = bracketsParser;
            _actionParser = actionParser;
            _actionBracketPair = actionBracketPair;
        }

        /// <summary>
        /// Parse an entire visual novel script, placing any errors that occur in an error accumulator.
        /// </summary>
        /// <param name="script">The visual novel script to parse.</param>
        /// <param name="errors">The error accumulator to place errors in.</param>
        public void Lint(Data.VNScript script, VNErrorAccumulator errors)
        {
            VNIndex? index = script.Start();
            while (index is VNIndex currentIndex)
            {
                VNBracketsParser.Result bracketsParserResult = _bracketsParser.Parse(script, errors, currentIndex);
                index = bracketsParserResult.EndIndex;

                if (errors.IsFatal)
                {
                    break;
                }

                if (bracketsParserResult.BracketPair is VNBracketPair bracketPair && bracketPair == _actionBracketPair)
                {
                    List<VNActionParser<C, R>.Result> actionParserResults = _actionParser.Parse(script, errors, bracketsParserResult.BracketPairContents.Value);
                    if (errors.IsFatal)
                    {
                        break;
                    }
                }
            }
        }
    }
}
