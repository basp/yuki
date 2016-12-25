# yuki
Database migration tool. 

Apart from (hopefully) a useful tool this is also a sandbox for playing
around with C# 6 features and functional programming in C# in general.

# design
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
in the form of an expression instead of a series of statements.

# tutorial
The easiest way to understand the design is to dive in and implement a command.
We'll start with a command that reads the current Yuki version from the 
assembly file. We'll start with a `YukiVersionCommand.cs` file:

	namespace Yuki.Examples {
		public class YukiVersionCommand {
	
		}
	}

It's good practice to have a seperate **request** and **response** class for 
each command:

	public class YukiVersionRequest { }
	public class YukiVersionResponse { }

These classes can remain empty for now. At this point we can implement the
`ICommand<TReq,TRes,TEx>` interface for our `YukiVersionCommand` class:

	using System;
	using Optional;

	public class YukiVersionCommand
		: ICommand<YukiVersionRequest, YukiVersionResponse, Exception> 
	{
		public Option<YukiVersionResponse, Exception> Execute(
			YukiVersionRequest req)
		{
			var err = new NotImplementedException();
			return Option.None<YukiVersionResponse, Exception>(err);
		}
	}

But you can already see that is getting *pretty* verbose. And this is still a
simple command. Things will get pretty horrible when dealing with nested 
generic types. Luckily we can do a bit better by *aliasing* the **request** And
**response** types. And while we're at it, we'll make use of a C# 6 feature and
statically alias the `Optional.Option` type as well so we can say `Some` and
`None` instead of `Option.None` and `Option.Some`:


	using System;
	using Optional;

	using static Optional.Option;

	using Req = YukiVersionRequest;
	using Res = YukiVersionResponse;

	public class YukiVersionCommand
		: ICommand<Req, Res, Exception> 
	{
		public Option<Res, Exception> Execute(
			Req req)
		{
			var err = new NotImplementedException();
			return None<Res, Exception>(err);
		}
	}

And Now it all looks a lot tighter.

## nested commands
You often end up in the situation where you want to use one or more commands in
a new command (i.e. compose them together). This is natural and desired and
one of the whole points of having such a command structure in the first place. 

However, in C# things might get a bit messy. Consider a `FooCommand` that makes
use of our (still in development) `YukiVersionCommand` for instance:

	using System;
	using Optional;

	using static Optional.Option;

	using Req = FooRequest;
	using Res = FooResponse;

	public class FooCommand
		: ICommand<Req, Res, Exception>
	{
		private readonly ICommand<YukiVersionRequest,YukiVersionResponse,Exception> yukiVersionCommand;

		public FooCommand(
			ICommand<YukiVersionRequest,YukiVersionResponse,Exception> yukiVersionCommand)
		{
			this.yukiVersionCommand = yukiVersionCommand;
		}

		public Option<Res, Exception> Execute(
			Req req)
		{
			var err = new NotImplementedException();
			return None<Res, Exception>>(err);
		}
	} 

Ugh... Yuck! Holy carp, now those long types are back with a vengeance! 

In my opinion we can't reasonably alias them without making things too unreadable 
or ugly. We don't want to pass in a `YukiVersionCommand` type either because that 
would defeat the whole purpose of having interfaces in the first place. 

However, there's a nice and clean way out that just utilizes old school language
features. We'll just define a new interface:

	public interface IYukiVersionCommand
		: ICommand<YukiVersionRequest, YukiVersionResponse, Exception>
	{
	}

And now we can use that in our `FooCommand` constructor:

	using System;
	using Optional;

	using static Optional.Option;

	using Req = FooRequest;
	using Res = FooResponse;

	public class FooCommand
		: ICommand<Req, Res, Exception>
	{
		private readonly IYukiVersionCommand yukiVersionCommand;

		public FooCommand(
			IYukiVersionCommand yukiVersionCommand)
		{
			this.YukiVersionCommand = yukiVersionCommand;
		}

		public Option<Res, Exception> Execute(
			Req req)
		{
			var err = new NotImplementedException();
			return None<Res, Exception>>(err);
		}
	} 

Which should help with the enormous type verbosity that can sometimes crop when
dealing with C# in a functional way.

# finishing up
TODO: finish up the version command

# templates
This section will explain how Yuki deals with text templates which are an
important part of the core design.

Text templates implement the `ITextTemplate` interface which has a single
method `Option<string,Exception> Format()`. One thing to notice is that The
`Format` method takes no arguments. That means that templates are lightweight
and cannot (or should) be mutated after instantiation. 

In other words, it's expected that the `Format` method is idempotent, it will
return the same results every time. And even though no part of Yuki really
depends on this *gentlemens agreement* nor is it enforced you should take
care to follow it anyway.

To implement a template you can just implement the `ITextTemplate` interface
which is pretty easy:

	using System;
	using Optional;

	using static Optional.Option;

	public class FooTemplate
		: ITextTemplate
	{
		public FooTemplate(string key, string value)
		{
			this.Key = key;
			this.Value = value;
		}

		public string Key { get; }

		public string Value { get; }

		public Option<string, Exception> Format()
		{
			var str = $"{this.Key}={this.Value}";
			return Some<string,Exception>(str);
		}
	}

Note that all the values for the template are passed in via the only
constructor and that all the properties are read only. A neat feature of
C# 6 is that those properties are accessible from the constructor even
if they don't have a `set` or `private set` specified. 

# dealing with options
At first it might be weird to wrap your head around all these option values
that are returned. Again, an example might help. Usually, when doing things the
*imperative* way we write stuff like this:

	public T Execute<T>(ICommand<T> command)
	{
		try
		{
			return command.Execute(); 
		}
		catch(Exception ex)
		{
			this.log.Error(ex);
			throw;
		}
	}

And then we'd (hopefully) let that bubble up to a final handler that just
responds a general message back to the user that *something* went wrong.

Now with the `Option<T,TEx>` type we can do this instead:

	public Option<T, Exception> Execute<T>(ICommand<T> command)
	{
		try
		{
			var res = command.Execute();
			return Some<T, Exception>(res);
		}
		catch(Exception ex)
		{
			return None<T, Exception>(ex);
		}
	}

The main gain we got is that we started to *homogenize* our code. There's
no exceptional paths, nothing special. There's only one conceptual path 
through our code.

# options are viral
When you start to use the `Option` type you find that it's quite viral due to
its functional nature. Of course, there's lots of ways in which you can *force*
a value out of it (and I will show a few of them later) but when you use it In
the **right way** things *should* flow naturally. This is in a lot of ways
similar to how you would deal with exceptions but (as you'll see soon) you 
end up with a much more functional flow in the end.