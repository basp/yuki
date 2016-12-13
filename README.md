# Yuki
Yuki is mostly a place where I can hack and code on (hopefully useful) command
line utilities and control them all from a single client. And although it was 
primarely designed as a tool to service databases, it's also generally
useful when you want to hack in a real sandbox environment before moving your 
experiments elsewhere, like to a server for example.

Yuki fanatically uses the command pattern and it's extremely robust and simple
to extend the client this way. On top of that, it encourages heavy use of the
`option` type (in the form of `Optional.Option`) throughout all its API. By 
using these patterns together it's easy to extend as well as to compose new 
functionality by using the smaller commands that are allready baked in in a
very preditable and reasonable fashion.


# Legacy notes
## Script types
* `onetime` scripts are only executed one time and cause the migration to fail
when the system detects any changes after already running them.
* `anytime` scripts are run any time they have changes.
* `everytime` scripts are run every migration run.

Sometimes a script might be classified as `unknown` in which case it will be 
ignored.