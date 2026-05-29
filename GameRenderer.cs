// AI-generated
using Silk.NET.SDL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using TheAdventure.Models.Game;
using TheAdventure.Models.Plants;
using TheAdventure.Models.Shop;

namespace TheAdventure;

public unsafe class GameRenderer : IDisposable
{
    private readonly Sdl _sdl;
    private Renderer* _renderer;
    private bool _disposed = false;

    private readonly Dictionary<string, int> _textureCache = new();
    private readonly Dictionary<int, IntPtr> _texturePointers = new();
    private int _nextTextureId = 0;

    public GameRenderer(Sdl sdl, IntPtr window)
    {
        _sdl = sdl;
        _renderer = (Renderer*)sdl.CreateRenderer((Window*)window, -1,
            (uint)RendererFlags.Accelerated);
        _sdl.RenderSetVSync(_renderer, 1);
    }

    public int LoadTexture(string path, out int width, out int height)
    {
        width = 0;
        height = 0;

        if (_textureCache.TryGetValue(path, out var cachedId))
            return cachedId;

        if (!File.Exists(path))
            return -1;

        try
        {
            using var fStream = new FileStream(path, FileMode.Open);
            using var image = SixLabors.ImageSharp.Image.Load<Rgba32>(fStream);
            width = image.Width;
            height = image.Height;

            var imageRawData = new byte[width * height * 4];
            image.CopyPixelDataTo(imageRawData.AsSpan());

            IntPtr texture;
            fixed (byte* data = imageRawData)
            {
                var surface = _sdl.CreateRGBSurfaceWithFormatFrom(
                    data, width, height, 8, width * 4,
                    (uint)Silk.NET.SDL.PixelFormatEnum.Rgba32);
                texture = (IntPtr)_sdl.CreateTextureFromSurface(_renderer, surface);
                _sdl.FreeSurface(surface);
            }

            var id = _nextTextureId++;
            _texturePointers[id] = texture;
            _textureCache[path] = id;
            return id;
        }
        catch
        {
            return -1;
        }
    }

    public void RenderTexture(int textureId, int x, int y, int w, int h)
    {
        if (!_texturePointers.TryGetValue(textureId, out var ptr)) return;
        var dst = new Silk.NET.Maths.Rectangle<int>(x, y, w, h);
        _sdl.RenderCopy(_renderer, (Texture*)ptr, (Silk.NET.Maths.Rectangle<int>*)null, in dst);
    }

    public void Clear()
    {
        _sdl.SetRenderDrawColor(_renderer, 20, 12, 28, 255);
        _sdl.RenderClear(_renderer);
    }

    public void Present() => _sdl.RenderPresent(_renderer);

    private void FillRect(int x, int y, int w, int h, byte r, byte g, byte b, byte a = 255)
    {
        _sdl.SetRenderDrawColor(_renderer, r, g, b, a);
        var rect = new Silk.NET.Maths.Rectangle<int>(x, y, w, h);
        _sdl.RenderFillRect(_renderer, in rect);
    }

    private void DrawRect(int x, int y, int w, int h, byte r, byte g, byte b)
    {
        _sdl.SetRenderDrawColor(_renderer, r, g, b, 255);
        var rect = new Silk.NET.Maths.Rectangle<int>(x, y, w, h);
        _sdl.RenderDrawRect(_renderer, in rect);
    }

