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
using SolutionCreator.SolutionProcessor.Factory.Interfaces;
using System;
using System.IO;

namespace SolutionCreator
{
    public class Creator : ICreator
    {
        private readonly ISolutionProcessorFactory SolutionProcessorFactory;

        public Creator(ISolutionProcessorFactory solutionProcessorFactory)
        {
            SolutionProcessorFactory = solutionProcessorFactory;
        }

        public void Create(string sourceDir, string destinationBaseDir, string newSolutionName)
        {
            var destinationDir = Path.Combine(destinationBaseDir, newSolutionName);

            ValidateDestinationFolder(destinationDir);

            var solutionProcessor = SolutionProcessorFactory.Get(sourceDir);

            solutionProcessor.Create(sourceDir, destinationDir, newSolutionName);
        }

        private void ValidateDestinationFolder(string folder)
        {
            if (string.IsNullOrWhiteSpace(folder)) throw new ArgumentNullException(nameof(folder));

            if (Directory.Exists(folder)) throw new DirectoryNotFoundException("The provided folder already exists.");
        }

    }
}
