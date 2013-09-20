start = Area.new("Meadow", "Blue skies hover over a grassy-green meadow. You smell the stink of oil and smog from the south. A haze-shimmering shape hovers, belching black smoke.")

factory = Area.new("Dirty Factory", "A dirty, disgusting, factory. Rats scurry everywhere.");

factory.AddTwoWayExit(Direction.North, start);

start