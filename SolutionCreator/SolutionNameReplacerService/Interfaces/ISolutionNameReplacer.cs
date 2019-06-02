using SolutionCreator.Dto;

namespace SolutionCreator.SolutionNameReplacerService.Interfaces
{
    public interface ISolutionNameReplacer
    {
        void Replace(string newSolutionDir, SolutionName solutionName);
    }
}
