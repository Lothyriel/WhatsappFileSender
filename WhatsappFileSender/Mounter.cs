namespace WhatsappFileSender
{
    public class Mounter
    {
        private readonly string _filesPath;

        public Mounter(string filesPath)
        {
            Directory.CreateDirectory(filesPath);
            _filesPath = filesPath;
        }
        public void Mount()
        {
            var directories = Directory.GetDirectories(_filesPath);

            foreach (var directory in directories)
            {
                var chunks = Directory.GetFiles(directory);

                var chunksOrdered = chunks.OrderBy(c => int.Parse(c.Split("-").Last()));

                var mountedPath = Path.Combine(directory, "mounted");

                Directory.CreateDirectory(mountedPath);

                var fileName = chunks.First().Split("\\").Last().Split("-part").First();

                using var outputStream = new FileStream(Path.Combine(mountedPath, fileName), FileMode.CreateNew);

                foreach (var chunkPath in chunksOrdered)
                {
                    var bytes = File.ReadAllBytes(chunkPath);
                    outputStream.Write(bytes, 0, bytes.Length);
                }
            }
        }
    }
}