    private void DrawDigit(int digit, int x, int y, int size, byte r, byte g, byte b)
    {
        bool[][] d0 = { [true, true, true], [true, false, true], [true, false, true], [true, false, true], [true, false, true], [true, false, true], [true, true, true] };
        bool[][] d1 = { [false, true, false], [true, true, false], [false, true, false], [false, true, false], [false, true, false], [false, true, false], [true, true, true] };
        bool[][] d2 = { [true, true, true], [false, false, true], [false, false, true], [false, true, true], [true, false, false], [true, false, false], [true, true, true] };
        bool[][] d3 = { [true, true, true], [false, false, true], [false, false, true], [false, true, true], [false, false, true], [false, false, true], [true, true, true] };
        bool[][] d4 = { [true, false, true], [true, false, true], [true, false, true], [true, true, true], [false, false, true], [false, false, true], [false, false, true] };
        bool[][] d5 = { [true, true, true], [true, false, false], [true, false, false], [true, true, true], [false, false, true], [false, false, true], [true, true, true] };
        bool[][] d6 = { [true, true, true], [true, false, false], [true, false, false], [true, true, true], [true, false, true], [true, false, true], [true, true, true] };
        bool[][] d7 = { [true, true, true], [false, false, true], [false, false, true], [false, true, false], [false, true, false], [false, true, false], [false, true, false] };
        bool[][] d8 = { [true, true, true], [true, false, true], [true, false, true], [true, true, true], [true, false, true], [true, false, true], [true, true, true] };
        bool[][] d9 = { [true, true, true], [true, false, true], [true, false, true], [true, true, true], [false, false, true], [false, false, true], [true, true, true] };

        var map = digit switch
        {
            0 => d0,
            1 => d1,
            2 => d2,
            3 => d3,
            4 => d4,
            5 => d5,
            6 => d6,
            7 => d7,
            8 => d8,
            _ => d9
        };

        for (int row = 0; row < 7; row++)
            for (int col = 0; col < 3; col++)
                if (map[row][col])
                    FillRect(x + col * size, y + row * size, size, size, r, g, b);
    }

    private void DrawLetter(char letter, int x, int y, int size, byte r, byte g, byte b)
    {
        bool[][] a = { [true, true, true], [true, false, true], [true, false, true], [true, true, true], [true, false, true], [true, false, true], [true, false, true] };
        bool[][] _b = { [true, true, false], [true, false, true], [true, false, true], [true, true, false], [true, false, true], [true, false, true], [true, true, false] };
        bool[][] c = { [true, true, true], [true, false, false], [true, false, false], [true, false, false], [true, false, false], [true, false, false], [true, true, true] };
        bool[][] d = { [true, true, false], [true, false, true], [true, false, true], [true, false, true], [true, false, true], [true, false, true], [true, true, false] };
        bool[][] e = { [true, true, true], [true, false, false], [true, false, false], [true, true, true], [true, false, false], [true, false, false], [true, true, true] };
        bool[][] f = { [true, true, true], [true, false, false], [true, false, false], [true, true, true], [true, false, false], [true, false, false], [true, false, false] };
        bool[][] g_ = { [true, true, true], [true, false, false], [true, false, false], [true, false, true], [true, false, true], [true, false, true], [true, true, true] };
        bool[][] h = { [true, false, true], [true, false, true], [true, false, true], [true, true, true], [true, false, true], [true, false, true], [true, false, true] };
        bool[][] i = { [true, true, true], [false, true, false], [false, true, false], [false, true, false], [false, true, false], [false, true, false], [true, true, true] };
        bool[][] j = { [false, false, true], [false, false, true], [false, false, true], [false, false, true], [true, false, true], [true, false, true], [true, true, true] };
        bool[][] k = { [true, false, true], [true, false, true], [true, true, false], [true, false, false], [true, true, false], [true, false, true], [true, false, true] };
        bool[][] l = { [true, false, false], [true, false, false], [true, false, false], [true, false, false], [true, false, false], [true, false, false], [true, true, true] };
        bool[][] m = { [true, false, true], [true, true, true], [true, true, true], [true, false, true], [true, false, true], [true, false, true], [true, false, true] };
        bool[][] n = { [true, false, true], [true, true, true], [true, false, true], [true, false, true], [true, false, true], [true, false, true], [true, false, true] };
        bool[][] o = { [true, true, true], [true, false, true], [true, false, true], [true, false, true], [true, false, true], [true, false, true], [true, true, true] };
        bool[][] p = { [true, true, true], [true, false, true], [true, false, true], [true, true, true], [true, false, false], [true, false, false], [true, false, false] };
        bool[][] q = { [true, true, true], [true, false, true], [true, false, true], [true, false, true], [true, false, true], [true, true, true], [false, false, true] };
        bool[][] r_ = { [true, true, true], [true, false, true], [true, false, true], [true, true, false], [true, true, false], [true, false, true], [true, false, true] };
        bool[][] s = { [true, true, true], [true, false, false], [true, false, false], [true, true, true], [false, false, true], [false, false, true], [true, true, true] };
        bool[][] t = { [true, true, true], [false, true, false], [false, true, false], [false, true, false], [false, true, false], [false, true, false], [false, true, false] };
        bool[][] u = { [true, false, true], [true, false, true], [true, false, true], [true, false, true], [true, false, true], [true, false, true], [true, true, true] };
        bool[][] v = { [true, false, true], [true, false, true], [true, false, true], [true, false, true], [true, false, true], [false, true, false], [false, true, false] };
        bool[][] w = { [true, false, true], [true, false, true], [true, false, true], [true, false, true], [true, true, true], [true, true, true], [true, false, true] };
        bool[][] x_ = { [true, false, true], [true, false, true], [false, true, false], [false, true, false], [false, true, false], [true, false, true], [true, false, true] };
        bool[][] y_ = { [true, false, true], [true, false, true], [true, false, true], [true, true, true], [false, true, false], [false, true, false], [false, true, false] };
        bool[][] z = { [true, true, true], [false, false, true], [false, false, true], [false, true, false], [true, false, false], [true, false, false], [true, true, true] };

        if (letter < 'A' || letter > 'Z') return;

        var map = letter switch
        {
            'A' => a,
            'B' => _b,
            'C' => c,
            'D' => d,
            'E' => e,
            'F' => f,
            'G' => g_,
            'H' => h,
            'I' => i,
            'J' => j,
            'K' => k,
            'L' => l,
            'M' => m,
            'N' => n,
            'O' => o,
            'P' => p,
            'Q' => q,
            'R' => r_,
            'S' => s,
            'T' => t,
            'U' => u,
            'V' => v,
            'W' => w,
            'X' => x_,
            'Y' => y_,
            'Z' => z,
            _ => a
        };

        for (int row = 0; row < 7; row++)
            for (int col = 0; col < 3; col++)
                if (map[row][col])
                    FillRect(x + col * size, y + row * size, size, size, r, g, b);
    }

