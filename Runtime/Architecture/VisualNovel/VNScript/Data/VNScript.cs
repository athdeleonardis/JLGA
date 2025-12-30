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

        public string ToString(VNString vnString)
        {
            StringBuilder stringBuilder = new StringBuilder();

            for (int line = vnString.Start.Line; line < vnString.End.Line + 1; line++)
            {
                bool isLastLine = line == vnString.End.Line;
                int characterLimit = isLastLine ? vnString.End.Character + 1: _lines[line].Length;

                for (int character = vnString.Start.Character; character < characterLimit; character++)
                {
                    stringBuilder.Append(_lines[line][character]);
                }

                if (!isLastLine)
                {
                    stringBuilder.Append('\n');
                }
            }

            return stringBuilder.ToString();
        }
    }

}
