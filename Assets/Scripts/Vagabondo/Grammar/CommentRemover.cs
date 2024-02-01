using System.Text;

namespace Vagabondo.Grammar
{
    internal class CommentRemover
    {
        public static string RemoveComments(string inputText)
        {
            var result = new StringBuilder();
            var lines = inputText.Split("\n");
            foreach (var line in lines)
            {
                var startCommentIndex = line.IndexOf("//");
                if (startCommentIndex == -1)
                    result.Append(line);
                else
                    result.Append(line.Substring(0, startCommentIndex));
            }

            return result.ToString();
        }
    }
}