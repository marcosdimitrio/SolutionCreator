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
using SolutionCreator.Enums;
using SolutionCreator.GitIgnore;
using SolutionCreator.GitIgnore.Factory.Interfaces;
using SolutionCreator.Interfaces;
using System;
using System.IO;

namespace SolutionCreator
{
    // http://stackoverflow.com/questions/58744/copy-the-entire-contents-of-a-directory-in-c-sharp#690980
    public class FileCopy : IFileCopy
    {
        private readonly IGitIgnoreFilterFactory GitIgnoreFilterFactory;

        public event EventHandler<FileProcessingProgressDto> FileProcessingProgress;

        public FileCopy(IGitIgnoreFilterFactory gitIgnoreFilterFactory)
        {
            GitIgnoreFilterFactory = gitIgnoreFilterFactory;
        }

        public void Copy(SolutionType solutionType, string sourceDirectory, string targetDirectory, SolutionName solutionName)
        {
            var diSource = new DirectoryInfo(sourceDirectory);
            var diTarget = new DirectoryInfo(targetDirectory);

            var gitIgnoreFilter = GitIgnoreFilterFactory.Get(solutionType);

            CopyAll(gitIgnoreFilter, diSource, diTarget, solutionName);

            RaiseEmptyFileProcessingProgressMessage();
        }

        private void CopyAll(IGitIgnoreFilter gitIgnoreFilter, DirectoryInfo sourceDirectory, DirectoryInfo targetDirectory, SolutionName solutionName)
        {
            if (gitIgnoreFilter.AcceptsFolder(targetDirectory.FullName))
            {
                Directory.CreateDirectory(targetDirectory.FullName);

                // Copy each file into the new directory.
                foreach (var fileInfo in sourceDirectory.GetFiles())
                {
                    var newName = AdaptName(fileInfo.Name, solutionName);

                    var newPath = Path.Combine(targetDirectory.FullName, newName);

                    if (gitIgnoreFilter.AcceptsFile(newPath))
                    {
                        RaiseFileProcessingProgressForFile(newPath);

                        fileInfo.CopyTo(newPath, true);
                    }
                }

                // Copy each subdirectory using recursion.
                foreach (var subDirectoryInfo in sourceDirectory.GetDirectories())
                {

                    if (gitIgnoreFilter.AcceptsFolder(subDirectoryInfo.FullName))
                    {
                        var newName = AdaptName(subDirectoryInfo.Name, solutionName);

                        var nextTargetSubDir = targetDirectory.CreateSubdirectory(newName);

                        CopyAll(gitIgnoreFilter, subDirectoryInfo, nextTargetSubDir, solutionName);
                    }

                }
            }

        }

        private void RaiseFileProcessingProgressForFile(string newPath)
        {
            FileProcessingProgress?.Invoke(this, new FileProcessingProgressDto() { Message = $"Copying {newPath}" });
        }

        private string AdaptName(string name, SolutionName solutionName)
        {
            return name.Replace(solutionName.OldName, solutionName.NewName);
        }

        private void RaiseEmptyFileProcessingProgressMessage()
        {
            FileProcessingProgress?.Invoke(this, new FileProcessingProgressDto() { Message = "" });
        }

    }
}
