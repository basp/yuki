# Yuki
Yuki is mostly a place where I can hack and code on (hopefully useful) command
line utilities and control them all from a single client. And although it was 
primarely designed as a tool to service databases, it's also generally
useful when you want to hack in a "real" environment before moving your 
experiments elsewhere, like to a server for example.

Yuki fanatically uses the command pattern and it's extremely robust and simple
to extend the client this way. On top of that, it encourages heavy use of the
`option` type (in the form of `Optional.Option`) throughout all its API. By 
using these patterns together it's easy to extend as well as to compose new 
functionality by using the smaller commands already baked in, in a
very predictable and reasonable fashion.

# Commands
The core unit of work in Yuki is the humble `ICommand<TReq,TRes,TEx>` type.
It has a single method:

	Option<TRes, TEx> Execute(TReq request);

By using the `Option<TRes, TEx>` type as a return value it becomes incredibly
easy to compose commands. 

Commands should be self sustained units. In fact, every command in Yuki 
(even if they are not accissible directly anymore) started out as a command
that could be directly invoked from the command-line. Even now that we have
composite commands like `Setup` internally, they just make use of the 
smaller `CreateDatabase` and `RestoreDatabase` commands. 

It's good practice to implement your commands in the form of smaller commands
instead of just some method somewhere. This will make sure they are composable
as well as easily tested/tried out.

# Requests
Every built-in `TReq` in Yuki is decorated with `PowerArgs` attributes. This
means that you can easily *hydrate* them from the command line. They are also
pure in the sense that they don't have side-effects. They might have a few 
utility members but these are always pure and safe to use in all cases.

# Responses
Every command should have a custom response type even if they are similar to
existing response types. Most of the built-in response types are or will be 
JSON serializable so that it is easy to interoperate with other tools.

For example, it's very easy to convert a lot of Yuki output to real objects
in PowerShell using the `ConvertFrom-Json` cmdlet. 

# Note
If there are clear hierarchies it's usually fine to
share some properties (Yuki does this as well on both the request and response
classes) but do be careful about not tangling your classes too much.

# Legacy notes
## Script types
* `onetime` scripts are only executed one time and cause the migration to fail
when the system detects any changes after already running them.
* `anytime` scripts are run any time they have changes.
* `everytime` scripts are run every migration run.

Sometimes a script might be classified as `unknown` in which case it will be 
ignored.