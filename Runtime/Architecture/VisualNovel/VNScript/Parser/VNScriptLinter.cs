using JLGA.Architecture.VisualNovel.VNScript.Data;
using System.Collections.Generic;

namespace JLGA.Architecture.VisualNovel.VNScript.Parser
{
    public class VNLinter<C,R>
    {
        private VNBracketsParser _bracketsParser;
        private VNActionParser<C,R> _actionParser;
        private VNBracketPair _actionBracketPair;

        public VNLinter(VNBracketsParser bracketsParser, VNActionParser<C,R> actionParser, VNBracketPair actionBracketPair)
        {
            _bracketsParser = bracketsParser;
            _actionParser = actionParser;
            _actionBracketPair = actionBracketPair;
        }

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
                    List<VNActionParser<C,R>.Result> actionParserResults = _actionParser.Parse(script, errors, bracketsParserResult.BracketPairContents.Value);
                    if (errors.IsFatal)
                    {
                        break;
                    }
                }
            }
        }
    }
}
