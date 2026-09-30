//4f9ec96ce03f734ff3a1215b400e2d170ab158d4

using System.Net.Http.Headers;
using System.Text.Json;

var apiKey = "4f9ec96ce03f734ff3a1215b400e2d170ab158d4";
var filePath = "testtr.mp3";

if (!File.Exists(filePath))
{
    Console.WriteLine($"Dosya bulunamadı: {filePath}");
    return;
}

using var client = new HttpClient();
client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", apiKey);
using var fileStream = File.OpenRead(filePath);

var content = new StreamContent(fileStream);
content.Headers.ContentType = new MediaTypeHeaderValue("audio/mp3");

var response = await client.PostAsync("https://api.deepgram.com/v1/listen?model=general&language=tr", content);
var json = await response.Content.ReadAsStringAsync();

try
{
    var doc = JsonDocument.Parse(json);
    var transcript = doc.RootElement
        .GetProperty("results")
        .GetProperty("channels")[0]
        .GetProperty("alternatives")[0]
        .GetProperty("transcript")
        .GetString();
    Console.WriteLine();
    Console.WriteLine("Transkript metni : \n");
    Console.WriteLine(transcript);
}
catch (Exception ex)
{
    Console.WriteLine($"Hata oluştu: {ex.Message}");
    Console.WriteLine("\n Gelen yanıt : \n" + json);
    throw;
}