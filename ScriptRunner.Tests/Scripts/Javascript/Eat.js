/** TODO: Move into common .js code **/
function to_clr_string_array(list) {
    var toReturn = [];
    for (var i = 0; i < list.length; i++) {
        toReturn.push(list[i].toString());
    }
}
/** END TODO **/

new Command('Eat', ['eat'], new CommandAction(function (target, instrument, preposition) {
    if (target == null || target == "") {
        return "Eat what?"
    } else {
        return "You eat the " + target.Name + ".";
    }
}));

// blah.constructor.name == "Array"