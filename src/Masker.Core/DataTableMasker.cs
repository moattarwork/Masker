using System;
using System.Data;

namespace Masker.Core
{
    public class DataTableMasker : IDataTableMasker
    {
        private readonly IFieldMasker<string> _fieldMasker;

        public DataTableMasker(IFieldMasker<string> fieldMasker)
        {
            _fieldMasker = fieldMasker ?? throw new ArgumentNullException(nameof(fieldMasker));
        }
        
        public DataTable Mask(DataTable dt, int[] indices = null)
        {
            if (dt == null) throw new ArgumentNullException(nameof(dt));

            var clonedDataTable = dt.Clone();
            
            foreach (DataRow row in clonedDataTable.Rows)
            {
                foreach (var i in indices)
                {
                    row[i] = _fieldMasker.Mask(row[i].ToString());
                }
            }

            return clonedDataTable;
        }
    }
}