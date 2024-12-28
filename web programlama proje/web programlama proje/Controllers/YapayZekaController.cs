using Microsoft.AspNetCore.Mvc;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using web_programlama_proje.Models;

public class YapayZekaController : Controller
{
    private readonly string _cloudinaryCloudName = "dvdgcsmem";
    private readonly string _cloudinaryApiKey = "373167841528198";
    private readonly string _cloudinaryApiSecret = "2JfJPX-cJsbpIlOCpjGqTqkdKYE";
    private readonly string _lightXApiKey = "e2ac32e96dd94ed6bd1125ba6d241e3d_cc4bab681eba44c68839eabc2f348a87_andoraitools";

    [HttpGet]
    public IActionResult Index()
    {
        return View(new YapayZeka());
    }

    [HttpPost]
    public async Task<IActionResult> Index(YapayZeka model)
    {
        if (model.Img == null || string.IsNullOrEmpty(model.Acıklama))
        {
            ModelState.AddModelError("", "Lütfen bir resim ve metin girin.");
            return View(model);
        }

        var tempPath = Path.GetTempFileName();
        using (var stream = System.IO.File.Create(tempPath))
        {
            await model.Img.CopyToAsync(stream);
        }

        try
        {

            // Cloudinary'e yükle
            string publicImageUrl = await UploadToCloudinaryAsync(tempPath);
            Console.WriteLine($"Cloudinary'den Alınan Public URL: {publicImageUrl}");

            // LightX API
            string resultImageUrl = await ProcessWithLightXAsync(publicImageUrl, model.Acıklama);
            Console.WriteLine($"LightX API'den Alınan Sonuç URL: {resultImageUrl}");

            model.ApiResimUrl = resultImageUrl;
            return View(model);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("person face not found"))
            {
                ModelState.AddModelError("", "Yüklediğiniz görselde insan yüzü algılanamadı. Lütfen başka bir görsel yükleyin.");
            }
            else
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
            }
            return View(model);
        }
        finally
        {
            System.IO.File.Delete(tempPath);
        }
    }

    private async Task<string> UploadToCloudinaryAsync(string filePath)
    {
        var account = new Account(_cloudinaryCloudName, _cloudinaryApiKey, _cloudinaryApiSecret);
        var cloudinary = new Cloudinary(account);

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(filePath),
            UseFilename = true,
            UniqueFilename = true,
            Folder = "lightx_images",
            Overwrite = false
        };

        var uploadResult = await cloudinary.UploadAsync(uploadParams);

        if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
        {
            throw new Exception("Cloudinary'e yükleme başarısız oldu.");
        }

        if (string.IsNullOrEmpty(uploadResult.SecureUrl.ToString()))
        {
            throw new Exception("Cloudinary'den beklenen 'link' bilgisi eksik.");
        }

        // Cloudinary'den alınan public URL
        return uploadResult.SecureUrl.ToString();
    }

    private async Task<string> ProcessWithLightXAsync(string imageUrl, string textPrompt)
{
    using var httpClient = new HttpClient();

    var requestUrl = "https://api.lightxeditor.com/external/api/v1/hairstyle";

    var requestData = new
    {
        imageUrl,
        textPrompt
    };

    var json = JsonConvert.SerializeObject(requestData);
    var content = new StringContent(json, Encoding.UTF8, "application/json");

    httpClient.DefaultRequestHeaders.Add("x-api-key", _lightXApiKey);

    // İlk isteği gönderin
    var initialResponse = await httpClient.PostAsync(requestUrl, content);
    var initialContent = await initialResponse.Content.ReadAsStringAsync();

    Console.WriteLine($"LightX API İlk Yanıt: {initialContent}");

    if (!initialResponse.IsSuccessStatusCode)
    {
        throw new Exception($"LightX API hatası: {initialResponse.StatusCode} - {initialContent}");
    }

    var initialData = JsonConvert.DeserializeObject<dynamic>(initialContent);
    string orderId = initialData?.body?.orderId;

    if (string.IsNullOrEmpty(orderId))
    {
        throw new Exception("LightX API yanıtında 'orderId' bilgisi bulunamadı.");
    }

    // İşlem tamamlanana kadar sorgula
    var statusUrl = $"https://api.lightxeditor.com/external/api/v1/hairstyle/status/{orderId}";
    for (int i = 0; i < 10; i++) // Maksimum 10 kez dene
    {   
        await Task.Delay(5000); // 5 saniye bekle

        var statusResponse = await httpClient.GetAsync(statusUrl);
        var statusContent = await statusResponse.Content.ReadAsStringAsync();

        Console.WriteLine($"LightX API Durum Yanıtı: {statusContent}");

        if (!statusResponse.IsSuccessStatusCode)
        {
            throw new Exception($"LightX API durum sorgulama hatası: {statusResponse.StatusCode} - {statusContent}");
        }

        var statusData = JsonConvert.DeserializeObject<dynamic>(statusContent);
        string status = statusData?.body?.status;

        if (status == "completed")
        {
            // İşlem tamamlandı, sonucu alın
            return statusData.body.output.ToString();
        }
        else if (status == "failed")
        {
            throw new Exception("LightX API işlemi başarısız oldu.");
        }
    }

    throw new Exception("LightX API işlemi zaman aşımına uğradı.");
}

    }
