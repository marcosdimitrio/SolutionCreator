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
using SolutionCreator.GitIgnore;
using System.IO;

namespace SolutionCreator
{
    // http://stackoverflow.com/questions/58744/copy-the-entire-contents-of-a-directory-in-c-sharp#690980
    public class FileCopy : IFileCopy
    {

        private readonly IGitIgnoreFilter GitIgnoreFilter;

        public FileCopy(IGitIgnoreFilter gitIgnoreFilter)
        {
            GitIgnoreFilter = gitIgnoreFilter;
        }

        public void Copy(string sourceDirectory, string targetDirectory, SolutionName solutionName)
        {
            var diSource = new DirectoryInfo(sourceDirectory);
            var diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget, solutionName);
        }

        private void CopyAll(DirectoryInfo source, DirectoryInfo target, SolutionName solutionName)
        {

            if (GitIgnoreFilter.Accepts(target.FullName, true))
            {
                Directory.CreateDirectory(target.FullName);

                // Copy each file into the new directory.
                foreach (FileInfo fi in source.GetFiles())
                {
                    var newName = AdaptName(fi.Name, solutionName);

                    var newPath = Path.Combine(target.FullName, newName);

                    if (GitIgnoreFilter.Accepts(newPath, false))
                    {
                        fi.CopyTo(newPath, true);
                    }

                }

                // Copy each subdirectory using recursion.
                foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
                {

                    if (GitIgnoreFilter.Accepts(diSourceSubDir.FullName, true))
                    {
                        var newName = AdaptName(diSourceSubDir.Name, solutionName);

                        var nextTargetSubDir = target.CreateSubdirectory(newName);

                        CopyAll(diSourceSubDir, nextTargetSubDir, solutionName);
                    }

                }
            }

        }

        private string AdaptName(string name, SolutionName solutionName)
        {
            return name.Replace(solutionName.OldName, solutionName.NewName);
        }

    }
}
