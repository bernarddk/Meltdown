new Command('Eat', ['eat'], new CommandAction(function (target, instrument, preposition) {
    if (target == null || target == "") {
        return "Eat what?"
    } else {
        return "You eat the " + target.Name + ".";
    }
}));