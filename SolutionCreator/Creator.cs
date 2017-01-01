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
using SolutionCreator.GuidReplaceService;
using System;
using System.IO;

namespace SolutionCreator
{
    public class Creator : ICreator
    {
        private readonly IFileCopy FileCopy;
        private readonly IGuidReplace GuidReplace;

        public Creator(IFileCopy fileCopy, IGuidReplace guidReplace)
        {
            FileCopy = fileCopy;
            GuidReplace = guidReplace;
        }

        public void Create(string sourceDir, string destinationBaseDir, string newSolutionName)
        {
            var newSolutionDir = Path.Combine(destinationBaseDir, newSolutionName);

            ValidateSourceFolder(sourceDir);
            ValidateDestinationFolder(newSolutionDir);

            var oldSolutionName = IdentifySourceSolutionName(sourceDir);

            var solutionName = new SolutionName()
            {
                OldName = oldSolutionName,
                NewName = newSolutionName
            };

            FileCopy.Copy(sourceDir, newSolutionDir, solutionName);

            GuidReplace.Replace(newSolutionDir, solutionName);

        }

        private string IdentifySourceSolutionName(string sourceDir)
        {
            var solutionFile = Directory.GetFiles(sourceDir, "*.sln");

            return Path.GetFileNameWithoutExtension(solutionFile[0]);
        }

        private void ValidateSourceFolder(string folder)
        {
            if (string.IsNullOrWhiteSpace(folder)) throw new ArgumentNullException(nameof(folder));

            if (!Directory.Exists(folder)) throw new DirectoryNotFoundException("The provided folder doesn't exist.");

            if (Directory.GetFiles(folder, "*.sln").Length == 0) throw new ArgumentException("There are no solution files on the provided folder.");

            if (Directory.GetFiles(folder, "*.sln").Length != 1) throw new ArgumentException("There are more than one solution file on the provided folder.");
        }

        private void ValidateDestinationFolder(string folder)
        {
            if (string.IsNullOrWhiteSpace(folder)) throw new ArgumentNullException(nameof(folder));

            if (Directory.Exists(folder)) throw new DirectoryNotFoundException("The provided folder already exists.");
        }

    }
}
