
function foo()
    local foo = Character("Foo")
    foo:say("Hello World!")
    while true do
        coroutine.yield()
        foo:say("How are you?")
    end
end