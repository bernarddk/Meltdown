start = Area.new("Meadow", "Blue skies hover over a grassy-green meadow. You smell the stink of oil and smog from the south. A haze-shimmering shape hovers, belching black smoke.")

factory = Area.new("Dirty Factory", "A dirty, disgusting, factory. Rats scurry everywhere.");
factory.OnExit(Proc.new {
    puts "Thank GOD you're out of there! You force yourself to breathe again."
})

factory.AddTwoWayExit(Direction.North, start);

box = InteractiveObject.new("Box", "A strange, black metal box. It radiates with some sort of power ...");
box.AfterCommand('get', Proc.new {
    puts "Rays of white light explore from the box, singing your hands. With a yelp, you drop the box. It clatters to the floor, smoking."    
})

lollipop = InteractiveObject.new("Lollipop", "Lollipop, lollipop, oh lolllllly lolly pop ...", ['get']);

start.AddObject(box)
start.AddObject(lollipop)

start