    private void DrawNumber(int number, int x, int y, int size, byte r, byte g, byte b)
    {
        var digits = number.ToString();
        int offsetX = 0;
        foreach (var ch in digits)
        {
            DrawDigit(ch - '0', x + offsetX, y, size, r, g, b);
            offsetX += 4 * size;
        }
    }

    private void DrawText(string text, int x, int y, int size, byte r, byte g, byte b)
    {
        if (string.IsNullOrEmpty(text)) return;

        int offsetX = 0;
        foreach (var ch in text.ToUpper())
        {
            if (ch == ' ' || ch == ':' || ch == '!')
            {
                offsetX += 4 * size;
                continue;
            }

            if (char.IsDigit(ch))
            {
                DrawDigit(ch - '0', x + offsetX, y, size, r, g, b);
            }
            else if (ch >= 'A' && ch <= 'Z')
            {
                DrawLetter(ch, x + offsetX, y, size, r, g, b);
            }

            offsetX += 4 * size;
        }
    }

    public void RenderGame(GameLogic logic, string? message)
    {
        Clear();

        var state = logic.State;

        // Background
        FillRect(0, 0, 800, 800, 20, 12, 28);

        // Title bar
        FillRect(0, 0, 750, 50, 80, 40, 100);
        DrawRect(0, 0, 750, 50, 200, 100, 220);
        DrawText("POTION GARDEN", 20, 15, 3, 255, 200, 255);

        // Garden area label
        FillRect(20, 70, 710, 30, 40, 20, 60);
        DrawText("GARDEN", 30, 75, 2, 180, 150, 200);

        // Garden beds (4 beds)
        for (int i = 0; i < logic.GardenBeds.Length; i++)
        {
            int bx = 20 + i * 175;
            int by = 110;
            var plant = logic.GardenBeds[i];

            // Bed background
            FillRect(bx, by, 160, 160, 101, 67, 33);
            DrawRect(bx, by, 160, 160, 139, 90, 43);

            // Bed number
            DrawNumber(i + 1, bx + 5, by + 5, 2, 200, 180, 100);

            // VERIFICARE CRITICA: Doar daca stratul NU este null desenam planta!
            if (plant != null)
            {
                var framePath = plant.GetCurrentFramePath();
                if (framePath != null)
                {
                    var texId = LoadTexture(framePath, out var tw, out var th);
                    if (texId >= 0)
                    {
                        int size = 128;
                        int px = bx + (160 - size) / 2;
                        int py = by + (160 - size) / 2;
                        RenderTexture(texId, px, py, size, size);
                    }
                }
                else
                {
                    var (r, g, b) = plant.GetStageColor();
                    int size = plant.Stage switch
                    {
                        PlantStage.Seedling => 30,
                        PlantStage.Growing => 60,
                        PlantStage.ReadyToHarvest => 90,
                        PlantStage.Wilted => 40,
                        _ => 30
                    };
                    int px = bx + (160 - size) / 2;
                    int py = by + (160 - size) / 2;
                    FillRect(px, py, size, size, r, g, b);
                }

                // Water indicator
                if (plant.NeedsWater)
                {
                    FillRect(bx + 135, by + 5, 20, 20, 0, 150, 255);
                    DrawText("W", bx + 137, by + 7, 2, 255, 255, 255);
                }

                // Ready indicator
                if (plant.IsReadyToHarvest)
                {
                    FillRect(bx + 5, by + 135, 50, 18, 0, 180, 0);
                    DrawText("RDY", bx + 7, by + 137, 2, 255, 255, 255);
                }
            }
            else
            {
                // Empty bed
                DrawText("EMPTY", bx + 35, by + 70, 2, 150, 100, 50);
            }
        }

        // Inventory panel
        FillRect(20, 285, 350, 215, 30, 15, 45);
        DrawRect(20, 285, 350, 215, 200, 100, 220);
        DrawText("INGREDIENTS", 30, 290, 2, 200, 150, 220);

        var ingredients = logic.Ingredients.Items.ToList();
        for (int i = 0; i < ingredients.Count && i < 5; i++)
        {
            var (name, count) = ingredients[i];
            var color = name switch
            {
                "Magic Carrot" => (255, 140, 0),
                "Magic Tomato" => (255, 50, 50),
                "Magic Corn" => (255, 215, 0),
                "Magic Cabbage" => (0, 200, 100),
                _ => (200, 200, 200)
            };
            FillRect(30, 312 + i * 35, 16, 16,
                (byte)color.Item1, (byte)color.Item2, (byte)color.Item3);
            DrawNumber(count, 52, 312 + i * 35, 2,
                (byte)color.Item1, (byte)color.Item2, (byte)color.Item3);
            DrawText(name, 75, 312 + i * 35, 2,
                (byte)color.Item1, (byte)color.Item2, (byte)color.Item3);
        }

        // Potions panel
        FillRect(380, 285, 350, 215, 30, 15, 45);
        DrawRect(380, 285, 350, 215, 200, 100, 220);
        DrawText("POTIONS", 390, 290, 2, 200, 150, 220);

        var potions = logic.PotionsInStock;
        for (int i = 0; i < potions.Count && i < 5; i++)
        {
            var (r, g, b) = potions[i].Color;
            FillRect(390, 312 + i * 35, 16, 16, r, g, b);
            DrawText(potions[i].Name, 412, 312 + i * 35, 2, r, g, b);
        }

        // Customers panel
        FillRect(20, 510, 710, 135, 25, 12, 40);
        DrawRect(20, 510, 710, 135, 200, 100, 220);
        DrawText("CUSTOMERS", 30, 515, 2, 200, 150, 220);

        var customers = logic.Customers;
        for (int i = 0; i < customers.Count && i < 3; i++)
        {
            int cx = 30 + i * 240;
            var moodColor = customers[i].Mood switch
            {
                CustomerMood.Happy => (0, 255, 100),
                CustomerMood.Neutral => (255, 200, 0),
                CustomerMood.Impatient => (255, 50, 50),
                _ => (200, 200, 200)
            };
            FillRect(cx, 530, 225, 105, 50, 25, 70);
            DrawRect(cx, 530, 225, 105,
                (byte)moodColor.Item1, (byte)moodColor.Item2, (byte)moodColor.Item3);

            // Customer icon
            FillRect(cx + 5, 535, 30, 30,
                (byte)moodColor.Item1, (byte)moodColor.Item2, (byte)moodColor.Item3);

            // Customer name
            DrawText(customers[i].Name, cx + 40, 537, 2,
                (byte)moodColor.Item1, (byte)moodColor.Item2, (byte)moodColor.Item3);

            // Afisam textul exact al potiunii cerute din instanta reala generata de logica stabila
            DrawText("WANTS", cx + 5, 570, 2, 180, 150, 200);
            DrawText(customers[i].RequestedPotion, cx + 5, 588, 2, 255, 255, 100);

            // Sell button
            FillRect(cx + 160, 600, 60, 25, 180, 50, 180);
            DrawText("SELL", cx + 163, 605, 2, 255, 255, 255);
        }

        // HUD
        FillRect(0, 655, 750, 65, 50, 25, 70);
        DrawRect(0, 655, 750, 65, 200, 100, 220);

        // Gold
        FillRect(10, 662, 20, 20, 255, 215, 0);
        DrawText("GOLD", 35, 660, 2, 255, 215, 0);
        DrawNumber(state.Gold, 100, 660, 3, 255, 215, 0);

        // Score
        FillRect(10, 685, 20, 20, 255, 105, 180);
        DrawText("SCORE", 35, 683, 2, 255, 105, 180);
        DrawNumber(state.Score, 110, 683, 2, 255, 105, 180);

        // High Score
        DrawText("BEST", 280, 660, 2, 150, 150, 255);
        DrawNumber(state.HighScore, 360, 660, 3, 150, 150, 255);

        // Shop level
        DrawText("SHOP LVL", 480, 660, 2, 200, 200, 100);
        DrawNumber(state.ShopLevel, 620, 660, 3, 255, 215, 0);

        // Upgrade hint
        if (state.CanUpgradeShop)
        {
            FillRect(480, 678, 200, 18, 100, 200, 100);
            DrawText("U TO UPGRADE", 485, 680, 2, 0, 0, 0);
        }

        // Notification
        if (message != null)
        {
            FillRect(100, 245, 550, 30, 180, 100, 200, 230);
            DrawRect(100, 245, 550, 30, 255, 200, 255);
            DrawText(message, 110, 250, 2, 255, 255, 255);
        }

        // Victory overlay
        if (state.Phase == GamePhase.Victory)
        {
            FillRect(75, 200, 600, 180, 0, 80, 0, 220);
            DrawRect(75, 200, 600, 180, 50, 255, 100);
            DrawText("YOU WIN", 150, 240, 5, 50, 255, 100);
            DrawText("SHOP LEVEL 3 REACHED", 90, 310, 2, 200, 255, 200);
        }

        Present();
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            foreach (var ptr in _texturePointers.Values)
                _sdl.DestroyTexture((Texture*)ptr);
            if (_renderer != null)
            {
                _sdl.DestroyRenderer(_renderer);
                _renderer = null;
            }
            _disposed = true;
            GC.SuppressFinalize(this);
        }
    }
}
// end AI-generated