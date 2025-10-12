using Grpc.Core;
using Microsoft.Extensions.Options;
using PlcCreatorSystem.Generator;
using System.IO;

namespace PlcCreatorSystem_Generator.Services
{
    public class CsvServiceImpl : GeneratorService.GeneratorServiceBase
    {
        private readonly StorageOptions _opts;
        public CsvServiceImpl(IOptions<StorageOptions> opts)
        {
            _opts = opts.Value;
            Directory.CreateDirectory(_opts.CsvRoot);
            Directory.CreateDirectory(_opts.TiaRoot);
        }

        public override async Task<UploadReply> UploadCsvStream(
            IAsyncStreamReader<UploadChunk> requestStream,
            ServerCallContext context)
        {
            string? fileName = null;
            var tmp = Path.Combine(_opts.CsvRoot, Path.GetRandomFileName());
            await using var fs = File.Open(tmp, FileMode.Create, FileAccess.Write, FileShare.None);

            await foreach (var chunk in requestStream.ReadAllAsync(context.CancellationToken))
            {
                if (fileName == null && !string.IsNullOrWhiteSpace(chunk.FileName))
                    fileName = Path.GetFileName(chunk.FileName);

                if (chunk.Data is { Length: > 0 })
                    await fs.WriteAsync(chunk.Data.Memory, context.CancellationToken);
            }

            fileName ??= "upload.csv";
            var final = Path.Combine(_opts.CsvRoot, fileName);
            fs.Close();
            if (File.Exists(final)) File.Delete(final);
            File.Move(tmp, final);

            return new UploadReply { Ok = true, Msg = $"CSV saved to {final}", JobId = Guid.NewGuid().ToString("N") };
        }
    }
}
