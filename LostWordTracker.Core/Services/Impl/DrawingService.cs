using LostWordTracker.Services;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostWordTracker.Core.Services.Impl
{
    public class DrawingService : IDrawingService
    {
        private readonly IDataService _dataService;
        private readonly IDatabaseService _databaseService;
        public DrawingService(IDataService dataService, IDatabaseService databaseService)
        {
            _dataService = dataService;
            _databaseService = databaseService;
        }

        public async Task<byte[]> MakeImage()
        {

            var fontBytes = await _databaseService.GetFont("fonts/arial.ttf");

            using var fontStream = new MemoryStream(fontBytes);

            using Image image = new Image<Rgba32>(400, 400);
            image.Mutate(x => x.Fill(Color.White));

            //
            FontCollection collection = new();
            collection.AddSystemFonts();
            collection.Add(fontStream);
            


            var fontFamily = collection.Get("Arial");

            Font font = fontFamily.CreateFont(12, FontStyle.Regular);

            image.Mutate(x => x.DrawText("Name", font, Color.Black, new PointF(1, 1)));

            using var stream = new MemoryStream();

            await image.SaveAsPngAsync(stream);

            return stream.ToArray();
        }
    }
}
