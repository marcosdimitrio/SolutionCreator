using SolutionCreator.Dto;
using SolutionCreator.GuidReplaceService;
using SolutionCreator.SolutionProcessor.Interfaces;
using System;
using System.IO;

namespace SolutionCreator.SolutionProcessor
{
    public class SolutionProcessorMvc : ISolutionProcessor
    {
        private readonly IFileCopy FileCopy;
        private readonly IGuidReplace GuidReplace;

        public SolutionProcessorMvc(IFileCopy fileCopy, IGuidReplace guidReplace)
        {
            FileCopy = fileCopy;
            GuidReplace = guidReplace;
        }

        public SolutionProcessorType SolutionProcessorType => SolutionProcessorType.AspNetMvc;

        public bool CanProcess(string sourceDir)
        {
            try
            {
                ValidateSourceFolder(sourceDir);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public void Create(string sourceDir, string destinationDir, string newSolutionName)
        {
            var oldSolutionName = IdentifySourceSolutionName(sourceDir);

            var solutionName = new SolutionName()
            {
                OldName = oldSolutionName,
                NewName = newSolutionName
            };

            FileCopy.Copy(sourceDir, destinationDir, solutionName);

            GuidReplace.Replace(destinationDir, solutionName);
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

            if (Directory.GetFiles(folder, "*.sln").Length == 0) throw new ArgumentException("There are no solution files in the provided folder.");

            if (Directory.GetFiles(folder, "*.sln").Length != 1) throw new ArgumentException("There are more than one solution file in the provided folder.");
        }

    }
}
