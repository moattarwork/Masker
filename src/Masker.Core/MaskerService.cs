using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using CsvHelper;

namespace Masker.Core
{
    public class MaskerService : IMaskerService
    {
        private readonly IDataTableMasker _masker;

        public MaskerService(IDataTableMasker masker)
        {
            _masker = masker ?? throw new ArgumentNullException(nameof(masker));
        }
        
        public Task MaskFileAsync(MaskFileOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            
            var dt = ReadFileInDataTable(options);

            var result = _masker.Mask(dt, options.fields);
            
            WriteDataTableToOutput(result, options);
            
            return Task.CompletedTask;
        }

        private static void WriteDataTableToOutput(DataTable dt, MaskFileOptions options)
        {
            using var writer = new StreamWriter(options.output);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            
            foreach (DataColumn dc in dt.Columns)
            {
                csv.WriteField(dc.ColumnName);
            }

            csv.NextRecord();

            foreach (DataRow dr in dt.Rows)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    csv.WriteField(dr[dc]);
                }

                csv.NextRecord();
            }

            writer.Flush();
        }

        private static DataTable ReadFileInDataTable(MaskFileOptions options)
        {
            var dt = new DataTable();

            using var reader = new StreamReader(options.input);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            using var dr = new CsvDataReader(csv);
            dt.Load(dr);

            return dt;
        }
    }
}
