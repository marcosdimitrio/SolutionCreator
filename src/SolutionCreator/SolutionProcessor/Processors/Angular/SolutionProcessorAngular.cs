using Newtonsoft.Json;
using SolutionCreator.Dto;
using SolutionCreator.Enums;
using SolutionCreator.Interfaces;
using SolutionCreator.SolutionNameReplacerService.Interfaces;
using SolutionCreator.SolutionProcessor.Interfaces;
using SolutionCreator.SolutionProcessor.Processors.Angular.Dto;
using System;
using System.IO;

namespace SolutionCreator.SolutionProcessor.Processors.Angular
{
    public class SolutionProcessorAngular : ISolutionProcessor
    {
        private readonly IFileCopy FileCopy;
        private readonly ISolutionNameReplacer SolutionNameReplacer;

        public event EventHandler<FileProcessingProgressDto> FileProcessingProgress;

        public SolutionType SolutionProcessorType => SolutionType.Angular;

        public SolutionProcessorAngular(IFileCopy fileCopy, ISolutionNameReplacer solutionNameReplacer)
        {
            FileCopy = fileCopy;
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

            ReplaceSolutionNameOnFiles(destinationDir, solutionName);
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
            var solutionFile = Directory.GetFiles(sourceDir, "angular.json");

            var contents = File.ReadAllText(solutionFile[0]);

            var angularJsonDto = JsonConvert.DeserializeObject<AngularJsonDto>(contents);

            return angularJsonDto.DefaultProject;
        }

        private void ValidateSourceFolder(string folder)
        {
            if (string.IsNullOrWhiteSpace(folder)) throw new ArgumentNullException(nameof(folder));

            if (!Directory.Exists(folder)) throw new DirectoryNotFoundException("The provided folder doesn't exist.");

            if (Directory.GetFiles(folder, "angular.json").Length == 0) throw new ArgumentException("There are no solution files in the provided folder.");

            if (Directory.GetFiles(folder, "angular.json").Length != 1) throw new ArgumentException("There are more than one solution file in the provided folder.");
        }

    }
}
