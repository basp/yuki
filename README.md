# Yuki
Database migration tool.

# Design
The design of Yuki revolves heavily around **commands** and the use of the
**option** type. Commands implement the `ICommand<TReq,TRes,TEx>` interface
which has a single method `Option<TRes,TEx> Execute(TReq req)`.

> Because these signatures tend to become very verbose in C# very quickly
> you will often see the types of `TRes` and `TReq` aliased to `Req` and `Res`
> in the Yuki source code.

To be completely honest, Yuki is probably more functional than it should be. 
The things it needs to do are mostly imperative in nature and in that regard
it is very much still a big experiment in order to see how useful it actually
is to wrap it up in such a functionally flavored command structure.

You *do* get a lot of composability and (at least) for me it *does* become a
lot easier to think about the code when there's just a single flow of control
in the form of a giant expression. 

On the other hand you have to be careful though, especially when you start to 
use the funky LINQ syntax:

	return from x in firstCommand.Execute()
		   from y in secondCommand.Execute()
		   let temp = new { x, y }
		   from z in thirdCommand.Execute(temp)
		   select new { x, y, z }

