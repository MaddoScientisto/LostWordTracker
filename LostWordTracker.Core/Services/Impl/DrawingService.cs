using LostWordTracker.Services;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
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

        public async Task<byte[]> MakeImage(bool drawLevel, bool drawSkills, int columns)
        {
            // Setup font
            var fontBytes = await _databaseService.GetFont("fonts/OpenSans-Regular.ttf");

            using var fontStream = new MemoryStream(fontBytes);



            FontCollection collection = new();
            collection.AddSystemFonts();
            collection.Add(fontStream);
            var fontFamily = collection.Get("Open Sans");
            Font font = fontFamily.CreateFont(14, FontStyle.Regular);
            // Font is set up

            var data = await _dataService.LoadData();

            const int width = 120;
            const int height = 220;

            int maxColumns = columns;

            var usableChars = data.CharacterStorage.Where(x => x.LimitBreak > 0).ToList();

            int rows = (int)Math.Ceiling((float)usableChars.Count / (float)maxColumns);

            int imageWidth = (maxColumns * width) + 20;
            int imageHeight = (rows * height) + 30; // Adding the signature space

            using Image image = new Image<Rgba32>(imageWidth, imageHeight);
            image.Mutate(x => x.Fill(Color.White));

            image.Mutate(x => x.DrawText(new TextOptions(font)
            {
                Origin = new PointF(10, imageHeight - 30),
                WrappingLength = imageWidth - 10,
                HorizontalAlignment = HorizontalAlignment.Left

            }, @"Made with: maddoscientisto.github.io/LostWordTracker", Color.Blue));

            int row = 0;
            int col = 0;



            int i = 0;

            foreach (var item in usableChars)
            {
                var currentBaseChar = data.Characters[item.Id];

                int currentRow = (int)Math.Ceiling((float)i / (float)maxColumns);

                int cursorX = (col * width) + 10;
                int cursorY = (row * height)+10;

                TextOptions options = new(font)
                {
                    Origin = new PointF(cursorX, cursorY + 100),
                    WrappingLength = width,
                    HorizontalAlignment = HorizontalAlignment.Left
                };


                var imgBytes = await _databaseService.GetFont($"images/portraits/{currentBaseChar.Portrait}");
                using var imgStream = new MemoryStream(imgBytes);

                var portrait = await Image.LoadAsync(imgStream);

                image.Mutate(x => x.DrawImage(portrait, new Point(cursorX, cursorY), 1));

                image.Mutate(x => x.DrawText(options, currentBaseChar.ShortName, Color.Black));

                DrawStars(image, 5, 10 + cursorX, cursorY + 150, Color.Black);
                DrawStars(image, item.LimitBreak, 10 + cursorX, cursorY + 150, Color.Red);

                DrawStars(image, 5, 10 + cursorX, cursorY + 170, Color.Black);
                DrawStars(image, item.Awakening, 10 + cursorX, cursorY + 170, Color.Gold);

                if (drawLevel)
                {
                    image.Mutate(x => x.DrawText($"Lv: {item.Level}", font, Color.Black, new PointF(cursorX, cursorY + 180)));
                }
                if (drawSkills)
                {
                    image.Mutate(x => x.DrawText($"{item.Skill1}/{item.Skill2}/{item.Skill3}", font, Color.Black, new PointF(cursorX + 40, cursorY + 180)));
                }

                i++;
                col++;
                if (col >= maxColumns)
                {
                    col = 0;
                    row++;
                }

              
            }

            

            using var stream = new MemoryStream();

            await image.SaveAsPngAsync(stream);
            return stream.ToArray();
        }

        private void DrawStars(Image baseImage, int count, int x, int y, Color color)
        {
            for (int index = 0; index < count; index++)
            {
                IPath starPolygon = new Star(x: x + 20f * index, y: y, prongs: 5, innerRadii: 5.0f, outerRadii: 10.0f, 180);

                baseImage.Mutate(x => x.Fill(color, starPolygon));
            }
        }
    }
}
