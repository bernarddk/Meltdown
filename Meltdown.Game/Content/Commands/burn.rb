def to_clr_string_array(list)
    System::Array[System::String].new(list.map { |s| s.to_s.to_clr_string })
end

require 'Meltdown.Core.dll'
include Meltdown::Core
include Meltdown::Core::Model

Command.new("Ruby Command", to_clr_string_array(['burn']), Proc.new { |target, instrument, preposition|
    #puts "Command invoked with #{target}, #{instrument}, and #{preposition}"
    if (target.nil? || target == "") then
        puts "Burn what?"
    else 
        target_object = nil        
        player.Inventory.each { |obj |
            if (obj.Name.downcase == target.downcase) then
                target_object = obj
            else
                puts "#{target.downcase} != #{obj.Name.downcase}"
            end
        }

        if (!target_object.nil?) then            
            if (!target_object.nil?) then
                if target_object.Can('burn') then
                    puts "You burn the #{target_object.Name}."
                    player.LoseObject(target_object)
                    target_object.Destroy
                else
                    puts "You can't burn #{target}!"
                end
            else
                puts "You don't have a #{target} to burn."
            end
        end
    end
})