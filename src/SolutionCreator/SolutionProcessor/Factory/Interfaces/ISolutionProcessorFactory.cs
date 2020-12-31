using SolutionCreator.SolutionProcessor.Interfaces;

namespace SolutionCreator.SolutionProcessor.Factory.Interfaces
{
    public interface ISolutionProcessorFactory
    {
        ISolutionProcessor Get(string sourceDir);
    }
}
