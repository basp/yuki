namespace Yuki
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public static class StatementSplitter
    {
        private const RegexOptions RegexOpts =
            RegexOptions.IgnoreCase | RegexOptions.Multiline;

        private const string BatchTerminatorReplacementString = @" |{[_REMOVE_]}| ";
        private const string RegexSplitString = @"\|\{\[_REMOVE_\]\}\|";

        private const string StringPattern = @"(?<KEEP1>'[^']*')";
        private const string DashCommentPattern = @"(?<KEEP1>--.*$)";
        private const string StarCommentPattern = @"(?<KEEP1>/\*[\S\s]*?\*/)";
        private const string SeparatorPattern = @"(?<KEEP1>\s)(?<BATCHSPLITTER>GO)(?<KEEP2>\s|$)";
        private const string BatchSplitPattern =
            StringPattern + "|" + DashCommentPattern + "|" + StarCommentPattern + "|" + SeparatorPattern;

        public static IEnumerable<string> Split(string sql)
        {
            var regex = new Regex(BatchSplitPattern, RegexOpts);
            var scrubbed = regex.Replace(sql, EvalAndReplaceSplitItems);
            return Regex.Split(scrubbed, RegexSplitString, RegexOpts)
                .Where(ScriptHasTextToRun)
                .Select(x => x.Trim());
        }

        private static string EvalAndReplaceSplitItems(Match match)
        {
            var keep1 = match.Groups["KEEP1"].Value;
            var keep2 = match.Groups["KEEP2"].Value;
            if (match.Groups["BATCHSPLITTER"].Success)
            {
                return keep1 + BatchTerminatorReplacementString + keep2;
            }

            return keep1 + keep2;
        }

        private static bool ScriptHasTextToRun(string sql)
        {
            sql = Regex.Replace(sql, RegexSplitString, string.Empty, RegexOpts);
            return !string.IsNullOrWhiteSpace(sql);
        }
    }
}
