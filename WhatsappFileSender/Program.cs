using WhatsappFileSender;
var currentDirect = Directory.GetCurrentDirectory();

var chunkFiles = Task.Run(new Chunker(Path.Combine(currentDirect, "chunk")).Chunk);
var mountFiles = Task.Run(new Mounter(Path.Combine(currentDirect, "mount")).Mount); 

await Task.WhenAll(chunkFiles, mountFiles);