using SolutionCreator.Dto;
using SolutionCreator.Enums;
using SolutionCreator.GuidReplaceService;
using SolutionCreator.Interfaces;
using SolutionCreator.SolutionNameReplacerService.Interfaces;
using SolutionCreator.SolutionProcessor.Interfaces;
using System;
using System.IO;

namespace SolutionCreator.SolutionProcessor.Processors.Mvc
{
    public class SolutionProcessorMvc : ISolutionProcessor
    {
        private readonly IFileCopy FileCopy;
        private readonly IGuidReplace GuidReplace;
        private readonly ISolutionNameReplacer SolutionNameReplacer;

        public event EventHandler<FileProcessingProgressDto> FileProcessingProgress;

        public SolutionType SolutionProcessorType => SolutionType.AspNetMvc;

        public SolutionProcessorMvc(IFileCopy fileCopy, IGuidReplace guidReplace, ISolutionNameReplacer solutionNameReplacer)
        {
            FileCopy = fileCopy;
            GuidReplace = guidReplace;
            SolutionNameReplacer = solutionNameReplacer;

            FileCopy.FileProcessingProgress += RaiseFileProcessingProgressEvent;
        }

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
            ValidateSourceFolder(sourceDir);

            var oldSolutionName = IdentifySourceSolutionName(sourceDir);

            var solutionName = new SolutionName()
            {
                OldName = oldSolutionName,
                NewName = newSolutionName
            };

            FileCopy.Copy(SolutionProcessorType, sourceDir, destinationDir, solutionName);

            ReplaceGuid(destinationDir, solutionName);

            ReplaceSolutionNameOnFiles(destinationDir, solutionName);
        }

        private void ReplaceGuid(string destinationDir, SolutionName solutionName)
        {
            FileProcessingProgress?.Invoke(this, new FileProcessingProgressDto() { Message = "Replacing Guid..." } );

            GuidReplace.Replace(destinationDir, solutionName);
        }

        private void ReplaceSolutionNameOnFiles(string destinationDir, SolutionName solutionName)
        {
            FileProcessingProgress?.Invoke(this, new FileProcessingProgressDto() { Message = "Replacing Solution Name..." });

            SolutionNameReplacer.Replace(destinationDir, solutionName);
        }

        private void RaiseFileProcessingProgressEvent(object sender, FileProcessingProgressDto dto)
        {
            FileProcessingProgress?.Invoke(this, dto);
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
