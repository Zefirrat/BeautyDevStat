using System;
using System.Threading;
using System.Threading.Tasks;
using BeautyDevStat.BackgroundJob;
using BeautyDevStat.Git.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace BeautyDevStat.Ui.Electron.Components.Statistic
{
    public partial class SimpleNumber : IDisposable
    {
        [Inject] private IGitStatisticService GitStatisticService { get; set; } = default!;
        private readonly PeriodicWorker _periodicWorker;
        public string? Path { get; set; }
        public string? Value { get; set; }
        public string? Author { get; set; }

        public SimpleNumber()
        {
            _periodicWorker = new PeriodicWorker(ReadValues);
            _periodicWorker.Start(TimeSpan.FromSeconds(5), new CancellationToken());
        }

        private async Task ReadValues(CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(Path) || string.IsNullOrWhiteSpace(Path))
            {
                Value = "null";
                return;
            }

            if (string.IsNullOrEmpty(Author) || string.IsNullOrWhiteSpace(Author))
            {
                Value = "null";
                return;
            }

            try
            {
                var valueInt = await GitStatisticService.GetCommittedLinesCountAsync(Path, Author, cancellationToken);
                Value = valueInt.ToString();
                StateHasChanged();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Value = "Err";
                StateHasChanged();
            }
        }

        public void Dispose()
        {
            _periodicWorker.Dispose();
        }
    }
}