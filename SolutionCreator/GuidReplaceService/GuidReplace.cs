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

using SolutionCreator.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using SolutionCreator.Helpers;

namespace SolutionCreator.GuidReplaceService
{
    public class GuidReplace : IGuidReplace
    {

        public void Replace(string newSolutionDir, SolutionName solutionName)
        {
            var listOfProjectGuids = SearchForGuidInProjectFiles(newSolutionDir);

            listOfProjectGuids = GenerateTheNewGuids(listOfProjectGuids);

            ReplaceInFiles(newSolutionDir, listOfProjectGuids, solutionName);
        }

        private IList<ProjectGuid> SearchForGuidInProjectFiles(string newSolutionDir)
        {
            var allfiles = Directory.GetFiles(newSolutionDir, "*.csproj", SearchOption.AllDirectories);
            var returnList = new List<ProjectGuid>();

            foreach (var file in allfiles)
            {
                var doc = XDocument.Load(file);
                XNamespace msbuild = "http://schemas.microsoft.com/developer/msbuild/2003";

                foreach (var resource in doc.Descendants(msbuild + "ProjectGuid"))
                {
                    var guid = resource.Value.Trim('{', '}');

                    returnList.Add(new ProjectGuid() { OldGuid = guid });
                }
            }

            return returnList;
        }

        private IList<ProjectGuid> GenerateTheNewGuids(IList<ProjectGuid> listOfProjectGuids)
        {
            foreach (var item in listOfProjectGuids)
            {
                item.NewGuid = Guid.NewGuid().ToString().ToUpperInvariant();
            }

            return listOfProjectGuids;
        }

        private void ReplaceInFiles(string newSolutionDir, IList<ProjectGuid> listOfProjectGuids, SolutionName solutionName)
        {
            var allFiles = new List<string>();

            allFiles.AddRange(Directory.GetFiles(newSolutionDir, "*.sln", SearchOption.AllDirectories));
            allFiles.AddRange(Directory.GetFiles(newSolutionDir, "*.csproj", SearchOption.AllDirectories));
            allFiles.AddRange(Directory.GetFiles(newSolutionDir, "*.cs", SearchOption.AllDirectories));
            allFiles.AddRange(Directory.GetFiles(newSolutionDir, "*.cshtml", SearchOption.AllDirectories));
            allFiles.AddRange(Directory.GetFiles(newSolutionDir, "*.asax", SearchOption.AllDirectories));
            allFiles.AddRange(Directory.GetFiles(newSolutionDir, "*.config", SearchOption.AllDirectories));

            foreach (var file in allFiles)
            {
                var text = File.ReadAllText(file);

                var newText = text;

                foreach (var projectGuid in listOfProjectGuids)
                {
                    newText = newText.ReplaceIgnoreCase(projectGuid.OldGuid, projectGuid.NewGuid);
                }

                if (newText != text)
                {
                    File.WriteAllText(file, newText, Encoding.UTF8);
                }
            }

        }

    }
}
