// AI-generated
using Silk.NET.SDL;
using TheAdventure;
using TheAdventure.Models.Game;
using SilkKeyCode = TheAdventure.KeyCode;

var sdlContext = new TheAdventure.SdlContext();
var sdl = new Sdl(sdlContext);

var sdlInitResult = sdl.Init(Sdl.InitVideo | Sdl.InitEvents | Sdl.InitTimer);
if (sdlInitResult < 0)
    throw new InvalidOperationException("Failed to initialize SDL.");

IntPtr window;
unsafe
{
    window = (IntPtr)sdl.CreateWindow(
      "Potion Garden",
      50, 10,
      750, 720,
      (uint)WindowFlags.Resizable | (uint)WindowFlags.AllowHighdpi
  );
    if (window == IntPtr.Zero)
        throw new Exception("Failed to create window.");
}

using var renderer = new GameRenderer(sdl, window);
using var logic = new GameLogic();
logic.InitializeGame();

var ev = new Event();
bool quit = false;
string? notificationMessage = null;
double notificationTimer = 0;
var lastTime = DateTimeOffset.UtcNow;

while (!quit)
{
    byte[] keyboardState;
    unsafe
    {
        var span = new ReadOnlySpan<byte>(sdl.GetKeyboardState(null), (int)SilkKeyCode.Count);
        keyboardState = span.ToArray();
    }

    while (sdl.PollEvent(ref ev) != 0)
    {
        if (ev.Type == (uint)EventType.Quit)
        {
            quit = true;
        }
        else if (ev.Type == (uint)EventType.Keydown)
        {
            var key = (SilkKeyCode)ev.Key.Keysym.Scancode;
            var state = logic.State;

            if (state.Phase == GamePhase.Playing)
            {
                // ==================== STRATUL 1 (Tasta 1) ====================
                if (key == SilkKeyCode.One)
                {
                    if (keyboardState[(int)SilkKeyCode.W] > 0)
                    {
                        logic.GardenBeds[0]?.Water();
                        notificationMessage = "Watered bed 1!";
                    }
                    else if (keyboardState[(int)SilkKeyCode.H] > 0)
                    {
                        var harvestedName = logic.HarvestBed(0);
                        if (harvestedName != null) notificationMessage = $"Harvested {harvestedName}!";
                    }
                    else if (keyboardState[(int)SilkKeyCode.S] > 0)
                    {
                        logic.PlantSeed(0, 0); // Carrot
                        notificationMessage = "Planted Magic Carrot!";
                    }
                    else if (keyboardState[(int)SilkKeyCode.C] > 0)
                    {
                        if (logic.CraftPotion(0)) notificationMessage = "Crafted Speed Potion!";
                        else notificationMessage = "Need 2 Magic Carrots!";
                    }
                    else
                    {
                        var g = logic.SellPotion(0);
                        if (g > 0) notificationMessage = $"Sold Potion! +{g} gold!";
                    }
                }
                // ==================== STRATUL 2 (Tasta 2) ====================
                else if (key == SilkKeyCode.Two)
                {
                    if (keyboardState[(int)SilkKeyCode.W] > 0)
                    {
                        logic.GardenBeds[1]?.Water();
                        notificationMessage = "Watered bed 2!";
                    }
                    else if (keyboardState[(int)SilkKeyCode.H] > 0)
                    {
                        var harvestedName = logic.HarvestBed(1);
                        if (harvestedName != null) notificationMessage = $"Harvested {harvestedName}!";
                    }
                    else if (keyboardState[(int)SilkKeyCode.S] > 0)
                    {
                        logic.PlantSeed(1, 1); // Tomato
                        notificationMessage = "Planted Magic Tomato!";
                    }
                    else if (keyboardState[(int)SilkKeyCode.C] > 0)
                    {
                        if (logic.CraftPotion(1)) notificationMessage = "Crafted Fire Potion!";
                        else notificationMessage = "Need 2 Magic Tomatoes!";
                    }
                    else
                    {
                        var g = logic.SellPotion(1);
                        if (g > 0) notificationMessage = $"Sold Potion! +{g} gold!";
                    }
                }
                // ==================== STRATUL 3 (Tasta 3) ====================
                else if (key == SilkKeyCode.Three)
                {
                    if (keyboardState[(int)SilkKeyCode.W] > 0)
                    {
                        logic.GardenBeds[2]?.Water();
                        notificationMessage = "Watered bed 3!";
                    }
                    else if (keyboardState[(int)SilkKeyCode.H] > 0)
                    {
                        var harvestedName = logic.HarvestBed(2);
                        if (harvestedName != null) notificationMessage = $"Harvested {harvestedName}!";
                    }
                    else if (keyboardState[(int)SilkKeyCode.S] > 0)
                    {
                        logic.PlantSeed(2, 2); // Corn
                        notificationMessage = "Planted Magic Corn!";
                    }
                    else if (keyboardState[(int)SilkKeyCode.C] > 0)
                    {
                        if (logic.CraftPotion(2)) notificationMessage = "Crafted Sun Potion!";
                        else notificationMessage = "Need 2 Magic Corn!";
                    }
                    else
                    {
                        var g = logic.SellPotion(2);
                        if (g > 0) notificationMessage = $"Sold Potion! +{g} gold!";
                    }
                }
                // ==================== STRATUL 4 (Tasta 4) ====================
                else if (key == SilkKeyCode.Four)
                {
                    if (keyboardState[(int)SilkKeyCode.W] > 0)
                    {
                        logic.GardenBeds[3]?.Water();
                        notificationMessage = "Watered bed 4!";
                    }
                    else if (keyboardState[(int)SilkKeyCode.H] > 0)
                    {
                        var harvestedName = logic.HarvestBed(3);
                        if (harvestedName != null) notificationMessage = $"Harvested {harvestedName}!";
                    }
                    else if (keyboardState[(int)SilkKeyCode.S] > 0)
                    {
                        logic.PlantSeed(3, 3); // Cabbage
                        notificationMessage = "Planted Magic Cabbage!";
                    }
                    else if (keyboardState[(int)SilkKeyCode.C] > 0)
                    {
                        if (logic.CraftPotion(3)) notificationMessage = "Crafted Nature Potion!";
                        else notificationMessage = "Need 2 Magic Cabbage!";
                    }
                }
                else if (key == SilkKeyCode.U && state.CanUpgradeShop)
                {
                    logic.UpgradeShop();
                    notificationMessage = $"Shop upgraded to level {state.ShopLevel}!";
                }
            }

            if (key == SilkKeyCode.Escape)
            {
                await logic.SaveGameAsync();
                quit = true;
            }
        }
    }

    var now = DateTimeOffset.UtcNow;
    var deltaMs = (now - lastTime).TotalMilliseconds;
    lastTime = now;

    logic.Update(deltaMs);

    if (notificationMessage != null)
    {
        notificationTimer += deltaMs;
        if (notificationTimer > 2000)
        {
            notificationMessage = null;
            notificationTimer = 0;
        }
    }

    renderer.RenderGame(logic, notificationMessage);
}

unsafe
{
    sdl.DestroyWindow((Window*)window);
}
sdl.Quit();
// end AI-generated