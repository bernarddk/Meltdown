### Start of common code in Common.rb needed for IronRuby-CLR compatibility ###
require 'Meltdown.Core.dll'
include Meltdown::Core
include Meltdown::Core::Model
load_assembly 'Meltdown.Core'
### End of Common.rb ###
