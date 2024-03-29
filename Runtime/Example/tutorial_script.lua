local alex = Character("Alex", {}) -- Character("name", {} = extra properties you may use later by calling
-- alex:get("property") or alex:set("property", value)

alex:say("Hello World!") -- say("text") = prints text to the console
alex:say("How are you?")
callback = Choice("How are you?", { "Fine", "Not so good" }) -- Choice("text", {"choice1", "choice2", ...})
if callback == 1 then
    alex:say("That's good!")
else
    alex:say("That's bad!")
end

alex.say("So where do you want to go?")
callback = Choice("So where do you want to go?", { "Home", "School", "Work" })
if callback == 1 then
    alex:say("Okay, let's go home!")
    goto home_scene
elseif callback == 2 then
    alex:say("Okay, let's go to school!")
    goto school_scene
elseif callback == 3 then
    alex:say("Okay, let's go to work!")
    goto work_scene
end

:: home_scene ::
alex:say("We're home!")

:: school_scene ::
alex:say("We're at school!")

:: work_scene ::
alex:say("We're at work!")