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
            if (!string.IsNullOrWhiteSpace(_opts.CsvRoot)) Directory.CreateDirectory(_opts.CsvRoot);
            if (!string.IsNullOrWhiteSpace(_opts.TiaRoot)) Directory.CreateDirectory(_opts.TiaRoot);
        }

        private static string SanitizeFolder(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "";

            // invalid filename chars
            var cleaned = string.Concat(name.Split(Path.GetInvalidFileNameChars()));

            // strip any path separators 
            cleaned = cleaned.Replace(Path.DirectorySeparatorChar.ToString(), "")
                             .Replace(Path.AltDirectorySeparatorChar.ToString(), "");

            return cleaned;
        }

        public override async Task<UploadReply> UploadCsvStream(
            IAsyncStreamReader<UploadChunk> requestStream,
            ServerCallContext context)
        {
            string? fileName = null;
            string? folderName = null;
            string destRoot = _opts.CsvRoot;

            var tmp = Path.Combine(_opts.CsvRoot, Path.GetRandomFileName());
            await using var fs = File.Open(tmp, FileMode.Create, FileAccess.Write, FileShare.None);


            await foreach (var chunk in requestStream.ReadAllAsync(context.CancellationToken))
            {
                if (fileName == null && !string.IsNullOrWhiteSpace(chunk.FileName))
                    fileName = Path.GetFileName(chunk.FileName);

                if (folderName == null && !string.IsNullOrWhiteSpace(chunk.Folder))
                {
                    folderName = SanitizeFolder(chunk.Folder);
                    destRoot = Path.Combine(_opts.CsvRoot, folderName);
                    Directory.CreateDirectory(destRoot);
                }


                if (chunk.Data is { Length: > 0 })
                    await fs.WriteAsync(chunk.Data.Memory, context.CancellationToken);
            }

            fileName ??= "upload.csv";
            var final = Path.Combine(destRoot, fileName);

            fs.Close();
            if (File.Exists(final)) File.Delete(final);
            File.Move(tmp, final);

            return new UploadReply { Ok = true, Msg = $"CSV saved to {final}", JobId = Guid.NewGuid().ToString("N") };
        }
    }
}
