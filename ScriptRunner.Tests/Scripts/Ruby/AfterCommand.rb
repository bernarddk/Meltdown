potato = InteractiveObject.new("Hot Potato", "A hot, hot potato. Steam billows from all sides.")
potato.AfterCommand("get", Proc.new {
    puts "You pick it up. Hot steam scorches your hand. You drop it. Dummy. I told you it was hot!"
    potato.Description = "A steaming, hand-burning potato"
})
potato