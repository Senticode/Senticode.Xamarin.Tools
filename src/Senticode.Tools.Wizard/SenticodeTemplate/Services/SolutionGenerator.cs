using SenticodeTemplate.Interfaces;
using SenticodeTemplate.Services.Managers;

namespace SenticodeTemplate.Services
{
    internal class SolutionGenerator
    {
        private readonly IProjectManager[] _managers =
        {
            CommonProjectManager.Instance,
            XamarinProjectManager.Instance,
            WebProjectManager.Instance,
            SolutionProjectManager.Instance
        };

        public void Run()
        {
            foreach (var manager in _managers)
            {
                manager.Compose();
            }
        }

        #region singleton

        private SolutionGenerator()
        {
        }

        public static SolutionGenerator Instance { get; } = new SolutionGenerator();

        #endregion
    }
}