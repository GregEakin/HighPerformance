// High Performance C# Multithreading Programming
// Copyright(C) 2017  Greg Eakin

// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License along
// with this program; if not, write to the Free Software Foundation, Inc.,
// 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HighPerformanceTests.Progress
{
    [TestClass]
    public class ProgressReportingTests
    {
        public class Progress<T> : IProgress<T>
        {
            private readonly SynchronizationContext _context;

            public Progress()
            {
                _context = SynchronizationContext.Current
                    ?? new SynchronizationContext();
            }

            public Progress(Action<T> action)
                : this()
            {
                ProgressReported += action;
            }

            protected virtual void OnReport(T value)
            {
                throw new NotImplementedException();
            }

            // public event EventHandler<T> ProgressChanged;
            public event Action<T> ProgressReported;

            public void Report(T value)
            {
                var action = ProgressReported;
                if (action != null)
                    _context.Post(arg => action((T)arg), value);
            }
        }

        public Task<ReadOnlyCollection<FileInfo>> FindFilesAsync(string pattern, CancellationToken cancellationToken, IProgress<Tuple<double, ReadOnlyCollection<List<FileInfo>>>> progress)
        {
            if (string.IsNullOrWhiteSpace(pattern)) throw new ArgumentNullException(nameof(pattern));
            cancellationToken.ThrowIfCancellationRequested();
            progress.Report(new Tuple<double, ReadOnlyCollection<List<FileInfo>>>(0.5, null));
            var fileInfos = new List<FileInfo> { new FileInfo("aa") };
            var readOnlyCollection = new ReadOnlyCollection<FileInfo>(fileInfos);
            return Task.FromResult(readOnlyCollection);
        }

        public Task<ReadOnlyCollection<FileInfo>> FindFilesAsync(string pattern, CancellationToken cancellationToken, IProgress<int> progress)
        {
            if (string.IsNullOrWhiteSpace(pattern)) throw new ArgumentNullException(nameof(pattern));
            cancellationToken.ThrowIfCancellationRequested();
            progress.Report(33);
            var fileInfos = new List<FileInfo> { new FileInfo("aa") };
            var readOnlyCollection = new ReadOnlyCollection<FileInfo>(fileInfos);
            return Task.FromResult(readOnlyCollection);
        }

        [TestMethod]
        public void Test1()
        {
            var cancellationTokenSource = new CancellationTokenSource();

            var progress = new Progress<int>();
            progress.ProgressReported += Console.WriteLine;

            var filesAsync = FindFilesAsync("fish", cancellationTokenSource.Token, progress);

            // we can do work here.

            // we can cancel the find
            // cancellationTokenSource.Cancel();

            foreach (var file in filesAsync.Result)
                Console.WriteLine(file);
        }

        private async Task<ReadOnlyCollection<FileInfo>> FindFiles2Async(string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern)) throw new ArgumentNullException(nameof(pattern));
            var fileInfos = new List<FileInfo> { new FileInfo("aa") };
            var readOnlyCollection = new ReadOnlyCollection<FileInfo>(fileInfos);
            return await Task.FromResult(readOnlyCollection);
        }

        [TestMethod]
        public void Test2()
        {
            var filesAsync = FindFiles2Async("fish");

            // we can do work here.

            foreach (var file in filesAsync.Result)
                Console.WriteLine(file);
        }

        public static async Task<string> PollAsync(Uri url, CancellationToken cancellationToken, IProgress<bool> progress)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
            var data = await Task.FromResult("download data");
            progress.Report(true);
            // cancellationToken.ThrowIfCancellationRequested();
            return data;
        }

        [TestMethod]
        public void Test3()
        {
            var progress = new System.Progress<bool>();
            progress.ProgressChanged += (sender, b) => Console.WriteLine($"status: {b}");
            var pollAsync = PollAsync(new Uri("http://andromeda"), CancellationToken.None, progress);
            Console.WriteLine(pollAsync.Status);
            pollAsync.Wait();
            Console.WriteLine(pollAsync.Status);
            Console.WriteLine($"results: {pollAsync.Result}");
        }
    }
}