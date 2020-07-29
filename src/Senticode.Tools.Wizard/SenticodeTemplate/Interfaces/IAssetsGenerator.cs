using SenticodeTemplate.Services;

namespace SenticodeTemplate.Interfaces
{
    internal interface IAssetsGenerator
    {
        void GenerateAssets(ProjectSettings settings);
    }
}