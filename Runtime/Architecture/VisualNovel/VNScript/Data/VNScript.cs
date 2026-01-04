using System.Text;

namespace JLGA.Architecture.VisualNovel.VNScript.Data
{
    public class VNScript
    {
        private readonly string[] _lines;

        public VNScript(string script)
        {
            _lines = script.Split("\r\n");
        }

        public VNIndex? Start()
        {
            if (_lines.Length == 0)
            {
                return null;
            }
            return new VNIndex(0, 0);
        }

        public VNIndex? Next(VNIndex index)
        {
            int line = index.Line;
            int character = index.Character;

            while (true)
            {
                string currentLine = _lines[line];

                character++;
                if (character < currentLine.Length)
                {
                    return new VNIndex(line, character);
                }

                line++;
                if (line >= _lines.Length)
                {
                    return null;
                }

                character = -1;
            }
        }

        public char ToCharacter(VNIndex index)
        {
            return _lines[index.Line][index.Character];
        }

        public string ToString(VNString vnString, bool includeLastCharacter = false, bool addNewlineCharacters = false)
        {
            StringBuilder stringBuilder = new StringBuilder();

            int lastCharacterAdded = (includeLastCharacter) ? 1 : 0;

            for (int line = vnString.Start.Line; line < vnString.End.Line + 1; line++)
            {
                bool isLastLine = line == vnString.End.Line;
                int characterLimit = isLastLine ? vnString.End.Character + lastCharacterAdded : _lines[line].Length;

                for (int character = vnString.Start.Character; character < characterLimit; character++)
                {
                    stringBuilder.Append(_lines[line][character]);
                }

                if (addNewlineCharacters && !isLastLine)
                {
                    stringBuilder.Append('\n');
                }
            }

            return stringBuilder.ToString();
        }

        public string ToString(VNErrorAccumulator errors)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (errors.IsFatal)
            {
                stringBuilder.Append("[VisualNovel][Errors][Fatal]\n");
            }
            else
            {
                stringBuilder.Append("[VisualNovel][Errors][NonFatal]\n");
            }

            foreach (VNError error in errors.Errors)
            {
                _ToString(error, stringBuilder);
            }

            return stringBuilder.ToString();
        }

        private void _ToString(VNError error, StringBuilder stringBuilder)
        {
            stringBuilder.Append(error.Description);
            stringBuilder.Append("\n| ");
            stringBuilder.Append(_lines[error.Start.Line].Replace('\t', ' '));
            stringBuilder.Append("\n| ");
            stringBuilder.Append(' ', error.Start.Character);
            stringBuilder.Append('^');

            if (error.End == null)
            {
                stringBuilder.Append("---...\n");
                return;
            }

            VNIndex end = error.End.Value;
            if (error.Start.Line == end.Line)
            {
                if (error.Start.Character == end.Character)
                {
                    stringBuilder.Append('\n');
                    return;
                }
                stringBuilder.Append('-', end.Character - error.Start.Character - 1);
                stringBuilder.Append("^\n");
                return;
            }

            stringBuilder.Append("---...\n| ");
            stringBuilder.Append(_lines[end.Line]);
            stringBuilder.Append('\n');
            int numSpaces = end.Character - 3;
            stringBuilder.Append(' ', (numSpaces < 0) ? 0 : numSpaces);
            int numLines = end.Character;
            stringBuilder.Append('-', numLines);
            stringBuilder.Append("^\n");
        }
    }

}
