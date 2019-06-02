using System.ComponentModel;

namespace SolutionCreator.SolutionProcessor.Interfaces
{
    public enum SolutionProcessorType
    {
        [Description("ASP.NET MVC")]
        AspNetMvc,

        [Description("Angular")]
        Angular,
    }
}
