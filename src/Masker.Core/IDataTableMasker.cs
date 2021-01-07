using System.Data;

namespace Masker.Core
{
    public interface IDataTableMasker
    {
        DataTable Mask(DataTable dt, int[] indices = null);
    }
}