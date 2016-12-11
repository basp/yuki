# Yuki
Yuki was primarely designed as a tool to easily manage and migrate your
databases but it has matured to be much more flexible.

# Setup
Once you have the binaries there should be a `Yuki.exe` somewhere. The first
thing you probably want to do is alias `yuki` to that executable:

	set-alias yuki path\to\yuki.exe

Almost every command involves a `server` argument so it's often useful to have
a variable for this:

	$server = "MAHPC\SQLEXPRESS"

Now it's easy to execute some commands, we could for example try to create a
database on that server:

	yuki createdatabase $server foo

This will create a new database named `foo` on the `$server` instance.

# Legacy
## Script types
* `onetime` scripts are only executed one time and cause the migration to fail
when the system detects any changes after already running them.
* `anytime` scripts are run any time they have changes.
* `everytime` scripts are run every migration run.

Sometimes a script might be classified as `unknown` in which case it will be 
ignored.