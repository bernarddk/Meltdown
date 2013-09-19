### TODO: Move into common .rb code ###
def to_clr_string_array(list)
    System::Array[System::String].new(list.map { |s| s.to_s.to_clr_string })
end

require 'Meltdown.Core.dll'
include Meltdown::Core
include Meltdown::Core::Model
load_assembly 'Meltdown.Core'
### End TODO ###

InteractiveObject.new('Car', 'A shiny red car!', to_clr_string_array(['burn']))