Command.new("Burn", ['burn'], Proc.new { |target, instrument, preposition|    
    if (target.nil? || target == "") then
        "Burn what?"
    else                 
        current_area.Objects.Remove(target)
        target.Destroy
        "You burn the #{target.Name}."
    end
})