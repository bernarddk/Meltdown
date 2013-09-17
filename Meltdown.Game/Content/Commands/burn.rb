def to_clr_string_array(list)
    System::Array[System::String].new(list.map { |s| s.to_s.to_clr_string })
end

require 'Meltdown.Core.dll'
include Meltdown::Core
include Meltdown::Core::Model
load_assembly 'Meltdown.Core'

Command.new("Burn", to_clr_string_array(['burn']), Proc.new { |target, instrument, preposition|    
    if (target.nil? || target == "") then
        print "Burn what?"
    else         
        print "You burn the #{target.Name}."
        game.current_area.RemoveObject(target)
        target.Destroy
    end
})