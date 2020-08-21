using SenticodeTemplate.Interfaces;

namespace SenticodeTemplate.Services.AssetsGenerators
{
    internal sealed class GtkSharpAssetsGenerator : IAssetsGenerator
    {
        public void GenerateAssets(ProjectSettings settings)
        {
            //throw new NotImplementedException();
        }

        #region singleton

        private GtkSharpAssetsGenerator()
        {
        }

        public static GtkSharpAssetsGenerator Instance { get; } = new GtkSharpAssetsGenerator();

        #endregion
    }
}