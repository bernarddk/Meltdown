potato = new InteractiveObject("Hot Potato", "A hot, hot potato. Steam billows from all sides.");

potato.AfterCommand("get", new Action(function() {
    //console.log("It burns us, preciousssss ...");
    potato.Description = "A steaming, hand-burning potato";
}));

potato