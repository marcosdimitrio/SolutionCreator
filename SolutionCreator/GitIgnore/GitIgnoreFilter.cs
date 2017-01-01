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

using System;
using System.Collections.Generic;
using System.Linq;

namespace SolutionCreator.GitIgnore
{
    public class GitIgnoreFilter : IGitIgnoreFilter
    {

        private IList<GitIgnoreRule> RuleList;

        public GitIgnoreFilter()
        {
            RuleList = GetGitIgnoreList();
        }

        public bool Accepts(string filename, bool isFolder)
        {
            //foreach (var rule in RuleList)
            //{
            //    var res = rule.IsMatch(filename, isFolder);
            //    System.Diagnostics.Debug.WriteLine(rule.OriginalPattern);
            //    if (rule.OriginalPattern == @"**/packages/*")
            //    {
            //        System.Diagnostics.Debug.WriteLine(rule.OriginalPattern);
            //    }
            //    if (res)
            //    {
            //        System.Diagnostics.Debug.WriteLine(res);
            //    }
            //}

            return
                RuleList.Where(x => x.Negation == true).Any(x => x.IsMatch(filename, isFolder)) ||
                !RuleList.Where(x => x.Negation == false).Any(x => x.IsMatch(filename, isFolder));
        }

        private IList<GitIgnoreRule> GetGitIgnoreList()
        {
            var list = new List<GitIgnoreRule>();
            var ignoreFile = Properties.Resources.ignorefile;

            foreach (var item in ignoreFile.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                var txt = item.Trim();
                if (txt.Length > 0 && !txt.StartsWith("#", StringComparison.InvariantCultureIgnoreCase))
                {
                    list.Add(new GitIgnoreRule(txt));
                }
            }

            return list;
        }

    }
}
