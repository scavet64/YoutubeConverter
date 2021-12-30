using MediaToolkit;
using MediaToolkit.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using VideoLibrary;

namespace YoutubeConverter2.Controllers;

[ApiController]
[Route("[controller]")]
public class DownloadController : Controller
{
    [HttpGet]
    public FileContentResult Get(string url)
    {
        Console.WriteLine($"Beginning Audio Request for {url}");
        if (!Directory.Exists("videos"))
        {
            Directory.CreateDirectory("videos");
        }
        var source = $"videos{Path.DirectorySeparatorChar}";
        var youtube = YouTube.Default;
        var vid = youtube.GetVideo(url);

        var inputFile = new MediaFile { Filename = source + vid.FullName };
        var outputFile = new MediaFile { Filename = $"{source + vid.FullName}.mp3" };

        if (!System.IO.File.Exists(inputFile.Filename))
        {
            Console.WriteLine("Saving Video");
            System.IO.File.WriteAllBytes(source + vid.FullName, vid.GetBytes());
        }

        if (!System.IO.File.Exists(outputFile.Filename))
        {
            Console.WriteLine("Converting Video");
            using var engine = new Engine();
            engine.GetMetadata(inputFile);

            engine.Convert(inputFile, outputFile);
        }


        var bytes = System.IO.File.ReadAllBytes(outputFile.Filename);
        Console.WriteLine($"Ending Audio Request for {url}");
        return File(bytes, "audio/mpeg", true);
    }

    [HttpGet]
    [Route("vid")]
    public FileContentResult Get2(string url)
    {
        Console.WriteLine($"Beginning Video Request for {url}");
        if (!Directory.Exists("videos"))
        {
            Console.WriteLine("Creating Videos Directory");
            Directory.CreateDirectory("videos");
        }
        var source = $"videos{Path.DirectorySeparatorChar}";
        var youtube = YouTube.Default;
        var vid = youtube.GetVideo(url);

        var inputFile = new MediaFile { Filename = source + vid.FullName };

        try
        {
            if (!System.IO.File.Exists(inputFile.Filename))
            {
                Console.WriteLine("Saving Video");
                var yo = vid.GetBytes();
                System.IO.File.WriteAllBytes(source + vid.FullName, yo);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }


        var bytes = System.IO.File.ReadAllBytes(inputFile.Filename);
        Console.WriteLine($"Ending Video Request for {url}");
        return File(bytes, "video/mp4", true);
    }
}
