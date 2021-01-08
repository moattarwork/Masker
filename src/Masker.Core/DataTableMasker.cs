using System;
using System.Data;
using System.Linq;

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

            var targetTable = dt.Copy();
            var targetIndices = indices ?? Enumerable.Range(0, dt.Columns.Count).ToArray();

            foreach (var i in targetIndices)
            {
                targetTable.Columns[i].ReadOnly = false;
            }
            
            foreach (DataRow row in targetTable.Rows)
            {
                foreach (var i in targetIndices)
                {
                    row.SetField(i, _fieldMasker.Mask(row[i].ToString()));
                }
            }

            return targetTable;
        }
    }
}