using SolutionCreator.Dto;
using SolutionCreator.Enums;
using System;

namespace SolutionCreator.SolutionProcessor.Interfaces
{
    public interface ISolutionProcessor
    {
        event EventHandler<FileProcessingProgressDto> FileProcessingProgress;
        SolutionType SolutionProcessorType { get; }
        bool CanProcess(string sourceDir);
        void Create(string sourceDir, string destinationDir, string newSolutionName);
    }
}
