using SenticodeTemplate.Interfaces;

namespace SenticodeTemplate.Services.AssetsGenerators
{
    internal sealed class WpfAssetsGenerator : IAssetsGenerator
    {
        public void GenerateAssets(ProjectSettings settings)
        {
            //throw new NotImplementedException();
        }

        #region singleton

        private WpfAssetsGenerator()
        {
        }

        public static WpfAssetsGenerator Instance { get; } = new WpfAssetsGenerator();

        #endregion
    }
}