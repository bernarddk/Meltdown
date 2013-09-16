def to_clr_string_array(list)
    System::Array[System::String].new(list.map { |s| s.to_s.to_clr_string })
end

require 'Meltdown.Core.dll'
include Meltdown::Core
include Meltdown::Core::Model
load_assembly 'Meltdown.Core'

Command.new("Ruby Command", to_clr_string_array(['burn']), Proc.new { |target, instrument, preposition|
    #puts "Command invoked with #{target}, #{instrument}, and #{preposition}"
    if (target.nil? || target == "") then
        puts "Burn what?"
    else         
        if target.Can('burn') then
            puts "You burn the #{target.Name}."
            game.current_area.RemoveObject(target)
            target.Destroy
        else
            puts "You can't burn #{target}!"
        end
    end
})