namespace SolutionCreator.SolutionProcessor.Interfaces
{
    public interface ISolutionProcessor
    {
        SolutionProcessorType SolutionProcessorType { get; }
        bool CanProcess(string sourceDir);
        void Create(string sourceDir, string destinationDir, string newSolutionName);
    }
}
