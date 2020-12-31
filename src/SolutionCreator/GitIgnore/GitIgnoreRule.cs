// Apache License 2.0
//
// Copyright 2017 Marcos Dimitrio
//
// Licensed under the Apache License, Version 2.0 the "License";
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using SolutionCreator.Enums;
using System;
using System.Text.RegularExpressions;

namespace SolutionCreator.GitIgnore
{

    public class GitIgnoreRule
    {

        public string OriginalPattern { get; private set; }
        public string GeneratedRegex { get; private set; }
        public bool Negation { get; private set; }
        public TypeOfComparison Comparison { get; private set; }

        public GitIgnoreRule(string originalPattern)
        {
            OriginalPattern = originalPattern;

            Initialize();
        }

        public bool IsMatch(string filename, bool isDirectory)
        {

            var filenameToMatch = ReplaceBackSlashesWithForwardSlashes(filename);

            if (isDirectory)
            {
                filenameToMatch = AddTrailingSlash(filenameToMatch);
            }

            return Regex.IsMatch(filenameToMatch, GeneratedRegex);
        }

        private void Initialize()
        {
            Comparison = TypeOfComparison.File;
            Negation = false;
            GeneratedRegex = OriginalPattern;

            if (GeneratedRegex.StartsWith("!", StringComparison.InvariantCultureIgnoreCase))
            {
                GeneratedRegex = GeneratedRegex.TrimStart('!');
                Negation = true;
            }

            if (GeneratedRegex.EndsWith("/", StringComparison.InvariantCultureIgnoreCase))
            {
                GeneratedRegex = GeneratedRegex.TrimEnd('/');
                Comparison = TypeOfComparison.Directory;
            }

            GeneratedRegex = RemoveTrailingSpaces(GeneratedRegex);

            GeneratedRegex = CleanEscapedSpaces(GeneratedRegex);

            GeneratedRegex = EscapeSpecialChars(GeneratedRegex);

            GeneratedRegex = TransformQuestionMark(GeneratedRegex);

            GeneratedRegex = TransformDoubleAsterisks(GeneratedRegex);

            GeneratedRegex = TransformSingleAsterisk(GeneratedRegex);

            GeneratedRegex = AddRegexSurroundings(GeneratedRegex);
        }

        private static string ReplaceBackSlashesWithForwardSlashes(string str)
        {
            return str.Replace('\\', '/');
        }

        private string AddTrailingSlash(string str)
        {
            if (str[str.Length - 1] != '/')
            {
                return string.Concat(str, '/');
            }
            return str;
        }

        private string RemoveTrailingSpaces(string regex)
        {
            return Regex.Replace(regex, @"(?<!\\) +$", "");
        }

        private string CleanEscapedSpaces(string regex)
        {
            return regex.Replace(@"\ ", " ");
        }

        private string EscapeSpecialChars(string regex)
        {
            return Regex.Replace(regex, @"([\-\/\{\}\(\)\+\.\\\^\$\|])", @"\$1");
        }

        private string TransformQuestionMark(string regex)
        {
            return regex.Replace("?", "[^/]?");
        }

        private string TransformDoubleAsterisks(string regex)
        {
            return regex.Replace("**", ".*");
        }

        private string TransformSingleAsterisk(string regex)
        {
            return regex.Replace("*", "[^/]*");
        }

        private string AddRegexSurroundings(string regex)
        {
            regex = $"(?:^|/){regex}";

            if (Comparison == TypeOfComparison.Directory)
            {
                regex = $@"{regex}\/";
            }
            else
            {
                regex = $@"{regex}(?:$|\/)";
            }

            return regex;
        }

    }
}
