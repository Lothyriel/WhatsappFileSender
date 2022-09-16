namespace WhatsappFileSender
{
    public class Chunker
    {
        private readonly string _filesPath;

        public Chunker(string filesPath)
        {
            Directory.CreateDirectory(filesPath);
            _filesPath = filesPath;
        }

        public void Chunk()
        {

            var files = Directory.GetFiles(_filesPath);

            if (!files.Any())
            {
                Console.WriteLine($"Nenhum arquivo em {_filesPath} para compactar");
            }

            foreach (var file in files)
            {
                ChunkFile(file);
            }
        }

        private void ChunkFile(string filePath)
        {
            const int chunkSize = 1073741824; 
            const int writeBufferSize = 1048576;

            using var inputStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var fileChunks = (double)inputStream.Length / chunkSize;

            for (int i = 0; i < fileChunks; i++)
            {
                var pathParts = filePath.Split("\\");
                var pathWithoutFileName = string.Join("\\", pathParts.Take(pathParts.Length - 1));

                var fileNameWithoutExtension = pathParts.Last().Split(".").First();
                Directory.CreateDirectory(Path.Combine(pathWithoutFileName, fileNameWithoutExtension));
                using var outputStream = new FileStream(Path.Combine(pathWithoutFileName, fileNameWithoutExtension, $"{filePath.Split("\\").Last()}-part-{i}"), FileMode.Append);

                var bytesToRead = Math.Min(inputStream.Length - (long)i * chunkSize, chunkSize);

                var writesCount = (double)bytesToRead / writeBufferSize;

                for (int j = 0; j < writesCount; j++)
                {
                    var buffer = new byte[writeBufferSize];

                    var readSize = inputStream.Read(buffer, 0, writeBufferSize);

                    outputStream.Write(buffer[..readSize], 0, readSize);
                }
            }
        }
    }
}