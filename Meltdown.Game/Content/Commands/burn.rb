def to_clr_string_array(list)
    System::Array[System::String].new(list.map { |s| s.to_s.to_clr_string })
end

require 'Meltdown.Core.dll'
include Meltdown::Core

known_commands << Command.new("Ruby Command", to_clr_string_array(['burn']), Proc.new { |target, instrument, preposition|
    puts "Command invoked with #{target}, #{instrument}, and #{preposition}"
})