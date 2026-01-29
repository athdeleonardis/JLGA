using System.Text;

namespace JLGA.Architecture.VisualNovel.Data
{
    /// <summary>
    /// Visual Novel Script. Represents a visual novel script, separated line-by-line, indexable via 'VNIndex'.
    /// </summary>
    public class VNScript
    {
        private readonly string[] _lines;

        /// <summary>
        /// Create a VNScript from a string, splitting it line-by-line.
        /// </summary>
        /// <param name="script">The script text to be split.</param>
        public VNScript(string script)
        {
            _lines = script.Split("\r\n");
        }

        /// <summary>
        /// Create a nullable VNIndex to begin parsing at. If the text is empty.
        /// </summary>
        /// <returns>The index to begin parsing from. If the text is empty, returns null.</returns>
        public VNIndex? Start()
        {
            if (_lines.Length == 0)
            {
                return null;
            }
            return new VNIndex(0, 0);
        }

        /// <summary>
        /// Get the next VNIndex from a VNIndex. Returns null if at the end of the script.
        /// </summary>
        /// <param name="index">The VNIndex from which to get the next index in the VNScript from.</param>
        /// <returns>The next VNIndex after the inputted VNIndex. If the next index is the EOF, returns null.</returns>
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

        /// <summary>
        /// Gets the character of the script at the inputted VNindex.
        /// </summary>
        /// <param name="index">The index to get the script's character at.</param>
        /// <returns>The character at the inputted VNIndex.</returns>
        public char ToCharacter(VNIndex index)
        {
            return _lines[index.Line][index.Character];
        }

        /// <summary>
        /// Gets the portion of text enclosed by the inputted VNString.
        /// </summary>
        /// <param name="vnString">The VNString encompassing the portion of the VNScript.</param>
        /// <param name="includeLastCharacter">Whether to include the character indexed by the inputted VNString's end.</param>
        /// <param name="addNewlineCharacters">Whether to add newline characters at every newline of the VNScript.</param>
        /// <returns>The portion of text enclosed by the inputted VNString.</returns>
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

        /// <summary>
        /// Converts a VNErrorAccumulator to a human-readable error message. Every error has it's description and references to the VNScript where the error occurs.
        /// </summary>
        /// <param name="errors">The list of errors to create an error message from.</param>
        /// <returns>The error message representing the error accumulator.</returns>
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
            stringBuilder.Append('\n');
            if (error.Start == null)
            {
                return;
            }
            VNIndex start = error.Start.Value;
            stringBuilder.Append("| ");
            stringBuilder.Append(_lines[start.Line].Replace('\t', ' '));
            stringBuilder.Append("\n| ");
            stringBuilder.Append(' ', start.Character);
            stringBuilder.Append('^');

            if (error.End == null)
            {
                stringBuilder.Append("---...\n");
                return;
            }

            VNIndex end = error.End.Value;
            if (start.Line == end.Line)
            {
                if (start.Character == end.Character)
                {
                    stringBuilder.Append('\n');
                    return;
                }
                stringBuilder.Append('-', end.Character - start.Character - 1);
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
