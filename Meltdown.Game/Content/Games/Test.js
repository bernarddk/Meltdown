var area = new Area("Strange Area", "A strange, mist-filled area. Is that Javascript I see?");

var coal = new InteractiveObject("coal", "a red-hot, burning coal. Go on, pick it up, you dummy.");

coal.AfterCommand("get", new Action(function () {
    Console.WriteLine("IT BURNSSSS!!!!");
}));

area.AddObject(coal);

Console.WriteLine("By the way, the player is " + player);
Console.WriteLine(" ... And the game is " + game);

area;