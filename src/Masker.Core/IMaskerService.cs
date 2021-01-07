using System.Threading.Tasks;

namespace Masker.Core
{
    public interface IMaskerService
    {
        Task MaskFileAsync(MaskFileOptions options);
    }
}