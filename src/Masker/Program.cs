using System;
using System.Linq;
using System.Threading.Tasks;
using ConsoleAppFramework;
using Masker.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Masker
{
    class Program : ConsoleAppBase
    {
        private readonly IMaskerService _masterService;

        public Program(IMaskerService masterService)
        {
            _masterService = masterService ?? throw new ArgumentNullException(nameof(masterService));
        }
        static async Task Main(string[] args)
        {
            await Host
                .CreateDefaultBuilder()
                .ConfigureServices(m =>
                {
                    m.AddTransient<IMaskerService, MaskerService>();
                    m.AddTransient<IDataTableMasker, DataTableMasker>();
                    m.AddTransient<IFieldMasker<string>, StringFieldMasker>();
                })
                .RunConsoleAppFrameworkAsync<Program>(args);
        }
        
        public async Task Execute(
            [Option("i","input file t process")] string input,
            [Option("f","field indices separate with comma")] string fields = null,
            [Option("o","output file to generate")] string output = "output.csv",
            [Option("d","delimiter. default ','")] string delimiter = ","
            )
        {
            var indices = fields?
                .Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();
            
            var options = new MaskFileOptions(input, output, delimiter, indices);
            await _masterService.MaskFileAsync(options);
        }
    }
}
